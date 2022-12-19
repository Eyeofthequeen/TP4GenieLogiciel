using FileWalkerImmutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFileWalkerImmutable
{
    [TestClass]
    public class FileSystemTests
    {
        private FileSystemFacade fileSys;

        private IComponent root, secondFolder, thirdFolder;
        private IComponent file;



        [TestInitialize]
        public void Setup()
        {
            fileSys = new FileSystemFacade();

            root = fileSys.CreateFolder("rootFolder");
            secondFolder = fileSys.CreateFolder("secondFolder");
            thirdFolder = fileSys.CreateFolder("thirdFolder");
            file = fileSys.CreateFile("testFile", 10, "This is text content.");

        }

        // -------------------------------- Retriving --------------------------------

        [TestMethod]
        public void TestWhenPathExistThenFileIsRetrived()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, secondFolder.Name, file.Name);
            Assert.AreEqual(file, retrivedFile);
        }

        [TestMethod]
        public void TestWhenFolderExistThenFolderIsRetrived()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, thirdFolder);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name);
            Assert.AreEqual(thirdFolder, retrivedFolder);
        }

        [TestMethod]
        public void TestWhenPathDoestExistThenFileIsNotRetrived()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, file.Name);
            Assert.IsNull(retrivedFile);
        }

        [TestMethod]
        public void TestWhenPathDoestExistThenFolderIsNotRetrived()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, thirdFolder);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, thirdFolder.Name);
            Assert.IsNull(retrivedFolder);
        }
        
        // -------------------------------- Renaming --------------------------------

        [TestMethod]
        public void TestWhenFileIsRenamedRetrivingItByItsNewPathIsPossible()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            string newName = "fileRenamed";
            fileSys.Rename(file, newName);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, secondFolder.Name, newName);
            Assert.IsNotNull(retrivedFile);
        }

        [TestMethod]
        public void TestWhenFileIsRenamedRetrivingItByItsOldPathIsNotPossible()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, file);

            string newName = "fileRenamed";
            fileSys.Rename(file, newName);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, secondFolder.Name, file.Name);
            Assert.IsNull(retrivedFile);
        }

        [TestMethod]
        public void TestWhenFolderIsRenamedRetrivingItByItsNewPathIsPossible()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, thirdFolder, file);

            string newName = "SecondFolderInsideTestFolderRenamed";
            fileSys.Rename(secondFolder, newName);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, newName, thirdFolder.Name);
            Assert.IsNotNull(retrivedFolder);
        }

        [TestMethod]
        public void TestWhenFolderIsRenamedRetrivingItByItsOldPathIsNotPossible()
        {
            fileSys.AddChildren(root, secondFolder, file);
            fileSys.AddChildren(secondFolder, thirdFolder);

            string newName = "thirdFileRenamed";
            fileSys.Rename(thirdFolder, newName);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name);
            Assert.IsNull(retrivedFolder);
        }

        // -------------------------------- Deleting --------------------------------

        [TestMethod]
        public void TestWhenFileIsDeletedRetrivingItIsNotPossible()
        {
            fileSys.AddChildren(root, secondFolder, thirdFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, file.Name);
            Assert.AreEqual(retrivedFile, file);

            fileSys.Delete(retrivedFile);
            Assert.IsNull(fileSys.GetComponentByPath(root, file.Name));
        }

        [TestMethod]
        public void TestWhenFileIsCascadedInsideIsDeletedRetrivingItIsNotPossible()
        {
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, thirdFolder);
            fileSys.AddChildren(thirdFolder, file);

            IComponent retrivedFile = fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name, file.Name);
            Assert.AreEqual(retrivedFile, file);

            fileSys.Delete(retrivedFile);
            Assert.IsNull(fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name, file.Name));

        }

        [TestMethod]
        public void TestWhenFolderIsDeletedRetrivingItIsNotPossible()
        {
            fileSys.AddChildren(root, secondFolder, thirdFolder, file);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, thirdFolder.Name);
            Assert.AreEqual(retrivedFolder, thirdFolder);

            fileSys.Delete(retrivedFolder);
            Assert.IsNull(fileSys.GetComponentByPath(root, thirdFolder.Name));
        }


        [TestMethod]
        public void TestWhenFolderIsCascadedInsideIsDeletedRetrivingItIsNotPossible()
        {
            IComponent otherFolder = fileSys.CreateFolder("otherFolder");
            
            fileSys.AddChildren(root, secondFolder);
            fileSys.AddChildren(secondFolder, thirdFolder);
            fileSys.AddChildren(thirdFolder, otherFolder, file);

            IComponent retrivedFolder = fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name, otherFolder.Name);
            Assert.AreEqual(retrivedFolder, otherFolder);

            fileSys.Delete(retrivedFolder);
            Assert.IsNull(fileSys.GetComponentByPath(root, secondFolder.Name, thirdFolder.Name, otherFolder.Name));
        }
    }
}
