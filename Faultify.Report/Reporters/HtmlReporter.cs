using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Faultify.Report.Models;
using RazorLight;

namespace Faultify.Report.Reporters
{
    public class HtmlReporter : IReporter
    {
        public string FileExtension { get; } = ".html";

        /// <summary>
        ///     Create the report 
        /// </summary>
        /// <param name="mutationRun"></param>
        /// <returns></returns>
        public async Task<byte[]> CreateReportAsync(MutationProjectReportModel mutationRun)
        {
            return Encoding.UTF8.GetBytes(await PopulateTemplate(mutationRun));
        }

        /// <summary>
        ///     Populate the template of the report with the information from the mutation report model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<string> PopulateTemplate(MutationProjectReportModel model)
        {
            StreamReader streamReader = GetStreamReader("Html.cshtml"); 
            string template = await streamReader.ReadToEndAsync();

            RazorLightEngine engine = new RazorLightEngineBuilder()
                // required to have a default RazorLightProject type,
                // but not required to create a template from string.
                .UseEmbeddedResourcesProject(typeof(HtmlReporter))
                .UseMemoryCachingProvider()
                .Build();

            string result = await engine.CompileRenderStringAsync("templateKey", template, model);
            return result;
        }

        /// <summary>
        ///     Get the streamReader with the information of the currentAssembly
        /// </summary>
        /// <returns></returns>
        public static StreamReader GetStreamReader(string endsWith)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string resourceName = currentAssembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith(endsWith));
            StreamReader streamReader = new StreamReader(currentAssembly.GetManifestResourceStream(resourceName));
            return streamReader;
        }
    }
}
