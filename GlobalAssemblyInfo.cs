using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;


[assembly: ComVisible(false)]
[assembly: AssemblyCompany("ET")]
[assembly: AssemblyProduct(RevisionClass.ProductName)]
[assembly: AssemblyCopyright("Copyright ©  2018-2020 ET WorkBench")]
[assembly: AssemblyVersion(RevisionClass.FullVersion)]
[assembly: AssemblyFileVersion(RevisionClass.FullVersion)]


internal static class RevisionClass
{
	public const string AppMainVer = "0";   //主版本
	public const string AppSubVer = "1";    //小版本
	public const string DocVer = "0";       //文档版本
	public const string DevVer = "1";       //开发版本 正式发布版本均为0

	public const string FullVersion = AppMainVer + "." + AppSubVer + "." + DocVer + "." + DevVer;
    public const string ProductName = "ET WorkBench";
    public const string ETModuleExportKey = "ETModule";
}
