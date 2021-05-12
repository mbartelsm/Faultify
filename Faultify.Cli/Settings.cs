using System;
using System.IO;
using System.Linq;
using CommandLine;
using Faultify.Analyze;
using Faultify.TestRunner;
using System.Text.Json;
using System.Collections.Generic;

// Disable Non-nullable is uninitialized, this is handled by the CommandLine package
#pragma warning disable 8618

namespace Faultify.Cli
{
    internal class Settings
    {
        [Option('i', "inputProject", Required = true,
            HelpText = "The path pointing to the test project project file.")]
        public string TestProjectPath { get; set; }

        [Option('r', "reportPath", Required = false, HelpText = "The path were the report will be saved.")]
        public string ReportPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "FaultifyOutput");

        [Option('f', "reportFormat", Required = false, Default = "json",
            HelpText = "Type of report to be generated, options: 'pdf', 'html', 'json'")]
        public string ReportType { get; set; }

        [Option('p', "parallel", Required = false, Default = 1,
            HelpText = "Defines how many test sessions are ran at the same time.")]
        public int Parallel { get; set; }

        [Option('l', "mutationLevel", Required = false, Default = MutationLevel.Detailed,
            HelpText = "The mutation level indicating the test depth. ")]
        public MutationLevel MutationLevel { get; set; }

        [Option('t', "testHost", Required = false, Default = nameof(TestHost.DotnetTest),
            HelpText = "The name of the test host framework.")]
        public string TestHostName { get; set; }

        [Option('d', "timeOut", Required = false, Default = 0, HelpText = "Time out in seconds for the mutations")]
        public double Seconds { get; set; }

        [Option('g', "excludeMutationGroups", Required = false, Default = null, HelpText = "Exclude mutations based on groupings")]
        public IEnumerable<string> ExcludeMutationGroups { get; set;}

        [Option('s', "excludeMutationSingles", Required = false, Default = null, HelpText = "Exclude mutations based on their individual id")]
        public IEnumerable<string> ExcludeMutationSingles { get; set; }

        [Option('e', "excludeMutationSinglesFile", Required = false, Default = "NoFile", HelpText = "Exclude mutations based on individual id in a JSON file")]
        public string ExcludeMutationSinglesFile { get; set; }

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
                    string jsonString = File.ReadAllText(ExcludeMutationSinglesFile);
                    string[]? excludeMutations = JsonSerializer.Deserialize<string[]>(jsonString);
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

        public TimeSpan TimeOut => TimeSpan.FromSeconds(Seconds);

        public TestHost TestHost
        {
            get => Enum.Parse<TestHost>(TestHostName, true);
            set => TestHostName = value.ToString();
        }
    }
}
