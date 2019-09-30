var target = Argument("target", "Default");
var outputDir = "./bin";

Task("Default")
  .IsDependentOn("Build");

Task("Build")
  .IsDependentOn("NugetRestore")
  .Does(()=>
  {
    var settings = new DotNetCoreBuildSettings{Configuration = "Release", NoRestore = true};
    DotNetCoreBuild("./PSO.sln", settings);
  });

Task("NugetRestore")
  .IsDependentOn("Clean")
  .Does(()=>
  {
    DotNetCoreRestore();
  });

Task("Publish-NuGet")
  .IsDependentOn("Build")
  .Does(() =>
{
  // Resolve the API key.
  var apiKey = EnvironmentVariable("NUGET_API_KEY");
  if(string.IsNullOrEmpty(apiKey)) {
    throw new InvalidOperationException("Could not resolve NuGet API key.");
  }

  // Resolve the API url.
  var apiUrl = EnvironmentVariable("NUGET_API_URL");
  if(string.IsNullOrEmpty(apiUrl)) {
    throw new InvalidOperationException("Could not resolve NuGet API url.");
  }

  var packagePath = GetFiles("./src/PSO/bin/Release/PSO.*.nupkg")
                      .Single()
                      .FullPath;
  Information($"Publish {packagePath}");
  // Push the package.
//   NuGetPush(packagePath, new NuGetPushSettings {
//     ApiKey = apiKey,
//     Source = apiUrl
//   });
});

Task("Clean")
  .Does(()=>
  {
     DotNetCoreClean("./PSO.sln");
  });

RunTarget(target);
