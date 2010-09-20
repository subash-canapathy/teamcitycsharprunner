using System.Text;

namespace CsharpCompiler
{
    internal class ExpressionCompiler : AbstractCompiler
    {
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