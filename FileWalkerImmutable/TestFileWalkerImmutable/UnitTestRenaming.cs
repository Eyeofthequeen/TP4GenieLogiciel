using FileWalkerImmutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFileWalkerImmutable
{
    [TestClass]
    public class RenamingTests
    {
        private FileSystemFacade fileSys;

        [TestInitialize]
        public void Setup()
        {
            fileSys = new FileSystemFacade();
        }

        [TestMethod]
        public void TestWhenPathExistThenFileIsRetrived()
        {
            IComponent folder = fileSys.CreateFolder("testFolder");
            IComponent secondFolder = fileSys.CreateFolder("testInsideTestFolder");
            IComponent file = fileSys.CreateFile("testFile", 10, "This is text content.");

            fileSys.AddChildren(folder, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(folder, secondFolder.Name, file.Name);
            Assert.AreEqual(retrivedFile.Name, "testFile");
        }

        [TestMethod]
        public void TestWhenPathDoestExistThenFileIsNotRetrived()
        {
            IComponent folder = fileSys.CreateFolder("testFolder");
            IComponent secondFolder = fileSys.CreateFolder("testInsideTestFolder");
            IComponent file = fileSys.CreateFile("testFile", 10, "This is text content.");

            fileSys.AddChildren(folder, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(folder, file.Name);
            Assert.AreEqual(retrivedFile, null);
        }
    }
}
