namespace ZetaShortPaths.UnitTests
{
    [TestClass]
    public class PathHelperTest
    {
        [TestMethod]
        public void TestGeneral4()
        {
            var s1 = @"a";
            var s2 = @"b";

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s1);

            s1 = null;
            s2 = @"b";

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s2);

            s1 = null;
            s2 = null;

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s2);

            s1 = @"a";
            s2 = null;

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s1);

            s1 = string.Empty;
            s2 = @"b";

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s2);

            s1 = string.Empty;
            s2 = string.Empty;

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s2);

            s1 = @"a";
            s2 = string.Empty;

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s1);

            s1 = null;
            s2 = string.Empty;

            Assert.AreEqual(s1.NullOrEmptyOther(s2), s2);
        }

        [TestMethod]
        public void TestGeneral2()
        {
            var s1 =
                @"C:\Users\ukeim\Documents\Visual Studio 2008\Projects\Zeta Producer 9\Zeta Producer Main\Deploy\Origin\Enterprise\C-Allgaier\Windows\Packaging\Stationary\DEU\FirstStart\StandardProject";
            var s2 = Path.GetFullPath(s1);

            Assert.AreEqual(
                @"C:\Users\ukeim\Documents\Visual Studio 2008\Projects\Zeta Producer 9\Zeta Producer Main\Deploy\Origin\Enterprise\C-Allgaier\Windows\Packaging\Stationary\DEU\FirstStart\StandardProject",
                s2);

            // --

            s1 = @"c:\ablage\..\windows\notepad.exe";
            s2 = Path.GetFullPath(s1);

            Assert.AreEqual(@"c:\windows\notepad.exe", s2);

            //--

            s1 = @"lalala-123";
            s2 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(@"lalala-123", s2);

            //--

            s1 = @"lalala-123.txt";
            s2 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(@"lalala-123", s2);

            //--

            s1 = @"C:\Ablage\lalala-123.txt";
            s2 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(@"lalala-123", s2);

            //--

            s1 = @"\\nas001\data\folder\lalala-123.txt";
            s2 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(@"lalala-123", s2);

            //--

            s1 = @"c:\ablage\..\windows\notepad.exe";
            s2 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(@"notepad", s2);

            //--

            s1 = @"c:\ablage\..\windows\notepad.exe";
            s2 = ZspPathHelper.GetExtension(s1);

            Assert.AreEqual(@".exe", s2);

            //--

            //--

            s1 = @"c:\ablage\..\windows\notepad.file.exe";
            s2 = ZspPathHelper.GetExtension(s1);

            Assert.AreEqual(@".exe", s2);

            //--

            s1 = @"c:\ablage\..\windows\notepad.exe";
            s2 = ZspPathHelper.ChangeExtension(s1, @".com");

            Assert.AreEqual(@"c:\ablage\..\windows\notepad.com", s2);

            // --

            s1 = @"file.ext";
            s2 = @"c:\ablage\path1\path2";
            var s3 = @"c:\ablage\path1\path2\file.ext";
            var s4 = ZspPathHelper.GetAbsolutePath(s1, s2);

            Assert.AreEqual(s3, s4);

            var s5 = s1.MakeAbsoluteTo(new DirectoryInfo(s2));

            Assert.AreEqual(s3, s5);

            // --

            s1 = @"c:\folder1\folder2\folder4\";
            s2 = @"c:\folder1\folder2\folder3\file1.txt";
            s3 = ZspPathHelper.GetRelativePath(s1, s2);

            s4 = @"..\folder3\file1.txt";

            Assert.AreEqual(s3, s4);
        }

        [TestMethod]
        public void TestCompareWithFrameworkFunctions()
        {
            // --

            var s1 = ZspPathHelper.GetFileNameFromFilePath(@"/suchen.html");
            var s2 = Path.GetFileName(@"/suchen.html");

            Assert.AreEqual(s1, s2);

            // --

            s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"sitemap.xml");
            s2 = Path.GetDirectoryName(@"sitemap.xml");

            Assert.AreEqual(s1, s2);

            //s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"");
            //s2 = Path.GetDirectoryName(@"");

            //Assert.AreEqual(s1, s2);

            s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"c:\ablage\sitemap.xml");
            s2 = Path.GetDirectoryName(@"c:\ablage\sitemap.xml");

            Assert.AreEqual(s1, s2);

            s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"c:\ablage\");
            s2 = Path.GetDirectoryName(@"c:\ablage\");

            Assert.AreEqual(s1, s2);

            s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"c:\ablage");
            s2 = Path.GetDirectoryName(@"c:\ablage");

            Assert.AreEqual(s1, s2);

            s1 = ZspPathHelper.GetDirectoryPathNameFromFilePath(@"c:/ablage/sitemap.xml");
            s2 = Path.GetDirectoryName(@"c:/ablage/sitemap.xml");

            Assert.AreEqual(s1, s2);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1.txt";

            var s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            var s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1";

            s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1.";

            s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1.txt";

            s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1";

            s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1.";

            s3 = ZspPathHelper.GetFileNameFromFilePath(s1);
            s4 = Path.GetFileName(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1.txt";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"c:\folder1\folder2\folder3\file1.";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1.txt";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);

            // --

            s1 = @"file1.";

            s3 = Path.GetFileNameWithoutExtension(s1);
            s4 = Path.GetFileNameWithoutExtension(s1);

            Assert.AreEqual(s3, s4);
        }

        [TestMethod]
        public void TestEvenMoreFunctions()
        {
            var r = Path.GetFileNameWithoutExtension(@"\\?\C:\Simulazioni\Albero\scratch_file.txt");
            Assert.AreEqual(r, @"scratch_file");

            r = Path.GetFileNameWithoutExtension(@"\\?\C:\Simulazioni\Albero\scratch_file.");
            Assert.AreEqual(r, @"scratch_file");

            r = Path.GetFileNameWithoutExtension(@"\\?\C:\Simulazioni\Albero\scratch_file");
            Assert.AreEqual(r, @"scratch_file");
        }

        [TestMethod]
        public void TestGeneral3()
        {
            var s1 = @"c:\folder1\folder2\folder3\file1.txt";
            var s2 = ZspPathHelper.ChangeFileNameWithoutExtension(s1, @"file2");

            Assert.AreEqual(s2, @"c:\folder1\folder2\folder3\file2.txt");

            s1 = @"c:\folder1\folder2\folder3\file1.txt";
            s2 = ZspPathHelper.ChangeFileName(s1, @"file2.md");

            Assert.AreEqual(s2, @"c:\folder1\folder2\folder3\file2.md");
        }

        [TestMethod]
        public void TestGeneral5()
        {
            var s1 = @"c:\folder1\folder2\folder3\file1.txt";
            var s2 = @"c:\folder1\folder2\folder3\file1.txt";

            Assert.IsTrue(ZspPathHelper.AreSameFilePaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\file1.txt";
            s2 = @"c:\folder1\folder2\folder3\file2.txt";

            Assert.IsFalse(ZspPathHelper.AreSameFilePaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\file1.txt";
            s2 = @"c:\folder1\folder2\folder4\file1.txt";

            Assert.IsFalse(ZspPathHelper.AreSameFilePaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\folder4\..\file1.txt";
            s2 = @"c:\folder1\folder2\folder3\file1.txt";

            Assert.IsTrue(ZspPathHelper.AreSameFilePaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\folder4\..\..\folder3\file1.txt";
            s2 = @"c:\folder1\folder2\folder3\file1.txt";

            Assert.IsTrue(ZspPathHelper.AreSameFilePaths(s1, s2));
        }

        [TestMethod]
        public void TestGeneral6()
        {
            var s1 = @"c:\folder1\folder2\folder3";
            var s2 = @"c:\folder1\folder2\folder3";

            Assert.IsTrue(ZspPathHelper.AreSameFolderPaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3";
            s2 = @"c:\folder1\folder2\folder4";

            Assert.IsFalse(ZspPathHelper.AreSameFolderPaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\folder4\..";
            s2 = @"c:\folder1\folder2\folder3";

            Assert.IsTrue(ZspPathHelper.AreSameFolderPaths(s1, s2));

            s1 = @"c:\folder1\folder2\folder3\folder4\..\..\folder3";
            s2 = @"c:\folder1\folder2\folder3";

            Assert.IsTrue(ZspPathHelper.AreSameFolderPaths(s1, s2));
        }
    }
}