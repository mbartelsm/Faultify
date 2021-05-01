using System;
using System.Collections.Generic;
using System.Linq;
using Faultify.Analyze.Mutation;
using Faultify.Analyze.MutationGroups;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NLog;

namespace Faultify.Analyze.Analyzers
{
    /// <summary>
    ///     Analyzer that searches for possible opcode mutations inside a method definition.
    ///     A list with opcodes definitions can be found here: https://en.wikipedia.org/wiki/List_of_CIL_instructions
    /// </summary>
    public abstract class OpCodeAnalyzer : IAnalyzer<OpCodeMutation, MethodDefinition>
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<OpCode, IEnumerable<(MutationLevel, OpCode)>> _mappedOpCodes;

        protected OpCodeAnalyzer(Dictionary<OpCode, IEnumerable<(MutationLevel, OpCode)>> mappedOpCodes)
        {
            _mappedOpCodes = mappedOpCodes;
        }

        public abstract string Description { get; }
        public abstract string Name { get; }

        public IMutationGroup<OpCodeMutation> GenerateMutations(
            MethodDefinition scope,
            MutationLevel mutationLevel,
            IDictionary<Instruction, SequencePoint> debug = null
        )
        {
            var mutGroup = new List<IEnumerable<OpCodeMutation>>();

            foreach (Instruction instruction in scope.Body?.Instructions)
            {
                OpCode original = instruction.OpCode;
                IEnumerable<OpCodeMutation> mutations;

                try
                {
                    IEnumerable<(MutationLevel, OpCode)> targets = _mappedOpCodes[original];
                    mutations =
                        from target
                            in targets
                        where mutationLevel.HasFlag(target.Item1)
                        select new OpCodeMutation(
                            instruction.OpCode,
                            target.Item2,
                            instruction,
                            scope);
                }
                catch (Exception e)
                {
                    _logger.Debug(e, $"Could not find key \"{original}\" in Dictionary.");
                    mutations = Enumerable.Empty<OpCodeMutation>();
                }
                mutGroup.Add(mutations);
            }

            return new MutationGroup<OpCodeMutation>
            {
                Name = Name,
                Description = Description,
                Mutations = mutGroup.SelectMany(x => x),
            };
        }
    }
}
