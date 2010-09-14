using System;
using System.CodeDom.Compiler;

namespace CsharpCompiler
{
    internal class ProgramCompiler : Compiler
    {
        public override bool CanCompile(string expression)
        {
            return false;
        }

        public override CompilerResults Compile(string expression)
        {
            throw new NotImplementedException();
        }
    }
}