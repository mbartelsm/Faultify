using System;
using System.IO;
using System.Linq;
using CommandLine;
using Faultify.Analyze;
using Faultify.TestRunner;
using System.Text.Json;
using System.Collections.Generic;
using NLog;

// Disable Non-nullable is uninitialized, this is handled by the CommandLine package
#pragma warning disable 8618

namespace Faultify.Cli
{
    
    internal class Settings
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The path pointing to the test project project file.
        /// </summary>
        [Option('i', "inputProject", Required = true,
            HelpText = "The path pointing to the test project project file.")]
        public string TestProjectPath { get; set; }

        /// <summary>
        /// The root directory were the reports will be saved.
        /// </summary>
        [Option('r', "reportPath", Required = false, HelpText = "The path were the report will be saved.")]
        public string ReportPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "FaultifyOutput");

        /// <summary>
        /// Type of report to be generated, options: 'pdf', 'html', 'json'
        /// </summary>
        [Option('f', "reportFormat", Required = false, Default = "json",
            HelpText = "Type of report to be generated, options: 'pdf', 'html', 'json'")]
        public string ReportType { get; set; }

        /// <summary>
        /// Defines how many test sessions are ran at the same time.
        /// </summary>
        [Option('p', "parallel", Required = false, Default = 1,
            HelpText = "Defines how many test sessions are ran at the same time.")]
        public int Parallel { get; set; }

        /// <summary>
        /// The mutation level indicating the test depth.
        /// </summary>
        [Option('l', "mutationLevel", Required = false, Default = MutationLevel.Detailed,
            HelpText = "The mutation level indicating the test depth.")]
        public MutationLevel MutationLevel { get; set; }

        /// <summary>
        /// The name of the test host framework. options:  "NUnit", "XUnit", "MsTest", "DotnetTest".
        /// </summary>
        [Option('t', "testHost", Required = false, Default = nameof(TestHost.DotnetTest),
            HelpText = "The name of the test host framework.")]
        public string TestHostName { get; set; }

        /// <summary>
        /// Time out in seconds for each mutation.
        /// </summary>
        [Option('d', "timeOut", Required = false, Default = 0, HelpText = "Time out in seconds for the mutations")]
        public double Seconds { get; set; }

        /// <summary>
        /// Mutation groups to be excluded
        /// </summary>
        [Option('g', "excludeMutationGroups", Required = false, Default = null, HelpText = "Mutation groups to be excluded")]
        public IEnumerable<string> ExcludeMutationGroups { get; set;}

        /// <summary>
        /// Individual mutation ids to be excluded
        /// </summary>
        [Option('s', "excludeMutationSingles", Required = false, Default = null, HelpText = "Individual mutation ids to be excluded")]
        public IEnumerable<string> _excludeMutationSingles { get; set; }

        /// <summary>
        /// Path to a json file detailing individual mutations to be excluded
        /// </summary>
        [Option('e', "excludeMutationSinglesFile", Required = false, Default = "NoFile", HelpText = "Path to a json file detailing individual mutations to be excluded")]
        public string _excludeMutationSinglesFile { get; set; }

        /// <summary>
        /// Set of individual mutations that should be excluded
        /// </summary>
        public HashSet<string> ExcludeSingleMutations 
        { 
            get
            {
                if (ExcludeMutationSinglesFile.Equals("NoFile"))
                {
                    return ExcludeMutationSingles.ToHashSet<string>();
                }
                else
                {
                    string jsonString;
                    string[]? excludeMutations;
                    try
                    {
                        jsonString = File.ReadAllText(ExcludeMutationSinglesFile);
                        excludeMutations = JsonSerializer.Deserialize<string[]>(jsonString);
                    } catch(Exception ex)
                    {
                        _logger.Error(ex, "Defaulting to -s exclusions" + ex.Message);
                        excludeMutations = null;
                    }
                    
                    
                    if (excludeMutations == null)
                    {
                        return ExcludeMutationSingles.ToHashSet<string>();
                    }
                    else
                    {
                        return new HashSet<string>(excludeMutations);
                    }
                    
                }
            }
        }
        /// <summary>
        /// Time out in seconds for each mutation.
        /// </summary>
        public TimeSpan TimeOut => TimeSpan.FromSeconds(_seconds);

        /// <summary>
        /// The name of the test host framework. options:  "NUnit", "XUnit", "MsTest", "DotnetTest".
        /// </summary>
        public TestHost TestHost
        {
            get => Enum.Parse<TestHost>(TestHostName, true);
            set => TestHostName = value.ToString();
        }
    }
}
