﻿using System.IO;
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

        public async Task<byte[]> CreateReportAsync(MutationProjectReportModel mutationRun)
        {
            return Encoding.UTF8.GetBytes(await PopulateTemplate(mutationRun));
        }

        private async Task<string> PopulateTemplate(MutationProjectReportModel model)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string resourceName = currentAssembly
                .GetManifestResourceNames()
                .Single(str => str.EndsWith("Html.cshtml"));

            using StreamReader streamReader = new StreamReader(currentAssembly.GetManifestResourceStream(resourceName));
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
    }
}