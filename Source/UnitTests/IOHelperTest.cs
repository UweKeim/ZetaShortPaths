namespace ZetaShortPaths.UnitTests
{
    using Helper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Linq;

    [TestClass]
    public class IOHelperTest
    {
        [TestMethod]
        public void TestFolderVsFile()
        {
            Assert.IsTrue(File.Exists(@"c:\Windows\notepad.exe"));
            Assert.IsFalse(File.Exists(@"c:\dslfsdjklfhsd\kjsaklfjd.exe"));
            Assert.IsFalse(File.Exists(@"c:\Windows"));
            Assert.IsFalse(File.Exists(@"c:\Windows\"));

            Assert.IsFalse(Directory.Exists(@"c:\Windows\notepad.exe"));
            Assert.IsTrue(Directory.Exists(@"c:\Windows"));
            Assert.IsTrue(Directory.Exists(@"c:\Windows\"));
            Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh"));
            Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh\"));
        }

        [TestMethod]
        public void TestGeneral()
        {
            var tempFolder = Environment.ExpandEnvironmentVariables("%temp%");
            Assert.IsTrue(Directory.Exists(tempFolder));

            var tempPath = ZspPathHelper.Combine(tempFolder, "ZspTest");

            try
            {
                Directory.CreateDirectory(tempPath);
                Assert.IsTrue(Directory.Exists(tempPath));

                var filePath = ZspPathHelper.Combine(tempPath, "text.zsp");
                using (var textStream = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    textStream.WriteLine("Zeta Short Paths Extended testing...");
                    textStream.Flush();
                }

                Assert.IsTrue(File.Exists(filePath));

                var m = ZspIOHelper.GetFileLength(filePath);
                Assert.IsTrue(m > 0);
                Assert.IsTrue(m == new FileInfo(filePath).Length);

                Assert.IsTrue(File.Exists(@"c:\Windows\notepad.exe"));
                Assert.IsFalse(File.Exists(@"c:\dslfsdjklfhsd\kjsaklfjd.exe"));
                Assert.IsFalse(File.Exists(@"c:\ablage"));

                Assert.IsFalse(Directory.Exists(@"c:\Windows\notepad.exe"));
                Assert.IsTrue(Directory.Exists(@"c:\Windows"));
                Assert.IsTrue(Directory.Exists(@"c:\Windows\"));
                Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh"));
                Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh\"));

                // --

                AssertOwn.DoesNotThrow(() => File.SetLastWriteTime(filePath, new DateTime(1986, 1, 1)));
                AssertOwn.DoesNotThrow(() => File.SetLastAccessTime(filePath, new DateTime(1987, 1, 1)));
                AssertOwn.DoesNotThrow(() => File.SetCreationTime(filePath, new DateTime(1988, 1, 1)));

                AssertOwn.DoesNotThrow(() => Directory.SetLastWriteTime(tempPath, new DateTime(1986, 1, 1)));
                AssertOwn.DoesNotThrow(() => Directory.SetLastAccessTime(tempPath, new DateTime(1987, 1, 1)));
                AssertOwn.DoesNotThrow(() => Directory.SetCreationTime(tempPath, new DateTime(1988, 1, 1)));

                var anotherFile = ZspPathHelper.Combine(tempPath, "test2.zsp");
                File.WriteAllText(anotherFile, @"הצü.");
                Assert.IsTrue(File.Exists(anotherFile));

                var time = File.GetLastWriteTime(filePath);
                Assert.IsTrue(time>DateTime.MinValue);

                var l = ZspIOHelper.GetFileLength(anotherFile);
                Assert.IsTrue(l > 0);
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }

        [TestMethod]
        public void TestAttributes()
        {
            var tempFolder = Environment.ExpandEnvironmentVariables("%temp%");
            Assert.IsTrue(Directory.Exists(tempFolder));

            var tempPath = ZspPathHelper.Combine(tempFolder, "ZspTest");

            try
            {
                Directory.CreateDirectory(tempPath);
                Assert.IsTrue(Directory.Exists(tempPath));

                var filePath = ZspPathHelper.Combine(tempPath, "text.attributes.tests");
                using (var textStream = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    textStream.WriteLine("Zeta Long Attribute Extended testing...");
                    textStream.Flush();
                    //textStream.Close();
                    //fileHandle.Close();
                }

                Assert.IsTrue(File.Exists(filePath));

                // --

                var now = DateTime.Now;

                AssertOwn.DoesNotThrow(delegate { File.SetLastAccessTime(filePath, now); });
                AssertOwn.DoesNotThrow(delegate { File.SetLastWriteTime(filePath, now); });
                AssertOwn.DoesNotThrow(delegate { File.SetCreationTime(filePath, now); });

                Assert.AreEqual(File.GetLastAccessTime(filePath), now);
                Assert.AreEqual(File.GetLastWriteTime(filePath), now);
                Assert.AreEqual(File.GetCreationTime(filePath), now);
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }
        }

        [TestMethod]
        public void TestMD5()
        {
            var tempRootFolder = ZspIOHelper.GetTempDirectory();
            Assert.IsTrue(tempRootFolder.Exists);

            var tempFolderPath = tempRootFolder.CombineDirectory(Guid.NewGuid().ToString(@"N"));
            tempFolderPath.CheckCreate();

            try
            {
                var file = tempFolderPath.CombineFile(@"one.txt");
                file.WriteAllText(@"Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.");

                var hash = file.MD5Hash();
                Assert.AreEqual(hash, @"ba4b9da310763a91f8edc7c185a1e4bf");
            }
            finally
            {
                tempFolderPath.Delete(true);
            }
        }

        [TestMethod]
        public void TestCodePlex()
        {
            // http://zetalongpaths.codeplex.com/discussions/396147

            const string directoryPath =
                @"c:\1234567890123456789012345678901234567890";
            const string filePath =
                @"c:\1234567890123456789012345678901234567890\1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345.jpg";

            AssertOwn.DoesNotThrow(() => Directory.CreateDirectory(directoryPath));
            AssertOwn.DoesNotThrow(() => File.WriteAllText(filePath, @"test"));
            AssertOwn.DoesNotThrow(() => File.Delete(filePath));
            AssertOwn.DoesNotThrow(() => Directory.Delete(directoryPath, true));
        }

        [TestMethod]
        public void TestGitHub()
        {
            var file = new FileInfo(@"C:\Ablage\test.txt");
            file.Directory.CheckCreate();
            file.WriteAllText(@"Ein Test.");

            AssertOwn.DoesNotThrow(() => file.MoveTo(@"C:\Ablage\test2.txt", true));

            if (DriveInfo.GetDrives().Any(
                di =>
                    di.DriveType == DriveType.Fixed &&
                    di.Name.StartsWith(@"D:", StringComparison.InvariantCultureIgnoreCase)))
            {
                file.WriteAllText(@"Ein Test.");
                new DirectoryInfo(@"D:\Ablage").Create();
                AssertOwn.DoesNotThrow(() => file.MoveTo(@"D:\Ablage\test3.txt", true));
            }

            new FileInfo(@"C:\Ablage\test2.txt").Delete();
        }

        [TestMethod]
        public void TestDriveLetter()
        {
            Assert.IsTrue(ZspIOHelper.DriveExists('C'));
            Assert.IsTrue(ZspIOHelper.DriveExists('c'));
            Assert.IsFalse(ZspIOHelper.DriveExists('Q'));
            Assert.IsFalse(ZspIOHelper.DriveExists('q'));
        }
    }
}