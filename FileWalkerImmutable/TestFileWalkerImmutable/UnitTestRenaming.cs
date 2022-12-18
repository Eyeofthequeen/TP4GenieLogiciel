using FileWalkerImmutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFileWalkerImmutable
{
    [TestClass]
    public class RenamingTests
    {
        private File file;
        private Folder folder;

        [TestInitialize]
        public void Setup()
        {
            file = new File("test", 100, "contenu du fichier.");
            folder = new Folder("test");
        }

        [TestMethod]
        public void TestWhenRenamingFileRetuningNewComponentFileOfTypeWithNewNameAndSameId()
        {
            string nameFile = "testing";
            IComponent newFile = file.Rename(nameFile);

            Assert.AreEqual(file.GetType(), newFile.GetType());
            Assert.AreNotEqual(newFile.Name, file.Name);
            Assert.AreEqual(newFile.ID, file.ID);
        }

        [TestMethod]
        public void TestWhenRenamingFolderReturningNewFolderWithNewNameAndSameId()
        {
            string nameFolder = "testing";
            IComponent newFolder = folder.Rename(nameFolder);

            Assert.AreEqual(folder.GetType(), newFolder.GetType());
            Assert.AreNotEqual(newFolder.Name, file.Name);
            Assert.AreEqual(newFolder.ID, folder.ID);
        }
    }
}
