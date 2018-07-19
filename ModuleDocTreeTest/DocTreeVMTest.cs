using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ET.Doc;
using ET.Interface;

namespace ET.Main.DocTree
{
    [TestClass]
    public class DocTreeVMTest
    {
        [TestMethod]
        public void TestVM()
        {
            var vm = new DocTreeVM(new DirNode("TEST_Key", "TEST_Root"));
            Assert.AreEqual(ModuleDocTree.ModuleKey, vm.ModuleKey);
            Assert.AreEqual(ModuleDocTree.ModuleShowName, vm.MFile.FileName);
            Assert.IsNotNull(vm.PageUI);
            Assert.IsTrue(vm.IsAutoSave);
            vm.MFile.Content = null;
            Assert.IsNull(vm.MFile.Content);
            vm.SaveContent();
            Assert.IsNotNull(vm.MFile.Content);
        }
    }
}
