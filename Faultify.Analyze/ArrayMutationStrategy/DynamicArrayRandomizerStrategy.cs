﻿using System.Collections.Generic;
using Faultify.Core.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Faultify.Analyze.ArrayMutationStrategy
{
    /// <summary>
    ///     Contains Mutating Strategy for Dynamic Arrays.
    /// </summary>
    public class DynamicArrayRandomizerStrategy : IArrayMutationStrategy
    {
        private readonly ArrayBuilder _arrayBuilder;
        private readonly MethodDefinition _methodDefinition;
        private TypeReference _type;

        public DynamicArrayRandomizerStrategy(MethodDefinition methodDefinition)
        {
            _arrayBuilder = new ArrayBuilder();
            _methodDefinition = methodDefinition;
        }

        /// <summary>
        ///     Mutates a dynamic array by creating a new array with random values using the arraybuilder.
        /// </summary>
        public void Mutate()
        {
            ILProcessor processor = _methodDefinition.Body.GetILProcessor();
            _methodDefinition.Body.SimplifyMacros();

            var length = 0;
            List<Instruction> beforeArray = new List<Instruction>();
            List<Instruction> afterArray = new List<Instruction>();

            // Find array to replace
            foreach (Instruction instruction in _methodDefinition.Body.Instructions)
                // Add all instruction before dynamic array to list.
            {
                if (!instruction.IsDynamicArray())
                {
                    beforeArray.Add(instruction);
                }
                // If it's a dynamic array, add all instructions after the array initialisation.
                else
                {
                    beforeArray.Remove(instruction.Previous);
                    // Get type of array
                    _type = (TypeReference) instruction.Operand;

                    Instruction previous = instruction.Previous;
                    Instruction call = instruction.Next.Next.Next;
                    // Get length of array.
                    length = (int) previous.Operand;

                    // Add all other nodes to the list.
                    Instruction next = call.Next;
                    while (next != null)
                    {
                        afterArray.Add(next);
                        next = next.Next;
                    }

                    break;
                }
            }

            processor.Clear();

            // Append everything before array.
            foreach (Instruction before in beforeArray) processor.Append(before);

            List<Instruction> newArray = _arrayBuilder.CreateArray(processor, length, _type);

            // Append new array
            foreach (Instruction newInstruction in newArray) processor.Append(newInstruction);

            // Append after array.
            foreach (Instruction after in afterArray) processor.Append(after);
            _methodDefinition.Body.OptimizeMacros();
        }

        /// <summary>
        ///     Clears the mutated method body and pushes the original method body.
        /// </summary>
        /// <param name="mutatedMethodDef"></param>
        /// <param name="methodClone"></param>
        public void Reset(MethodDefinition mutatedMethodDef, MethodDefinition methodClone)
        {
            mutatedMethodDef.Body.Instructions.Clear();
            foreach (Instruction instruction in methodClone.Body.Instructions)
                mutatedMethodDef.Body.Instructions.Add(instruction);
        }
    }
}
