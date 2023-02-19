using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(ShowHolodecks.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(ShowHolodecks.BuildInfo.Company)]
[assembly: AssemblyProduct(ShowHolodecks.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + ShowHolodecks.BuildInfo.Author)]
[assembly: AssemblyTrademark(ShowHolodecks.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(ShowHolodecks.BuildInfo.Version)]
[assembly: AssemblyFileVersion(ShowHolodecks.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonInfo(typeof(ShowHolodecks.ShowHolodecks), ShowHolodecks.BuildInfo.Name, ShowHolodecks.BuildInfo.Version, ShowHolodecks.BuildInfo.Author, ShowHolodecks.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]