using System.Collections;
using System.Collections.Generic;
using Faultify.Analyze.Mutation;

namespace Faultify.Analyze.MutationGroups
{
    /// <summary>
    ///     Groups a list of mutations along with some metadata for reporting purposes
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IMutation" /> that this class groups together</typeparam>
    public class MutationGroup<T> : IMutationGroup<T> where T : IMutation
    {
        // Collection of mutations
        public IEnumerable<T> Mutations { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Mutations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Mutations.GetEnumerator();
        }

        public override string ToString()
        {
            return $"Analyzed by {Name} ({Description})";
        }
    }
}
