using System;
using System.Collections.Generic;
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
        private TestMapData _data;
        private TestMapPage _page;

        public TestMapVM(TestMapData data, ModuleFile mf)
        {
            _data = data;
            _mfile = mf;
            _page = (TestMapPage)System.Windows.Application.LoadComponent(new Uri("/ModuleTestMap;component/TestMapPage.xaml", System.UriKind.Relative));
            _page.disp.Text = _data.ToString();
        }

        #region --IViewDoc--

        public ETPage PageUI => _page;

        public ModuleFile MFile => _mfile;

        public string ModuleKey => ModuleTestMap.ModuleKey;

        public bool IsAutoSave { get; set; }

        public void UpdateContent()
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
