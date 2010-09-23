using System.Text;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    internal class ProgramCompiler : AbstractCompiler
    {
        public ProgramCompiler(IServiceMessages serviceMessages) : base(serviceMessages)
        {
        }

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