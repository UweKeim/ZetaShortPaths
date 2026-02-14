# Umgesetzte Änderungen

## Runtime
- `Runtime/ZspSimpleFileAccessProtector.cs`
  - Retry-Bedingungen in allen 4 Protect-Varianten korrigiert.
- `Runtime/ZspSafeFileOperations.cs`
  - Null-Fallback für `searchPattern` ergänzt:
    - `SafeGetDirectories(string? folderPath, string? searchPattern)`
    - `SafeGetFiles(DirectoryInfo? folderPath, string? searchPattern, SearchOption searchOption)`

## UnitTests
- `UnitTests/SimpleFileAccessProtectorTest.cs` (neu)
  - Sync/Generic/Async Retry-Verhalten verifiziert.
  - Pass-Through-Markierung verifiziert.
- `UnitTests/IOHelperTest.cs`
  - `TestSafeGetWithNullSearchPattern` ergänzt.
- `UnitTests/AssemblyInfo.cs` (neu)
  - `[assembly: DoNotParallelize]` ergänzt.

## Verifikation
- Ausgeführt: `dotnet test ZetaShortPaths.slnx -c Release -v minimal`
- Ergebnis: erfolgreich für `net9.0` und `net10.0` (31/31).