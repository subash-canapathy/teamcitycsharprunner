using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace CsharpCompiler
{
    internal class ExpressionCompiler : Compiler
    {
        public override bool CanCompile(string expression)
        {
            return !expression.Contains(";");
        }

        public override CompilerResults Compile(string expression)
        {
            var program = new StringBuilder()
                .Append(string.Join(Environment.NewLine, Namespaces.Select(n => "using " + n + ";")))
                .AppendLine()
                .AppendFormat(MainTemplate, expression, GeneratedFileName);

            return new CSharpCodeProvider()
                .CompileAssemblyFromSource(new CompilerParameters(References.ToArray())
                                               {
                                                   GenerateExecutable = true
                                               },
                                           program.ToString());
        }
    }
}