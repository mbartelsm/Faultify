using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Environment;

namespace Faultify.Core.ProjectAnalyzing
{
    public class ProjectReader : IProjectReader
    {
        public Task<IProjectInfo> ReadProjectAsync(string path, IProgress<string> progress)
        {
            return Task.Run(() =>
            {
                return AnalyzeProject(path, progress);
            });
        }

        /// <summary>
        ///     Analyze the project and return the results
        /// </summary>
        /// <param name="path"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        private static IProjectInfo AnalyzeProject(string path, IProgress<string> progress)
        {
            AnalyzerManager analyzerManager = new AnalyzerManager();

            // TODO: This should add debug symbols to the build, which we can then access
            // via Cecil according to https://github.com/jbevain/cecil/wiki/Debug-symbols
            // analyzerManager.SetGlobalProperty("Configuration", "Debug");

            IProjectAnalyzer projectAnalyzer = analyzerManager.GetProject(path);
            progress.Report($"Building {Path.GetFileName(path)}");
            IAnalyzerResult analyzerResult = projectAnalyzer.Build(new EnvironmentOptions
            {
                DesignTime = false,
                Restore = true,
            })
                .First();

            return new ProjectInfo(analyzerResult);
        }
    }
}
