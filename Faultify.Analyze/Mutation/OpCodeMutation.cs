using Mono.Cecil.Cil;

namespace Faultify.Analyze.Mutation
{
    /// <summary>
    ///     Opcode mutation that can be performed or reverted.
    /// </summary>
    public class OpCodeMutation : IMutation
    {
        public OpCodeMutation(OpCode original, OpCode replacement, Instruction scope, int lineNumber = -1)
        {
            Original = original;
            Replacement = replacement;
            Scope = scope;
            LineNumber = lineNumber;
        }

        /// <summary>
        ///     The original opcode.
        /// </summary>
        private OpCode Original { get; set; }

        /// <summary>
        ///     The replacement for the original opcode.
        /// </summary>
        private OpCode Replacement { get; set; }

        private int LineNumber { get; set; }

        /// <summary>
        ///     Reference to the instruction line in witch the opcode can be mutated.
        /// </summary>
        private Instruction Scope { get; set; }

        public void Mutate()
        {
            Scope.OpCode = Replacement;
        }

        public void Reset()
        {
            Scope.OpCode = Original;
        }

        public string Report
        {
            get
            {
                if (LineNumber == -1)
                {
                    return $"Change operator from: '{Original}' to: '{Replacement}'.";
                }

                return $"Change operator from: '{Original}' to: '{Replacement}'. In line {LineNumber}";
            }
        }

        public bool HasOpcode(OpCode opCode)
        {
            return Scope.OpCode == opCode;
        }
    }
}
