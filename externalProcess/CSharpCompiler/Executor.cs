using System.CodeDom.Compiler;
using System.Reflection;

namespace CsharpCompiler
{
    public class Executor : IExecutor
    {
        /// <exception cref="TargetInvocationException">When the supplied assembly's main method throws</exception>
        public void Execute(CompilerResults results)
        {
            var entryPoint = results.CompiledAssembly.EntryPoint;

            entryPoint.Invoke(entryPoint.DeclaringType, null);
        }
    }
}