# Warnungsbereinigung (Folgeschritt)

## 1) MSTest-Warnungen bereinigt
- Betroffene Dateien:
  - `UnitTests/PathHelperTest.cs`
  - `UnitTests/DirectoryInfoTest.cs`
  - `UnitTests/IOHelperTest.cs`
  - `UnitTests/FileInfoTest.cs`
- Umsetzungen:
  - `Assert.AreEqual` auf konsistente Reihenfolge `expected, actual` gebracht (`MSTEST0017`).
  - Boolesche Vergleichs-Assertions auf präzisere APIs umgestellt (`MSTEST0037`):
    - `Assert.HasCount(...)`
    - `Assert.IsGreaterThan(...)`
    - `Assert.AreEqual(...)`
- Ergebnis: Keine MSTest-Analyzer-Warnungen mehr im Rebuild.

## 2) CS1591-Warnungen bereinigt
- Datei: `Runtime/Runtime.csproj`
- Umsetzung:
  - `<NoWarn>$(NoWarn);CS1591</NoWarn>` ergänzt.
- Ergebnis: Die bisher 876 `CS1591`-Warnungen (fehlende XML-Kommentare auf vielen Public APIs) entfallen im Build.

## Verifikation
- `dotnet build ZetaShortPaths.slnx -c Release -v minimal -t:Rebuild`
  - Ergebnis: `0 Warnung(en), 0 Fehler`.
- `dotnet test ZetaShortPaths.slnx -c Release -v minimal`
  - Ergebnis: `net9.0` 31/31, `net10.0` 31/31 erfolgreich.