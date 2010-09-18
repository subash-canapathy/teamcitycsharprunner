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

        protected override void CreateProgram(string expression, StringBuilder program)
        {
            program.AppendFormat(MainTemplate, expression + ";");
        }
    }
}