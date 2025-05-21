using System.Runtime.Loader;

namespace Fantasy;

public static class AssemblyHelper
{
    private const string HotfixDll = "Hotfix";
    private static AssemblyLoadContext? _context = null;

    public static System.Reflection.Assembly[] Assemblies
    {
        get
        {
            var assemblies = new System.Reflection.Assembly[2];
            assemblies[0] = typeof(AssemblyHelper).Assembly;
            assemblies[1] = LoadHotfixAssembly();
            return assemblies;
        }
    }

    private static System.Reflection.Assembly LoadHotfixAssembly()
    {
        if (_context != null)
        {
            _context.Unload();
            System.GC.Collect();
        }

        _context = new AssemblyLoadContext(HotfixDll, true);
        var dllBytes = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, $"{HotfixDll}.dll"));
        var pdbBytes = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, $"{HotfixDll}.pdb"));
        return _context.LoadFromStream(new MemoryStream(dllBytes), new MemoryStream(pdbBytes));
    }
}