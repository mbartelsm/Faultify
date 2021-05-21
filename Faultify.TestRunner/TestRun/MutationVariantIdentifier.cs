using System.Collections.Generic;

namespace Faultify.TestRunner.TestRun
{
    /// <summary>
    ///     Single mutation variant that serves as an identifier for a particular possible mutation.
    /// </summary>
    public readonly struct MutationVariantIdentifier
    {
        public MutationVariantIdentifier(
            HashSet<string> testNames,
            string memberName,
            int mutationId,
            int mutationGroupId
        )
        {
            TestCoverage = testNames;
            MemberName = memberName;
            MutationId = mutationId;
            MutationGroupId = mutationGroupId;
        }

        public int MutationId { get; }
        public int MutationGroupId { get; }
        public string MemberName { get; }
        public HashSet<string> TestCoverage { get; }
    }
}
