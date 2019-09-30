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
     var apiKey = EnvironmentVariable("NUGET_API_KEY");
     var apiUrl = EnvironmentVariable("NUGET_API_URL");
     bool apiKeyPresent = string.IsNullOrEmpty(apiKey);
     bool apiUrlPresent = string.IsNullOrEmpty(apiUrl);

     Information($"Key: {apiKeyPresent} | Url: {apiUrlPresent}");

     if (apiKeyPresent)
     {
         throw new InvalidOperationException("Could not resolve NuGet API key.");
     }

     if (apiUrlPresent)
     {
         throw new InvalidOperationException("Could not resolve NuGet API url.");
     }

     var packagePath = GetFiles((string)"./src/PSO/bin/Release/PSO.*.nupkg")
                        .Single()
                        .FullPath;
     Information((string)$"Publish {packagePath}");
     // Push the package.
     NuGetPush((FilePath)packagePath, (NuGetPushSettings)new NuGetPushSettings
     {
         ApiKey = apiKey,
         Source = apiUrl
     });
 });

Task("Clean")
  .Does(()=>
  {
     DotNetCoreClean("./PSO.sln");
     DeleteFiles("**/*.nupkg");
  });

RunTarget(target);
