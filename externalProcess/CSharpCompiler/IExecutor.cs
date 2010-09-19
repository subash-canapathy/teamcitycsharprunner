using System.CodeDom.Compiler;
using System.Reflection;

namespace CsharpCompiler
{
    public interface IExecutor
    {
        /// <exception cref="TargetInvocationException">When the supplied assembly's main method throws</exception>
        void Execute(CompilerResults results);
    }
}