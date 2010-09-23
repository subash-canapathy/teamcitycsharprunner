using System.Text;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    internal class ExpressionCompiler : AbstractCompiler
    {
        public ExpressionCompiler(IServiceMessages serviceMessages) : base(serviceMessages)
        {
        }

        public override bool CanCompile(string expression)
        {
            return !expression.Contains(";");
        }

        protected override void CreateProgram(string expression, StringBuilder program)
        {
            program.AppendFormat(MainTemplate, expression + ".Dump();");
        }
    }
}