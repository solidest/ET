using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ET.Interface
{
    //单个文档视图的接口
    public interface IViewDoc
    {

        //获取加载加载文件的UI
        UIElement GetUI();

        //获取最新的文档内容 返回的对象必须实现ICloneable接口
        byte[] GetNewDocContent();

        //文档被保存后调用 通常用来处理修改状态
        void AfterSaved();
        

        //命令系统
        bool CanCopy();
        void DoCopy();
        bool CanPaste();
        void DoPaste();
        bool CanRedo();
        void DoRedo();
        bool CanUndo();
        void DoUndo();

    }
}
