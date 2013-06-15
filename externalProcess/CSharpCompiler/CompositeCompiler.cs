using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler
{
    public class CompositeCompiler : ICompiler
    {
        public CompositeCompiler(IServiceMessages serviceMessages)
        {
            innerCompilers = new ICompiler[]
                             {
                                 new ExpressionCompiler(serviceMessages),
                                 new StatementCompiler(serviceMessages),
                                 new ProgramCompiler(serviceMessages)
                             };
        }

        private readonly IEnumerable<ICompiler> innerCompilers;

        public bool CanCompile(string expression)
        {
            return innerCompilers.Any(c => c.CanCompile(expression));
        }

        public IEnumerable<string> AdditionalNamespaces
        {
            set
            {
                foreach (var innerCompiler in innerCompilers)
                    innerCompiler.AdditionalNamespaces = value;
            }
        }

        public IEnumerable<string> AdditionalReferences
        {
            set
            {
                foreach (var innerCompiler in innerCompilers)
                    innerCompiler.AdditionalReferences = value;
            }
        }

        /// <exception cref="CannotCompileException"></exception>
        public CompilerResults Compile(string expression)
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