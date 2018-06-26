using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media.Imaging;
using ET.Doc;
using ET.Interface;

namespace ET.TestMap
{
    [Export(RevisionClass.ETModuleExportKey, typeof(ICommModule))]
    [ModuleHeader(ModuleKey, ModuleShowName, 1, ETModuleFileTypeEnum.CustomOnlyOne)]
    public class ModuleTestMap : ICommModule
    {

        #region --Property--

        static private BitmapImage _icon = null;

        #endregion

        public const string ModuleKey = "TestMap";
        public const string ModuleShowName = "测试环境";

        static  ModuleTestMap()
        {
            
            _icon = GetImaage("module.png");
        }

        public BitmapImage ModuleIcon => _icon;

        public BitmapImage FileIcon => _icon;

        public BitmapImage OpenModuleIcon => _icon;

        public object LoadFile(ModuleFile0 mf, int version)
        {
            if (version > 0) throw new ETException(ModuleKey, "程序版本过低，打开文档失败！");

            using (MemoryStream ms = new MemoryStream(mf.Content))
            {
                var formatter = new BinaryFormatter();
                formatter.Binder = new TestMapBinder();
                return formatter.Deserialize(ms);
            }
        }

        public IViewDoc OpenFile(ModuleFile0 mf, int version)
        {
            return new TestMapVM(LoadFile(mf, version) as TestMapData0, mf);
        }

        public IViewDoc OpenNewFile(string name)
        {
            var ret = new TestMapVM(new TestMapData0(ModuleTestMap.ModuleShowName), new ModuleFile0(ModuleTestMap.ModuleKey, ModuleTestMap.ModuleShowName));
            ret.UpdateContent();
            return ret;
        }

        #region --Helper--

        private static BitmapImage GetImaage(string imgName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("ModuleTestMap.images." + imgName);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = myStream;
            image.EndInit();
            myStream.Dispose();
            myStream.Close();
            return image;
        }

        #endregion

    }
}
