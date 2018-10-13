///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("Build");

Task("Restore")
    .Does(()=>
    {
        DotNetCoreRestore();
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(()=>
    {
        MSBuild("./PSO.sln");
    });

Task("Clean")
    .Does(()=>
    {
        CleanDirectories(GetDirectories("./**/bin"));
        CleanDirectories(GetDirectories("./**/obj"));
    });

RunTarget(target);
