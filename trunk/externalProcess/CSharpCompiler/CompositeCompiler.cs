using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace CsharpCompiler
{
    public class CompositeCompiler : Compiler
    {
        private readonly IEnumerable<Compiler> innerCompilers = new Compiler[]
                                                                    {
                                                                        new ExpressionCompiler(),
                                                                        new StatementCompiler(),
                                                                        new ProgramCompiler()
                                                                    };

        public override bool CanCompile(string expression)
        {
            return innerCompilers.Any(c => c.CanCompile(expression));
        }

        public override IEnumerable<string> AdditionalNamespaces
        {
            set
            {
                foreach (var innerCompiler in innerCompilers)
                    innerCompiler.AdditionalNamespaces = value;
            }
        }

        public override IEnumerable<string> AdditionalReferences
        {
            set
            {
                foreach (var innerCompiler in innerCompilers)
                    innerCompiler.AdditionalReferences = value;
            }
        }

        /// <exception cref="CannotCompileException"></exception>
        public override CompilerResults Compile(string expression)
        {
            foreach (var innerCompiler in innerCompilers)
            {
                if (innerCompiler.CanCompile(expression))
                    return innerCompiler.Compile(expression);
            }

            throw new CannotCompileException(expression);
        }
    }
}