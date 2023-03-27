namespace ZetaShortPaths.UnitTests;

[TestClass]
public class FileInfoTest
{
    [TestMethod]
    public void TestExtensions()
    {
        var a = new FileInfo(@"C:\ablage\test.txt");

        var x = a.NameWithoutExtension();
        const string y = @"test";

        Assert.AreEqual(x, y);
    }

    [TestMethod]
    public void TestToString()
    {
        var a = new FileInfo(@"C:\ablage\test.txt");
        var b = new FileInfo(@"C:\ablage\test.txt");

        var x = a.ToString();
        var y = b.ToString();

        Assert.AreEqual(x, y);

        // --

        a = new(@"C:\ablage\");
        b = new(@"C:\ablage\");

        x = a.ToString();
        y = b.ToString();

        Assert.AreEqual(x, y);

        // --

        a = new(@"test.txt");
        b = new(@"test.txt");

        x = a.ToString();
        y = b.ToString();

        Assert.AreEqual(x, y);

        // --

        a = new(@"c:\ablage\..\ablage\test.txt");
        b = new(@"c:\ablage\..\ablage\test.txt");

        x = a.ToString();
        y = b.ToString();

        Assert.AreEqual(x, y);

        // --

        a = new(@"\ablage\test.txt");
        b = new(@"\ablage\test.txt");

        x = a.ToString();
        y = b.ToString();

        Assert.AreEqual(x, y);

        // --

        a = new(@"ablage\test.txt");
        b = new(@"ablage\test.txt");

        x = a.ToString();
        y = b.ToString();

        Assert.AreEqual(x, y);
    }

    [TestMethod]
    public void TestTilde()
    {
        // https://github.com/UweKeim/ZetaLongPaths/issues/24

        var path1 = ZspIOHelper.GetTempDirectory().CombineDirectory(Guid.NewGuid().ToString()).CheckCreate();
        var path2 = Directory.CreateDirectory(ZspPathHelper.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())!);
        try
        {
            var p1 = path1.CombineDirectory(@"a~b").CheckCreate();
            var p2 = Directory.CreateDirectory(ZspPathHelper.Combine(path2.FullName, @"a~b")!).FullName;

            var f1 = p1.CombineFile("1.txt");
            f1?.WriteAllText("1");

            var f2 = ZspPathHelper.Combine(p2, "1.txt");
            File.WriteAllText(f2!, "1");

            foreach (var file in p1.GetFiles())
            {
                Console.WriteLine(file.FullName);
            }

            foreach (var file in Directory.GetFiles(p2))
            {
                Console.WriteLine(file);
            }
        }
        finally
        {
            path1.SafeDelete();
            path2.Delete(true);
        }
    }
}