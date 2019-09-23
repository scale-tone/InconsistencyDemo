//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target               = Argument("target", "Default");
var buildConfiguration   = Argument("configuration", "Debug");
var verbosity            = Argument("verbosity",     "Minimal");

//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////

var solutionName            = "DemoService";
var solution                = Directory($"../src/{solutionName}") + File($"{solutionName}.sln");

//////////////////////////////////////////////////////////////////////
// SETUP
//////////////////////////////////////////////////////////////////////
Task("Build")
    .Description($"Building {solutionName}")
    .IsDependentOn("NuGet-Restore")
    .Does(() =>
{
    MSBuild(
        solution,
        settings =>
        {
            settings
                .SetConfiguration(buildConfiguration)
                .SetVerbosity((Verbosity) Enum.Parse(typeof(Verbosity), verbosity))
                .WithTarget("Clean")
                .WithTarget("Build");
        });
});

Task("NuGet-Restore")
    .Description("Restoring NuGet packages")
    .Does(() =>
{
    NuGetRestore(solution);
});


Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
