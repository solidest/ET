using System;
using ET.Doc;
using ET.Interface;
using ET.Service;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace ET.Main
{
    [Export(RevisionClass.ETModuleExportKey, typeof(ICommModule))]
    [ModuleHeader(ModuleKey,  "文档结构", 0, ETModuleFileTypeEnum.OnlyOneFile)]
    public class ModuleDocTree : ICommModule
    {


        public const String ModuleKey = "DocTree";

        //模块图标
        public BitmapImage ModuleIcon
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("ModuleDocTree.Images.dir.png");
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = myStream;
                image.EndInit();
                myStream.Dispose();
                myStream.Close();
                return image;
            }
        }

        //文件图标
        public  BitmapImage FileIcon
        {
            get
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("ModuleDocTree.Images.dir.png");
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = myStream;
                image.EndInit();
                myStream.Dispose();
                myStream.Close();
                return image;
            }
        }

        //新建空项目
        public IViewDoc OpenNewFile()
        {
            var rootNodes = new List<DirNode>(); 
            foreach (var m in ETService.MainService.ModulesHeaders)
            {
                if(m.ModuleKey != ModuleKey) //排除本模块
                {
                    var dir = new DirNode(m.ModuleKey, m.ModuleKey, null);
                    if (m.ModuleFileType == ETModuleFileTypeEnum.OnlyOneFile)
                    {
                        var vm = ETService.MainService.Modules[m.ModuleKey].OpenNewFile();
                        dir.SubModuleFiles.Add(vm.PageFile);
                        var ete = new ETEventArgs(ETPage.ETModuleFileSavedEvent, vm.PageUI, vm);
                        vm.PageUI.RaiseEvent(ete);
                    }
                    rootNodes.Add(dir);
                }
            }

            byte[] content = null;
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, rootNodes);
                content = ms.GetBuffer();
            }

            return new DocTreeVM(rootNodes);
        }

        //加载项目
        public IViewDoc OpenFile(ModuleFile mf, int version)
        {
            if (version > 0) throw new ETException(ModuleKey, "程序版本过低，打开文档失败！");
            var rootNodes = new List<DirNode>();
            using (MemoryStream ms = new MemoryStream(mf.Content))
            {
                var formatter = new BinaryFormatter();
                rootNodes = formatter.Deserialize(ms) as List<DirNode>;
            }
            return new DocTreeVM(rootNodes);
        }
    }
}
