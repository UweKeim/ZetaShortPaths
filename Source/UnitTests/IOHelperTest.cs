namespace ZetaShortPaths.UnitTests;

using Helper;

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

        var tempPath = ZspPathHelper.Combine(tempFolder, $@"ZspTest-{Guid.NewGuid():N}");

        try
        {
            Directory.CreateDirectory(tempPath!);
            Assert.IsTrue(Directory.Exists(tempPath));

            var filePath = ZspPathHelper.Combine(tempPath, "text.zsp");
            using (var textStream = new StreamWriter(new FileStream(filePath!, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                textStream.WriteLine("Zeta Short Paths Extended testing...");
                textStream.Flush();
            }

            Assert.IsTrue(File.Exists(filePath));

            var m = ZspIOHelper.GetFileLength(filePath);
            Assert.IsGreaterThan(0L, m);
            Assert.AreEqual(new FileInfo(filePath).Length, m);

            Assert.IsTrue(File.Exists(@"c:\Windows\notepad.exe"));
            Assert.IsFalse(File.Exists(@"c:\dslfsdjklfhsd\kjsaklfjd.exe"));
            Assert.IsFalse(File.Exists(@"c:\ablage"));

            Assert.IsFalse(Directory.Exists(@"c:\Windows\notepad.exe"));
            Assert.IsTrue(Directory.Exists(@"c:\Windows"));
            Assert.IsTrue(Directory.Exists(@"c:\Windows\"));
            Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh"));
            Assert.IsFalse(Directory.Exists(@"c:\fkjhskfsdhfjkhsdjkfhsdkjfh\"));

            // --

            AssertOwn.DoesNotThrow(() => File.SetLastWriteTime(filePath, new(1986, 1, 1)));
            AssertOwn.DoesNotThrow(() => File.SetLastAccessTime(filePath, new(1987, 1, 1)));
            AssertOwn.DoesNotThrow(() => File.SetCreationTime(filePath, new(1988, 1, 1)));

            AssertOwn.DoesNotThrow(() => Directory.SetLastWriteTime(tempPath, new(1986, 1, 1)));
            AssertOwn.DoesNotThrow(() => Directory.SetLastAccessTime(tempPath, new(1987, 1, 1)));
            AssertOwn.DoesNotThrow(() => Directory.SetCreationTime(tempPath, new(1988, 1, 1)));

            var anotherFile = ZspPathHelper.Combine(tempPath, "test2.zsp");
            File.WriteAllText(anotherFile!, @"äöü.");
            Assert.IsTrue(File.Exists(anotherFile));

            var time = File.GetLastWriteTime(filePath);
            Assert.IsTrue(time>DateTime.MinValue);

            var l = ZspIOHelper.GetFileLength(anotherFile);
            Assert.IsGreaterThan(0L, l);
        }
        finally
        {
            if (tempPath != null) Directory.Delete(tempPath, true);
        }
    }

    [TestMethod]
    public void TestAttributes()
    {
        var tempFolder = Environment.ExpandEnvironmentVariables("%temp%");
        Assert.IsTrue(Directory.Exists(tempFolder));

        var tempPath = ZspPathHelper.Combine(tempFolder, $@"ZspTest-{Guid.NewGuid():N}");

        try
        {
            Directory.CreateDirectory(tempPath!);
            Assert.IsTrue(Directory.Exists(tempPath));

            var filePath = ZspPathHelper.Combine(tempPath, "text.attributes.tests");
            using (var textStream = new StreamWriter(new FileStream(filePath!, FileMode.OpenOrCreate, FileAccess.Write)))
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
            if (tempPath != null) Directory.Delete(tempPath, true);
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
            file?.WriteAllText(@"Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.");

            var hash = file?.MD5Hash();
            Assert.AreEqual(@"ba4b9da310763a91f8edc7c185a1e4bf", hash);
        }
        finally
        {
            tempFolderPath?.Delete(true);
        }
    }

    [TestMethod]
    public void TestCodePlex()
    {
        // http://zetalongpaths.codeplex.com/discussions/396147

        var directoryPath =
            $@"c:\1234567890123456789012345678901234567890-{Guid.NewGuid():N}";
        var filePath =
            $@"{directoryPath}\1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345.jpg";

        AssertOwn.DoesNotThrow(() => Directory.CreateDirectory(directoryPath));
        AssertOwn.DoesNotThrow(() => File.WriteAllText(filePath, @"test"));
        AssertOwn.DoesNotThrow(() => File.Delete(filePath));
        AssertOwn.DoesNotThrow(() => Directory.Delete(directoryPath, true));
    }

    [TestMethod]
    public void TestGitHub()
    {
        var key = Guid.NewGuid().ToString(@"N");
        var sourcePath = $@"C:\Ablage\test-{key}.txt";
        var targetPath = $@"C:\Ablage\test2-{key}.txt";
        var dTargetPath = $@"D:\Ablage\test3-{key}.txt";

        var file = new FileInfo(sourcePath);
        file.Directory.CheckCreate();

        try
        {
            file.WriteAllText(@"Ein Test.");
            AssertOwn.DoesNotThrow(() => file.MoveTo(targetPath, true));

            if (DriveInfo.GetDrives().Any(
                    di =>
                        di.DriveType == DriveType.Fixed &&
                        di.Name.StartsWith(@"D:", StringComparison.InvariantCultureIgnoreCase)))
            {
                file.WriteAllText(@"Ein Test.");
                new DirectoryInfo(@"D:\Ablage").Create();
                AssertOwn.DoesNotThrow(() => file.MoveTo(dTargetPath, true));
            }
        }
        finally
        {
            if (File.Exists(sourcePath)) File.Delete(sourcePath);
            if (File.Exists(targetPath)) File.Delete(targetPath);
            if (File.Exists(dTargetPath)) File.Delete(dTargetPath);
        }
    }

    [TestMethod]
    public void TestSafeGetWithNullSearchPattern()
    {
        var tempRootFolder = ZspIOHelper.GetTempDirectory();
        var tempFolderPath = tempRootFolder.CombineDirectory(Guid.NewGuid().ToString(@"N"));
        tempFolderPath.CheckCreate();

        try
        {
            tempFolderPath.CombineDirectory(@"sub").CheckCreate();
            tempFolderPath.CombineFile(@"one.txt")!.WriteAllText(@"test");

            var dirs = ZspSafeFileOperations.SafeGetDirectories(tempFolderPath!.FullName, null);
            Assert.HasCount(1, dirs);

            var files = ZspSafeFileOperations.SafeGetFiles(tempFolderPath, null, SearchOption.TopDirectoryOnly);
            Assert.HasCount(1, files);
        }
        finally
        {
            tempFolderPath?.Delete(true);
        }
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
