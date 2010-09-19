using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace CSharpCompiler
{
    interface ICompiler
    {
        CompilerResults Compile(string expression);
        bool CanCompile(string expression);
        IEnumerable<string> AdditionalNamespaces { set; }
        IEnumerable<string> AdditionalReferences { set; }
    }
}
