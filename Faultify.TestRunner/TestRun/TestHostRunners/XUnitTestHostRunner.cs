﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Faultify.MemoryTest.TestInformation;
using Faultify.TestRunner.Shared;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using TestResult = Faultify.TestRunner.Shared.TestResult;
using NLog;

namespace Faultify.TestRunner.TestRun.TestHostRunners
{
    public class XUnitTestHostRunner : ITestHostRunner
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly HashSet<string> _coverageTests = new HashSet<string>();
        private readonly string _testProjectAssemblyPath;
        private readonly TestResults _testResults = new TestResults();

        public XUnitTestHostRunner(string testProjectAssemblyPath)
        {
            _testProjectAssemblyPath = testProjectAssemblyPath;
        }

        public TestFramework TestFramework => TestFramework.XUnit;

        public async Task<TestResults> RunTests(TimeSpan timeout, IProgress<string> progress, IEnumerable<string> tests)
        {
            HashSet<string>? hashedTests = new HashSet<string>(tests);

            MemoryTest.XUnit.XUnitTestHostRunner? xunitHostRunner =
                new MemoryTest.XUnit.XUnitTestHostRunner(_testProjectAssemblyPath);
            xunitHostRunner.TestEnd += OnTestEnd;

            await xunitHostRunner.RunTestsAsync(CancellationToken.None, hashedTests);

            return _testResults;
        }

        public async Task<MutationCoverage> RunCodeCoverage(CancellationToken cancellationToken)
        {
            MemoryTest.XUnit.XUnitTestHostRunner? xunitHostRunner =
                new MemoryTest.XUnit.XUnitTestHostRunner(_testProjectAssemblyPath);
            xunitHostRunner.TestEnd += OnTestEndCoverage;

            await xunitHostRunner.RunTestsAsync(CancellationToken.None);

            return ReadCoverageFile();
        }

        private void OnTestEnd(object? sender, TestEnd e)
        {
            _testResults.Tests.Add(new TestResult { Name = e.TestName, Outcome = ParseTestOutcome(e.TestOutcome) });
        }

        private void OnTestEndCoverage(object? sender, TestEnd e)
        {
            _coverageTests.Add(e.FullTestName);
        }

        private TestOutcome ParseTestOutcome(MemoryTest.TestOutcome outcome)
        {
            try
            {
                return outcome switch
                {
                    MemoryTest.TestOutcome.Passed => TestOutcome.Passed,
                    MemoryTest.TestOutcome.Failed => TestOutcome.Failed,
                    MemoryTest.TestOutcome.Skipped => TestOutcome.Skipped,
                    _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null),
                };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error(ex, ex.Message + "defautling to Skipped");
                return TestOutcome.Skipped;
            }
        }

        private MutationCoverage ReadCoverageFile()
        {
            MutationCoverage? mutationCoverage = Utils.ReadMutationCoverageFile();

            mutationCoverage.Coverage = mutationCoverage.Coverage
                .Where(pair => _coverageTests.Contains(pair.Key))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            return mutationCoverage;
        }
    }
}
