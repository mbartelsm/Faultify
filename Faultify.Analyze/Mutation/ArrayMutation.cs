using Faultify.Analyze.ArrayMutationStrategy;
using Mono.Cecil;
using MonoMod.Utils;

namespace Faultify.Analyze.Mutation
{
    /// <summary>
    ///     Array Mutation, receives specific Strategy and MethodDefinition. The logic for the methods depends on a given
    ///     Strategy.
    /// </summary>
    public class ArrayMutation : IMutation
    {
        private readonly IArrayMutationStrategy _arrayMutationStrategy;
        private MethodDefinition Replacement { get; }
        private MethodDefinition Original { get; }

        public ArrayMutation(IArrayMutationStrategy mutationStrategy, MethodDefinition methodDef)
        {
            _arrayMutationStrategy = mutationStrategy;
            Original = methodDef;
            Replacement = Original.Clone();
        }

        /// <summary>
        ///     Mutates Array. Mutate logic depends on given Strategy.
        /// </summary>
        public void Mutate()
        {
            _arrayMutationStrategy.Mutate();
        }

        /// <summary>
        ///     Undo functionality for mutation array.
        /// </summary>
        public void Reset()
        {
            _arrayMutationStrategy.Reset(Original, Replacement);
        }

        public string Report => $"{Original} Change array contents.";
    }
}
