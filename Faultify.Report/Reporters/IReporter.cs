using System.Threading.Tasks;
using Faultify.Report.Models;

namespace Faultify.Report.Reporters
{
    public interface IReporter
    {
        public string FileExtension { get; }
        public Task<byte[]> CreateReportAsync(MutationProjectReportModel mutationRun);
    }
}
