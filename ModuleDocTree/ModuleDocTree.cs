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

namespace ET.Main.DocTree
{
    [Export(RevisionClass.ETModuleExportKey, typeof(ICommModule))]
    [ModuleHeader(ModuleKey, ModuleShowName, 0, ETModuleFileTypeEnum.DefaultOnlyOne)]
    public class ModuleDocTree : ICommModule
    {

        static private BitmapImage _folderIcon = null;
        static private BitmapImage _folderOpenIcon = null;
        static private BitmapImage _fileIcon = null;

        public const String ModuleKey = "DocTree";
        public const String ModuleShowName = "文档结构";


        public ModuleDocTree()
        {
            _folderIcon = GetImage("folder.png");
            _fileIcon = GetImage("file.png");
            _folderOpenIcon = GetImage("openfolder.png");
        }

        #region --For Icon--

        public static object GetFileIcon(string modeuleKey)
        {
            object ret = null;
            switch (ETService.MainService.ModulesHeaders[modeuleKey].ModuleFileType)
            {
                case ETModuleFileTypeEnum.CustomMulti:
                    ret = ETService.MainService.Modules[modeuleKey].FileIcon;
                    break;
                case ETModuleFileTypeEnum.CustomOnlyOne:
                    ret = ETService.MainService.Modules[modeuleKey].ModuleIcon;
                    break;
                default:
                    ret = _fileIcon;
                    break;
            }
            return ret;
        }

        public static object GetFolderIcon(string modeuleKey)
        {
            object ret = null;
            switch (ETService.MainService.ModulesHeaders[modeuleKey].ModuleFileType)
            {
                case ETModuleFileTypeEnum.CustomMulti:
                case ETModuleFileTypeEnum.CustomOnlyOne:
                    ret = ETService.MainService.Modules[modeuleKey].ModuleIcon;
                    break;
                default:
                    ret = _folderIcon;
                    break;
            }
            return ret;
        }


        public static object GetOpenFolderIcon(string modeuleKey)
        {
            object ret = null;
            switch (ETService.MainService.ModulesHeaders[modeuleKey].ModuleFileType)
            {
                case ETModuleFileTypeEnum.CustomMulti:
                case ETModuleFileTypeEnum.CustomOnlyOne:
                    ret = ETService.MainService.Modules[modeuleKey].OpenModuleIcon;
                    break;
                default:
                    ret = _folderOpenIcon;
                    break;
            }
            return ret;
        }



        private BitmapImage GetImage(string imgName)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("ModuleDocTree.Images."+ imgName);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = myStream;
            image.EndInit();
            myStream.Dispose();
            myStream.Close();
            return image;
        }

        #endregion

        //目录图标
        public BitmapImage ModuleIcon
        {
            get
            {
                return _folderIcon;
            }
        }

        //文件图标
        public  BitmapImage FileIcon
        {
            get
            {
                return _fileIcon;
            }
        }

        //目录打开图标
        public BitmapImage OpenModuleIcon
        {
            get
            {
                return _folderOpenIcon;
            }
        }

        //打开新项目
        public IViewDoc OpenNewFile(string name)
        {
            var rootNode = new DirNode(ModuleKey, ModuleShowName);
            foreach (var m in ETService.MainService.ModulesHeaders)
            {
                if(m.Key != ModuleKey) //排除本模块
                {
                    if (m.Value.ModuleFileType == ETModuleFileTypeEnum.CustomOnlyOne || m.Value.ModuleFileType == ETModuleFileTypeEnum.DefaultOnlyOne)
                    {
                        var vm = ETService.MainService.Modules[m.Key].OpenNewFile(name);
                        ETService.MainService.ShowModuleFile(vm);
                        rootNode.SubModuleFiles.Add(vm.MFile);
                    }
                    else
                    {
                        rootNode.SubDirNodes.Add(new DirNode(m.Key, m.Value.ModuleShowName));
                    }
                }
            }

            var ret = new DocTreeVM(rootNode);
            ret.SaveContent();
            return ret;
        }

        //加载项目
        public IViewDoc OpenFile(ModuleFile mf, int version)
        {
            var rootNode = (LoadFile(mf, version) as DirNode);
            return new DocTreeVM(rootNode);
        }

        public object LoadFile(ModuleFile mf, int version)
        {
            if (version > 0) throw new ETException(ModuleKey, "程序版本过低，打开文档失败！");

            using (MemoryStream ms = new MemoryStream(mf.Content))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(ms) ;
            }
        }
    }
}
