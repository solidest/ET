using System;
using ET.Doc;
using ET.ModuleInterface;
using System.ComponentModel.Composition;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace ET.Main
{
    [Export("ETModule", typeof(ICommModule))]
    [ModuleHeader("DocTree",  "文档结构", 0, true)]
    public class ModuleDocTree : ICommModule
    {
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

        public ModuleFile[] GetNewFiles()
        {
            throw new NotImplementedException();
        }

        public IViewDoc LoadFile(byte[] content, int version)
        {
            throw new NotImplementedException();
        }
    }
}
