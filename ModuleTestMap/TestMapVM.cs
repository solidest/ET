using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ET.Doc;
using ET.Interface;

namespace ET.TestMap
{
    public class TestMapVM : IViewDoc
    {

        private ModuleFile _mfile;
        private TestMapData0 _data;
        private TestMapPage _page;
        private bool _isModify;

        public TestMapVM(TestMapData0 data, ModuleFile mf)
        {
            _data = data;
            _mfile = mf;
            _page = (TestMapPage)System.Windows.Application.LoadComponent(new Uri("/ModuleTestMap;component/TestMapPage.xaml", System.UriKind.Relative));
            _page.disp.Text = _data.ToString();
            IsAutoSave = false;
        }

        #region --IViewDoc--

        public ETPage PageUI => _page;

        public ModuleFile MFile => _mfile;

        public string ModuleKey => ModuleTestMap.ModuleKey;

        public bool IsAutoSave { get; set; }
        public bool IsModify
        {
            get
            {
                return _isModify;
            }
            set
            {
                if (_isModify != value)
                {
                    _isModify = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsModify"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //序列化数据内容
        public void SaveContent()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, _data);
                _mfile.Content = ms.GetBuffer();
            }
        }

        #endregion
    }
}
