namespace ZetaShortPaths.UnitTests;

[TestClass]
public sealed class SafeFileOperationTest
{
	[TestMethod]
	public void Test01()
	{
		var fileName = $@"{Guid.NewGuid()}.txt";
		var filePath = Path.Combine(Path.GetTempPath(), fileName);
		File.WriteAllText(filePath, "File");

		Assert.IsTrue(File.Exists(filePath));

		// --

		ZspSafeFileOperations.SafeDeleteFile(filePath);

		Assert.IsFalse(File.Exists(filePath));
	}
}