namespace ZetaShortPaths.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class DirectoryInfoTest
    {
        [TestMethod]
        public void TestMove()
        {
            var path = ZspIOHelper.GetTempDirectory().CombineDirectory(Guid.NewGuid().ToString()).CheckCreate();
            try
            {
                var p1 = path.CombineDirectory(@"a").CheckCreate();
                var p2 = path.CombineDirectory(@"b");

                var f1 = p1.CombineFile("1.txt");
                f1.WriteAllText("1");

                try
                {
                    p1.MoveTo(p2);
                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }
            }
            finally
            {
                path.SafeDelete();
            }
        }

        [TestMethod]
        public void TestFolders()
        {
            var dirInfo1 = new DirectoryInfo(@"C:\Foo\Bar");
            Console.WriteLine(dirInfo1.Name); //"Bar"
            var dirInfo2 = new DirectoryInfo(@"C:\Foo\Bar\");
            Console.WriteLine(dirInfo2.Name); //"", an empty string

            var dirInfo3 = new DirectoryInfo(@"C:\Foo\Bar");
            Console.WriteLine(dirInfo1.Name);
            var dirInfo4 = new DirectoryInfo(@"C:\Foo\Bar\");
            Console.WriteLine(dirInfo2.Name);

            Assert.AreEqual(dirInfo1.Name, dirInfo3.Name);
            Assert.AreEqual(dirInfo2.Name, dirInfo4.Name);
        }

        [TestMethod]
        public void TestGetFileSystemInfos()
        {
            var path = ZspIOHelper.GetTempDirectory().CombineDirectory(Guid.NewGuid().ToString()).CheckCreate();
            try
            {
                var p1 = path.CombineDirectory(@"a").CheckCreate();
                path.CombineDirectory(@"b").CheckCreate();

                var f1 = p1.CombineFile("1.txt");
                f1.WriteAllText("1");

                Assert.IsTrue(path.GetFileSystemInfos().Length == 2);
                Assert.IsTrue(path.GetFileSystemInfos(SearchOption.AllDirectories).Length == 3);
                Assert.IsTrue(
                    path.GetFileSystemInfos(SearchOption.AllDirectories).Where(f => f is FileInfo).ToList().Count ==
                    1);
                Assert.IsTrue(
                    path.GetFileSystemInfos(SearchOption.AllDirectories)
                        .Where(f => f is DirectoryInfo)
                        .ToList()
                        .Count == 2);

            }
            finally
            {
                path.SafeDelete();
            }
        }

        [TestMethod]
        public void TestGeneral()
        {
            // Ordner mit Punkt am Ende.
            var dir = $@"C:\Ablage\{Guid.NewGuid():N}.";
            Assert.IsFalse(new DirectoryInfo(dir).Exists);
            new DirectoryInfo(dir).CheckCreate();
            Assert.IsTrue(new DirectoryInfo(dir).Exists);
            new DirectoryInfo(dir).Delete(true);
            Assert.IsFalse(new DirectoryInfo(dir).Exists);


            //Assert.IsTrue(new DirectoryInfo(Path.GetTempPath()).CreationTime>DateTime.MinValue);
            //Assert.IsTrue(new DirectoryInfo(Path.GetTempPath()).Exists);
            //Assert.IsFalse(new DirectoryInfo(@"C:\Ablage\doesnotexistjdlkfjsdlkfj").Exists);
            //Assert.IsTrue(new DirectoryInfo(Path.GetTempPath()).Exists);
            //Assert.IsFalse(new DirectoryInfo(@"C:\Ablage\doesnotexistjdlkfjsdlkfj2").Exists);
            //Assert.IsFalse(new DirectoryInfo(@"\\zetac11\C$\Ablage").Exists);
            //Assert.IsFalse(new DirectoryInfo(@"\\zetac11\C$\Ablage\doesnotexistjdlkfjsdlkfj2").Exists);

            const string s1 =
                @"C:\Users\Chris\Documents\Development\ADC\InterStore.NET\Visual Studio 2008\6.4.2\Zeta Resource Editor";
            const string s2 =
                @"C:\Users\Chris\Documents\Development\ADC\InterStore.NET\Visual Studio 2008\6.4.2\Web\central\Controls\App_LocalResources\ItemSearch";

            var s3 = ZspPathHelper.GetRelativePath(s1, s2);
            Assert.AreEqual(s3, @"..\Web\central\Controls\App_LocalResources\ItemSearch");

            var ext = ZspPathHelper.GetExtension(s3);
            Assert.AreEqual(ext ?? string.Empty, string.Empty);

            ext = ZspPathHelper.GetExtension(@"C:\Ablage\Uwe.txt");
            Assert.AreEqual(ext, @".txt");

            const string path = @"C:\Ablage\Test";
            Assert.AreEqual(
                new DirectoryInfo(path).Name,
                new DirectoryInfo(path).Name);

            Assert.AreEqual(
                new DirectoryInfo(path).FullName,
                new DirectoryInfo(path).FullName);

            const string filePath = @"C:\Ablage\Test\file.txt";
            var fn1 = new FileInfo(filePath).Directory?.FullName;
            var fn2 = new FileInfo(filePath).Directory.FullName;

            var fn1A = new FileInfo(filePath).DirectoryName;
            var fn2A = new FileInfo(filePath).DirectoryName;

            Assert.AreEqual(fn1, fn2);
            Assert.AreEqual(fn1A, fn2A);

            var fn = new DirectoryInfo(@"\\zetac11\C$\Ablage\doesnotexistjdlkfjsdlkfj2").Parent.FullName;

            Assert.AreEqual(fn, @"\\zetac11\C$\Ablage");

            fn = new DirectoryInfo(@"\\zetac11\C$\Ablage\doesnotexistjdlkfjsdlkfj2\").Parent.FullName;

            Assert.AreEqual(fn, @"\\zetac11\C$\Ablage");
        }

        [TestMethod]
        public void TestToString()
        {
            var a = new DirectoryInfo(@"C:\ablage\test.txt");
            var b = new DirectoryInfo(@"C:\ablage\test.txt");

            var x = a.ToString();
            var y = b.ToString();

            Assert.AreEqual(x, y);

            // --

            a = new DirectoryInfo(@"C:\ablage\");
            b = new DirectoryInfo(@"C:\ablage\");

            x = a.ToString();
            y = b.ToString();

            Assert.AreEqual(x, y);

            // --

            a = new DirectoryInfo(@"test.txt");
            b = new DirectoryInfo(@"test.txt");

            x = a.ToString();
            y = b.ToString();

            Assert.AreEqual(x, y);

            // --

            a = new DirectoryInfo(@"c:\ablage\..\ablage\test.txt");
            b = new DirectoryInfo(@"c:\ablage\..\ablage\test.txt");

            x = a.ToString();
            y = b.ToString();

            Assert.AreEqual(x, y);

            // --

            a = new DirectoryInfo(@"\ablage\test.txt");
            b = new DirectoryInfo(@"\ablage\test.txt");

            x = a.ToString();
            y = b.ToString();

            Assert.AreEqual(x, y);

            // --

            a = new DirectoryInfo(@"ablage\test.txt");
            b = new DirectoryInfo(@"ablage\test.txt");

            x = a.ToString();
            y = b.ToString();

            Assert.AreEqual(x, y);
        }
    }
}
