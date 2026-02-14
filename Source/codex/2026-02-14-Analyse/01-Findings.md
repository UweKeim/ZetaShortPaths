# Findings

## Hoch: Retry-Off-by-one
- Ort: `Runtime/ZspSimpleFileAccessProtector.cs:66`, `Runtime/ZspSimpleFileAccessProtector.cs:160`, `Runtime/ZspSimpleFileAccessProtector.cs:256`, `Runtime/ZspSimpleFileAccessProtector.cs:351`
- Problem: Die Bedingung `count++ > info.RetryCount` erlaubte einen zusätzlichen Fehlversuch.
- Auswirkung: Bei `RetryCount = 1` wurden effektiv 3 Versuche ausgeführt statt 2 (1 initial + 1 Retry).
- Maßnahme: Auf `++count > info.RetryCount` umgestellt.
- Absicherung: Neue Tests in `UnitTests/SimpleFileAccessProtectorTest.cs:9`, `UnitTests/SimpleFileAccessProtectorTest.cs:30`, `UnitTests/SimpleFileAccessProtectorTest.cs:51`, `UnitTests/SimpleFileAccessProtectorTest.cs:73`.

## Mittel: Nullable-Pattern in Safe-Enumerationen
- Ort: `Runtime/ZspSafeFileOperations.cs:522`, `Runtime/ZspSafeFileOperations.cs:602`
- Problem: Nullable-Parameter wurden ohne Fallback an APIs mit non-null `searchPattern` weitergegeben.
- Auswirkung: Potenziell `ArgumentNullException` bei `null` aus Aufrufercode.
- Maßnahme: Fallback auf `@"*"` ergänzt.
- Absicherung: Neuer Test in `UnitTests/IOHelperTest.cs:189`.

## Niedrig: Testkonfiguration
- Ort: `UnitTests/AssemblyInfo.cs:3`
- Problem: MSTest meldete vorher `MSTEST0001` (Parallelisierung nicht explizit definiert).
- Maßnahme: `[assembly: DoNotParallelize]` ergänzt, um explizites und reproduzierbares Verhalten festzulegen.