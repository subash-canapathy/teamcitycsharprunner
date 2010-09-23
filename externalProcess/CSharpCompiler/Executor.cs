using System.CodeDom.Compiler;
using System.Reflection;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    public class Executor : IExecutor
    {
        private readonly IServiceMessages serviceMessages;

        public Executor(IServiceMessages serviceMessages)
        {
            this.serviceMessages = serviceMessages;
        }

        /// <exception cref="TargetInvocationException">When the supplied assembly's main method throws</exception>
        public void Execute(CompilerResults results)
        {
        	var entryPoint = results.CompiledAssembly.EntryPoint;

        	using(serviceMessages.ProgressBlock("Executing script"))
				ExecutePrivate(entryPoint);
        }

    	private static void ExecutePrivate(MethodBase entryPoint)
    	{
    		entryPoint.Invoke(entryPoint.DeclaringType, null);
    	}
    }
}