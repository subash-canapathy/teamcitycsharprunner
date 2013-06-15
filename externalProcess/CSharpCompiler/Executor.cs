using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CSharpCompiler.Runtime.Messages;
using System.Linq;

namespace CSharpCompiler
{
    public class Executor : IExecutor
    {
	    private readonly string artifactsPath;
	    private readonly IServiceMessages serviceMessages;

        public Executor(string artifactsPath, IServiceMessages serviceMessages)
        {
	        this.artifactsPath = artifactsPath;
	        this.serviceMessages = serviceMessages;
        }

	    /// <exception cref="TargetInvocationException">When the supplied assembly's main method throws</exception>
        public void Execute(CompilerResults results, IEnumerable<string> additionalReferences)
        {
        	var entryPoint = results.CompiledAssembly.EntryPoint;

        	using(serviceMessages.ProgressBlock("Executing script"))
				ExecutePrivate(entryPoint, additionalReferences);
        }

    	private void ExecutePrivate(MethodBase entryPoint, IEnumerable<string> additionalReferences)
    	{
    		AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => CurrentDomainOnAssemblyResolve(args.Name, additionalReferences);

			Environment.SetEnvironmentVariable("CSharpRunner_ArtifactsPath", artifactsPath);

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