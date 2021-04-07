﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Faultify.Analyze;
using Faultify.Analyze.AssemblyMutator;
using Faultify.Core.ProjectAnalyzing;
using Faultify.TestRunner.TestRun;

namespace Faultify.TestRunner.ProjectDuplication
{
    /// <summary>
    ///     A test project duplication.
    /// </summary>
    public class TestProjectDuplication : IDisposable
    {
        public TestProjectDuplication(FileDuplication testProjectFile,
            IEnumerable<FileDuplication> testProjectReferences, int duplicationNumber)
        {
            TestProjectFile = testProjectFile;
            TestProjectReferences = testProjectReferences;
            DuplicationNumber = duplicationNumber;
        }

        /// <summary>
        ///     Test project references.
        /// </summary>
        public IEnumerable<FileDuplication> TestProjectReferences { get; set; }

        /// <summary>
        ///     Test project file handle.
        /// </summary>
        public FileDuplication TestProjectFile { get; set; }

        /// <summary>
        ///     Number indicating which duplication this test project is.
        /// </summary>
        public int DuplicationNumber { get; }

        /// <summary>
        ///     Indicates if the test project is currently used by any test runner.
        /// </summary>
        public bool IsInUse { get; set; }

        public void Dispose()
        {
            TestProjectFile.Dispose();
            foreach (var fileDuplication in TestProjectReferences) fileDuplication.Dispose();
        }

        /// <summary>
        ///     Event that notifies when ever this test project is given free by a given test runner.
        /// </summary>
        public event EventHandler<TestProjectDuplication> TestProjectFreed;

        /// <summary>
        ///     Mark this project as free for any test runner.
        /// </summary>
        public void FreeTestProject()
        {
            //TODO: rename to MarkAsFree(). Currently, it gives the false impression that it actually does something to free the project.
            IsInUse = false;
            TestProjectFreed?.Invoke(this, this);
        }


        /// <summary>
        ///     Delete the test project completely
        ///     Currently does not work, given that Nunit restricts access to the files await's been given
        /// </summary>
        public void DeleteTestProject()
        {
            Directory.Delete(TestProjectFile.Directory, true);
        }

        /// <summary>
        ///     Returns a list of <see cref="MutationVariant" /> that can be executed on this given test project duplication.
        /// </summary>
        /// <param name="mutationIdentifiers"></param>
        /// <param name="mutationLevel"></param>
        /// <returns></returns>
        public IList<MutationVariant> GetMutationVariants(IList<MutationVariantIdentifier> mutationIdentifiers,
            MutationLevel mutationLevel)
        {
            List<MutationVariant> foundMutations = new List<MutationVariant>();

            foreach (var reference in TestProjectReferences)
            {
                // Read the reference and its contents
                using var stream = reference.OpenReadStream();
                using var binReader = new BinaryReader(stream);
                byte[] data = binReader.ReadBytes((int) stream.Length);

                var decompiler = new CodeDecompiler(reference.FullFilePath(), new MemoryStream(data));

                // Create assembly mutator and look up the mutations according to the passed identifiers.
                AssemblyMutator assembly = new AssemblyMutator(new MemoryStream(data));

                foreach (FaultifyTypeDefinition type in assembly.Types)
                {
                    // this might want to be moved outside.
                    // The operation is entirely a read operation that doesn't have to be done multiple times.
                    var toMutateMethods = new HashSet<string>(
                        mutationIdentifiers.Select(x => x.MemberName)
                    );

                    foreach (FaultifyMethodDefinition method in type.Methods)
                    {
                        //maybe move this out but probably impossibru
                        if (!toMutateMethods.Contains(method.AssemblyQualifiedName)) continue;

                        var methodMutationId = 0;

                        foreach (var mutationGroup in method.AllMutations(mutationLevel))
                        {
                            foreach (var mutation in mutationGroup)
                            {
                                MutationVariantIdentifier mutationIdentifier = mutationIdentifiers.FirstOrDefault(x =>
                                x.MutationId == methodMutationId && method.AssemblyQualifiedName == x.MemberName);

                                if (mutationIdentifier.MemberName != null)
                                {
                                    foundMutations.Add(new MutationVariant
                                    {
                                        Assembly = assembly,
                                        CausesTimeOut = false,
                                        MemberHandle = method.Handle,
                                        OriginalSource = decompiler.Decompile(method.Handle), // this might not be a good idea
                                        MutatedSource = "",
                                        Mutation = mutation,
                                        MutationAnalyzerInfo = new MutationAnalyzerInfo
                                        {
                                            AnalyzerDescription = mutationGroup.Description,
                                            AnalyzerName = mutationGroup.Name
                                        },
                                        MutationIdentifier = mutationIdentifier
                                    });
                                }

                                methodMutationId++;
                            }
                        }
                    }
                }
            }

            return foundMutations;
        }

        /// <summary>
        ///     Flush any changes made to the passed list of mutations to the file system.
        /// </summary>
        /// <param name="mutationVariants"></param>
        public void FlushMutations(IList<MutationVariant> mutationVariants)
        {
            var assemblies = new HashSet<AssemblyMutator>(mutationVariants.Select(x => x.Assembly));
            foreach (AssemblyMutator assembly in assemblies)
            {
                var fileDuplication = TestProjectReferences.FirstOrDefault(x =>
                    assembly.Module.Name == x.Name);
                try
                {
                    using var writeStream = fileDuplication.OpenReadWriteStream();
                    using var ms = new MemoryStream();
                    assembly.Module.Write(ms);
                    writeStream.Write(ms.ToArray());

                    ms.Position = 0;
                    var decompiler = new CodeDecompiler(fileDuplication.FullFilePath(), ms);

                    foreach (var mutationVariant in mutationVariants)
                        if (assembly == mutationVariant.Assembly && string.IsNullOrEmpty(mutationVariant.MutatedSource))
                            mutationVariant.MutatedSource = decompiler.Decompile(mutationVariant.MemberHandle);

                    } 
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    } 
                    finally
                    {
                        fileDuplication.Dispose();
                    }
            }

            TestProjectFile.Dispose();
        }
    }
}