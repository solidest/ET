using System;
using System.Collections;
using System.Collections.Generic;

namespace ET.Doc
{

    //主文档，存储为二进制格式的文件
    [Serializable]
    public class MainDocument : IEnumerable<ModuleFile>
    {

        #region --Property--
        //文档版本
        public String FileVersion { get; }

        //文档内容列表
        private List<ModuleFile> _filecontents = new List<ModuleFile>();

        #endregion

        public MainDocument()
        {
            FileVersion = RevisionClass.DocVer;
        }

        public void AddFileContent(ModuleFile fc)
        {
            _filecontents.Add(fc);
        }

        public IEnumerator<ModuleFile> GetEnumerator()
        {
            return _filecontents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _filecontents.GetEnumerator();
        }

    }



}
