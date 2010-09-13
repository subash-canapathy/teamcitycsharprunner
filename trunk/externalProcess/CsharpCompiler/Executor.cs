using System.CodeDom.Compiler;

namespace CsharpCompiler
{
    public class Executor
    {
        public void Execute(CompilerResults results)
        {
            var entryPoint = results.CompiledAssembly.EntryPoint;

            entryPoint.Invoke(entryPoint.DeclaringType, null);
        }
    }
}