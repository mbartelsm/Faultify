using System;
using System.Collections.Generic;
using System.Linq;
using Faultify.Analyze.Mutation;
using Faultify.Analyze.MutationGroups;
using Mono.Cecil.Cil;
using NLog;

namespace Faultify.Analyze.Analyzers
{
    /// <summary>
    ///     Analyzer that searches for possible opcode mutations inside a method definition.
    ///     A list with opcodes definitions can be found here: https://en.wikipedia.org/wiki/List_of_CIL_instructions
    /// </summary>
    public abstract class OpCodeAnalyzer : IAnalyzer<OpCodeMutation, Instruction>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<OpCode, IEnumerable<(MutationLevel, OpCode, string)>> _mappedOpCodes;

        protected OpCodeAnalyzer(Dictionary<OpCode, IEnumerable<(MutationLevel, OpCode, string)>> mappedOpCodes)
        {
            _mappedOpCodes = mappedOpCodes;
        }

        public abstract string Description { get; }
        public abstract string Name { get; }
        public abstract string Id { get; }

        public IMutationGroup<OpCodeMutation> GenerateMutations(
            Instruction scope,
            MutationLevel mutationLevel,
            List<string> exclusions,
            IDictionary<Instruction, SequencePoint> debug = null
        )
        {
            OpCode original = scope.OpCode;
            IEnumerable<OpCodeMutation> mutations;

            int lineNumber = -1;
            if (debug != null)
            {
                Instruction prev = scope;
                SequencePoint seqPoint = null;
                // If prev is not null
                // and line number is not found
                // Try previous instruction.
                while (prev != null && !debug.TryGetValue(prev, out seqPoint))
                {
                    prev = prev.Previous;
                }

                if (seqPoint != null)
                {
                    lineNumber = seqPoint.StartLine;
                }
            }

            try
            {
                IEnumerable<(MutationLevel, OpCode, string)> targets = _mappedOpCodes[original];
                mutations =
                    from target
                        in targets
                    where mutationLevel.HasFlag(target.Item1) && !(exclusions.Contains(target.Item3))
                    select new OpCodeMutation(
                        scope.OpCode,
                        target.Item2,
                        scope,
                        lineNumber);
            }
            catch (Exception e)
            {
                _logger.Debug(e, $"Could not find key \"{original}\" in Dictionary.");
                mutations = Enumerable.Empty<OpCodeMutation>();
            }

            return new MutationGroup<OpCodeMutation>
            {
                Name = Name,
                Description = Description,
                Mutations = mutations,
            };
        }
    }
}
