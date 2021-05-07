using System;
using System.Linq;
using System.Collections.Generic;
using NLog;

namespace Faultify.Report.Models
{
    public class TestProjectReportModel
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public TestProjectReportModel(string testProjectName, TimeSpan testSessionDuration)
        {
            TestProjectName = testProjectName;
            TestSessionDuration = testSessionDuration;
        }

        public IList<MutationVariantReportModel> Mutations { get; } = new List<MutationVariantReportModel>();
        public string TestProjectName { get; }
        public TimeSpan TestSessionDuration { get; private set; }

        public int MutationsSurvived { get; private set; }
        public int MutationsKilled { get; private set; }
        public int MutationsNoCoverage { get; private set; }
        public int MutationsTimedOut { get; private set; }

        public int TotalMutations { get; private set; }
        public int TotalTestRuns { get; private set; }

        public float ScorePercentage { get; set; }

        public void InitializeMetrics(int totalTestRuns, TimeSpan testSessionDuration)
        {
            TotalTestRuns = totalTestRuns;
            TestSessionDuration = testSessionDuration;

            TotalMutations = Mutations.Count;
            
            Dictionary<MutationStatus, int> results = Mutations
                .GroupBy(mutation => mutation.TestStatus)
                .ToDictionary(
                    keySelector: grouping => grouping.Key,
                    elementSelector: grouping => grouping.Count());
            foreach (MutationStatus mutationStatus in results.Keys)
            {
                _logger.Debug($"Mutation status: {mutationStatus}");
            }
            
            MutationsSurvived = results.GetValueOrDefault(MutationStatus.Survived, 0);
            MutationsKilled = results.GetValueOrDefault(MutationStatus.Killed, 0);
            MutationsTimedOut = results.GetValueOrDefault(MutationStatus.Timeout, 0);
            MutationsNoCoverage = results.GetValueOrDefault(MutationStatus.NoCoverage, 0);

            ScorePercentage = (int) (100.0 / TotalMutations * MutationsKilled);
        }
    }
}
