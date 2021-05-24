using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Faultify.Report.Models;
using RazorLight;
using WkHtmlToPdfDotNet;

namespace Faultify.Report.Reporters
{
    public class PdfReporter : IReporter
    {
        private static readonly BasicConverter Converter = new BasicConverter(new PdfTools());
        public string FileExtension => ".pdf";

        /// <summary>
        ///     Create the report pdf, by converting an html report to pdf 
        /// </summary>
        /// <param name="mutationRun"></param>
        /// <returns></returns>
        public async Task<byte[]> CreateReportAsync(MutationProjectReportModel mutationRun)
        {
            HtmlToPdfDocument doc = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4Plus,
                },
                Objects =
                {
                    new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = await PopulateTemplate(mutationRun),
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings =
                            { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 },
                    },
                },
            };

            return Converter.Convert(doc);
        }

        /// <summary>
        ///     Populate the template using the data from the mutation project report model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<string> PopulateTemplate(MutationProjectReportModel model)
        {
            StreamReader streamReader = HtmlReporter.GetStreamReader("Pdf.cshtml"); 
            string template = await streamReader.ReadToEndAsync();

            RazorLightEngine engine = new RazorLightEngineBuilder()
                // required to have a default RazorLightProject type,
                // but not required to create a template from string.
                .UseEmbeddedResourcesProject(typeof(PdfReporter))
                .UseMemoryCachingProvider()
                .Build();

            string result = await engine.CompileRenderStringAsync("templateKey", template, model);
            return result;
        }
    }
}
