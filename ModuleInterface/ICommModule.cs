using System;
using ET.Doc;
using System.Windows.Media.Imaging;

namespace ET.Interface
{
    public interface ICommModule
    {
        //新建初始化文件
        ModuleFile[] GetNewFiles();

        //加载制定版本的文件内容
        IViewDoc LoadFile(byte[] content, Int32 version);

        //模块图标
        BitmapImage ModuleIcon { get; }

        //模块关联文件图标
        BitmapImage FileIcon { get;}

    }
}
