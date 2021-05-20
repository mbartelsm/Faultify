using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Environment;
using Faultify.Core.Exceptions;
using NLog;

namespace Faultify.Core.ProjectAnalyzing
{
    public class ProjectReader : IProjectReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Task<IProjectInfo> ReadProjectAsync(string path, IProgress<string> progress)
        {
            return Task.Run<IProjectInfo>(() =>
            {
                AnalyzerManager analyzerManager = new AnalyzerManager();

                // TODO: This should add debug symbols to the build, which we can then access
                // via Cecil according to https://github.com/jbevain/cecil/wiki/Debug-symbols
                analyzerManager.SetGlobalProperty("Configuration", "Debug");

                IProjectAnalyzer projectAnalyzer = analyzerManager.GetProject(path);
                progress.Report($"Building {Path.GetFileName(path)}");

                ProjectInfo result = null;
                
                try
                {
                    IAnalyzerResults analyzerResults = projectAnalyzer.Build(new EnvironmentOptions
                    {
                        DesignTime = false,
                        Restore = true,
                    });

                    if (!analyzerResults.OverallSuccess)
                    {
                        throw new ProjectNotBuiltException();
                    }

                    result = new ProjectInfo(analyzerResults.First(r => r.Succeeded));
                }
                catch (ProjectNotBuiltException e)
                {
                    Logger.Fatal(e, "Faultify was unable to build any targets for the provided project. Terminating program");
                    Environment.Exit(-1);

                    return null;
                }
                catch (InvalidOperationException e)
                {
                    Logger.Fatal(e, "Could not find any target frameworks to build the project for. Terminating program");
                    Environment.Exit(-1);

                    return null;
                }

                return result;
            });
        }
    }
}
