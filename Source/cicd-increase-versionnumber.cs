//css_import ../_References/Submodules/zeta-shared-ci-assets/DevelopmentTools/CentralScripts/Afx.cs;

using System.Threading.Tasks;
using ZetaSoftware.CentralScripts;

public static class Processor
{
    public static async Task<int> Main()
    {
        Afx.Init();
        return await Afx.RunMain(async () => await processTasks());
    }

    private static async Task processTasks()
    {
        // Immer eines hochzählen.
        BuildUtilities.IncreaseVersionNumber();
    }
}