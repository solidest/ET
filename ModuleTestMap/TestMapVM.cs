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

        private ModuleFile0 _mfile;
        private TestMapData0 _data;
        private TestMapPage _page;

        public TestMapVM(TestMapData0 data, ModuleFile0 mf)
        {
            _data = data;
            _mfile = mf;
            _page = (TestMapPage)System.Windows.Application.LoadComponent(new Uri("/ModuleTestMap;component/TestMapPage.xaml", System.UriKind.Relative));
            _page.disp.Text = _data.ToString();
        }

        #region --IViewDoc--

        public ETPage PageUI => _page;

        public ModuleFile0 MFile => _mfile;

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
