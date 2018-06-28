using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media.Imaging;
using ET.Doc;
using ET.Interface;

namespace ET.CodeEditor
{
    [Export(RevisionClass.ETModuleExportKey, typeof(ICommModule))]
    [ModuleHeader(ModuleKey, ModuleShowName, 10, ETModuleFileTypeEnum.DefaultMulti)]
    public class ModuleCodeEditor : ICommModule
    {
        public const string ModuleKey = "CodeEditor";
        public const string ModuleShowName = "代码文件";

        public BitmapImage ModuleIcon => null;

        public BitmapImage FileIcon => null;

        public BitmapImage OpenModuleIcon => null;

        public object LoadFile(ModuleFile mf, int version)
        {
            if (version > 0) throw new ETException(ModuleKey, "程序版本过低，打开文档失败！");

            using (MemoryStream ms = new MemoryStream(mf.Content))
            {
                var formatter = new BinaryFormatter();
                formatter.Binder = new CodeEditorBinder();
                return formatter.Deserialize(ms);
            }
        }

        public IViewDoc OpenFile(ModuleFile mf, int version)
        {
            return new CodeEditorVM(LoadFile(mf, version) as String, mf);
        }

        public IViewDoc OpenNewFile(string name)
        {
            var mf = new ModuleFile(ModuleCodeEditor.ModuleKey, name);
            var ret = new CodeEditorVM("", mf);
            ret.SaveContent();
            return ret;
        }
    }
}
