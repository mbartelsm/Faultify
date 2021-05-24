using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Faultify.Core.ProjectAnalyzing
{
    /// <summary>
    /// A dummy decompiler to use when a proper one is not available.
    /// </summary>
    public class NullDecompiler : ICodeDecompiler
    {
        public string Decompile(EntityHandle entityHandle)
        {
            return "SOURCE CODE COULD NOT BE GENERATED";
        }
    }
}
