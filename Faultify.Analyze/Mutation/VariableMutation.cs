using System;
using Mono.Cecil.Cil;

namespace Faultify.Analyze.Mutation
{
    public class VariableMutation : IMutation
    {
        public VariableMutation(Instruction instruction, Type type, int lineNumber = -1)
        {
            Original = instruction.Operand;
            Replacement = RandomValueGenerator.GenerateValueForField(type, Original);
            Variable = instruction;
            LineNumber = lineNumber;
        }
        /// <summary>
        ///     The original variable value.
        /// </summary>
        private object Original { get; set; }

        /// <summary>
        ///     The replacement for the variable value.
        /// </summary>
        private object Replacement { get; set; }

        /// <summary>
        ///     The replacement for the variable value.
        /// </summary>
        private int LineNumber { get; set; }

        /// <summary>
        ///     Reference to the variable instruction that can be mutated.
        /// </summary>
        private Instruction Variable { get; set; }

        public void Mutate()
        {
            Variable.Operand = Replacement;
        }

        public void Reset()
        {
            Variable.Operand = Original;
        }


        public string Report
        {
            get
            {
                if (LineNumber == -1)
                {
                    return $"Change variable from: '{Original}' to: '{Replacement}'.";
                }

                return $"Change variable from: '{Original}' to: '{Replacement}'. In line {LineNumber}";
            }
        }
    }
}
