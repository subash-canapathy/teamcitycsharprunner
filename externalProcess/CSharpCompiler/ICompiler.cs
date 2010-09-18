using System;
using System.CodeDom.Compiler;

namespace CSharpCompiler
{
    interface ICompiler
    {
        CompilerResults Compile(string expression);
        bool CanCompile(string expression);
    }
}
