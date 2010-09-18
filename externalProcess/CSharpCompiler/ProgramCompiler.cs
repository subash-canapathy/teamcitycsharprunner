using System.Text;

namespace CsharpCompiler
{
    internal class ProgramCompiler : Compiler
    {
        public override bool CanCompile(string expression)
        {
            return ContainsClassDefinition(expression) && ContainsMainMethod(expression);
        }

        protected override void CreateProgram(string expression, StringBuilder program)
        {
            program.Append(expression);
        }
    }
}