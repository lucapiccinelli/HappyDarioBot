using System.Reflection;

#if DEBUG
[assembly : AssemblyConfiguration("Debug Build")]
#else
[assembly: AssemblyConfiguration("Release Build")]
#endif

[assembly: AssemblyCompany("Luca Piccinelli")]
[assembly: AssemblyCopyright("Luca Piccinelli © 2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//http://stackoverflow.com/questions/64602/what-are-differences-between-assemblyversion-assemblyfileversion-and-assemblyin
[assembly: AssemblyVersion("1.5.0.4")]
[assembly: AssemblyFileVersion("1.5.0.4")]
[assembly: AssemblyInformationalVersion("1.5.0.4")]
