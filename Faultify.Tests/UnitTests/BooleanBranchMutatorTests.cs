﻿extern alias MC;
using System.IO;
using Faultify.Analyze.Analyzers;
using Faultify.Tests.UnitTests.Utils;
using MC::Mono.Cecil.Cil;
using NUnit.Framework;

namespace Faultify.Tests.UnitTests
{
    /// <summary>
    /// Test mutations of branching.
    /// </summary>
    internal class BooleanBranchMutatorTests
    {
        private readonly string _folder = Path.Combine("UnitTests", "TestSource", "BooleanTarget.cs");
        private readonly string _nameSpace = "Faultify.Tests.UnitTests.TestSource.BooleanTarget";


        [TestCase("BrTrue", true)]
        [TestCase("BrFalse", true)]
        public void BooleanBranch_PreMutation(string methodName, object expectedReturn)
        {
            // Arrange
            byte[] binary = DllTestHelper.CompileTestBinary(_folder);
            var expected = true;

            // Act

            using (DllTestHelper binaryInteractor = new DllTestHelper(binary))
            {
                var actual = (bool) binaryInteractor.DynamicMethodCall(_nameSpace, methodName.FirstCharToUpper(),
                    new[] { expectedReturn });

                // Assert
                Assert.AreEqual(expected, actual);
            }
        }

        [TestCase("BrTrue", nameof(OpCodes.Brfalse_S), true, false)]
        [TestCase("BrFalse", nameof(OpCodes.Brtrue_S), true, false)]
        [TestCase("BrTrue", nameof(OpCodes.Brfalse), true, true)]
        [TestCase("BrFalse", nameof(OpCodes.Brtrue), true, true)]
        public void BooleanBranch_PostMutation(
            string methodName,
            string expectedOpCodeName,
            object argument1,
            bool simplify
        )
        {
            // Arrange
            byte[] binary = DllTestHelper.CompileTestBinary(_folder);
            var expected = false;
            OpCode opCodeExpected = expectedOpCodeName.ParseOpCode();

            // Act
            byte[] mutatedBinary =
                DllTestHelper.MutateMethod<BranchingAnalyzer>(binary, methodName, opCodeExpected, simplify);
            using (DllTestHelper binaryInteractor = new DllTestHelper(mutatedBinary))
            {
                var actual =
                    (bool) binaryInteractor.DynamicMethodCall(_nameSpace, methodName.FirstCharToUpper(),
                        new[] { argument1 });

                // Assert
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
