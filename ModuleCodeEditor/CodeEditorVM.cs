using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ET.Doc;
using ET.Interface;

namespace ET.CodeEditor
{
    public class CodeEditorVM : IViewDoc
    {
        private ModuleFile _mf;
        private string _code;
        private CodeEditorPage _page;

        #region --IViewDoc--

       public CodeEditorVM(string code, ModuleFile mf)
        {
            _mf = mf;
            _code = code;
            _page = (CodeEditorPage)System.Windows.Application.LoadComponent(new Uri("/ModuleCodeEditor;component/CodeEditorPage.xaml", System.UriKind.Relative));
            _page.textEditor.Text = _code;
            IsAutoSave = false;
        }
        public ETPage PageUI => _page;

        public ModuleFile MFile => _mf;

        public string ModuleKey => ModuleCodeEditor.ModuleKey;

        public bool IsAutoSave { get; set; }

        public void UpdateContent()
        {
            _code = _page.textEditor.Text;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _code);
                _mf.Content = ms.GetBuffer();
            }
        }

        #endregion
 
    }
}
