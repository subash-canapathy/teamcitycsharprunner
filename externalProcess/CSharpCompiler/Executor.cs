using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CSharpCompiler.Runtime.Messages;
using System.Linq;

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
        public void Execute(CompilerResults results, IEnumerable<string> additionalReferences)
        {
        	var entryPoint = results.CompiledAssembly.EntryPoint;

        	using(serviceMessages.ProgressBlock("Executing script"))
				ExecutePrivate(entryPoint, additionalReferences);
        }

    	private static void ExecutePrivate(MethodBase entryPoint, IEnumerable<string> additionalReferences)
    	{
    		AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => CurrentDomainOnAssemblyResolve(args.Name, additionalReferences);

    		entryPoint.Invoke(entryPoint.DeclaringType, null);
    	}

    	private static Assembly CurrentDomainOnAssemblyResolve(string assemblyFullName, IEnumerable<string> additionalReferences)
    	{
    		try
    		{
    			var assemblyName = new AssemblyName(assemblyFullName).Name;

    			var found = additionalReferences.First(r => Path.GetFileNameWithoutExtension(r).Equals(assemblyName));

    			return Assembly.LoadFrom(found);
    		}
    		catch (Exception)
    		{
    			return null;
    		}
    	}
    }
}