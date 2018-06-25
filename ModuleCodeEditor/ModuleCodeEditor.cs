using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using ET.Doc;
using ET.Interface;

namespace ModuleCodeEditor
{
    [Export(RevisionClass.ETModuleExportKey, typeof(ICommModule))]
    [ModuleHeader(ModuleKey, ModuleShowName, 0, ETModuleFileTypeEnum.DefaultMulti)]
    public class ModuleCodeEditor : ICommModule
    {
        public const string ModuleKey = "CodeEditor";
        public const string ModuleShowName = "代码文件";

        public BitmapImage ModuleIcon => throw new NotImplementedException();

        public BitmapImage FileIcon => throw new NotImplementedException();

        public BitmapImage OpenModuleIcon => throw new NotImplementedException();

        public object LoadFile(ModuleFile0 mf, int version)
        {
            throw new NotImplementedException();
        }

        public IViewDoc OpenFile(ModuleFile0 mf, int version)
        {
            throw new NotImplementedException();
        }

        public IViewDoc OpenNewFile(string name)
        {
            throw new NotImplementedException();
        }
    }
}
