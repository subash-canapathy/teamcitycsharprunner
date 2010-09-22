using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    class Program
    {
    	private readonly string program;
    	private readonly List<string> namespaces;
    	private readonly List<string> references;

    	private TextWriter Out { get; set; }
    	private TextWriter Error { get; set; }

    	private static int Main(string[] args)
    	{
    		string.Format("Running on runtime {0}", RuntimeEnvironment.GetSystemVersion()).LogMessage();

    		if (args.Length == 0)
    			return -"fuck".Length;

    		return new Program(args).Run();
    	}

    	private Program(IList<string> args)
    	{
			program = Encoding.UTF8.GetString(Convert.FromBase64String(args[0]));
			namespaces = new List<string>();
			references = new List<string>();

			if (args.Count > 1)
				namespaces.AddRange(args[1].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

			if (args.Count > 2)
				references.AddRange(args[2].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

			Out = Console.Out;
    		Error = Console.Error;
    	}

    	private int Run()
    	{
			var compiler = new CompositeCompiler { AdditionalNamespaces = namespaces, AdditionalReferences = references };

			CompilerResults results;

			try
			{
				results = compiler.Compile(program);
			}
			catch (CannotCompileException e)
			{
				Out.WriteLine(e.Message);
				return -1;
			}

			if (results.Errors.HasErrors)
			{
				foreach (CompilerError compilerError in results.Errors)
					Error.WriteLine(compilerError.ToString());

				return results.NativeCompilerReturnValue;
			}

			var executor = new ArtifactPublisherExecutorDecorator(new Executor());

			try
			{
				executor.Execute(results);
			}
			catch (TargetInvocationException e)
			{
				Error.WriteLine(e.InnerException);
				return -1;
			}
			catch (Exception e)
			{
				Error.WriteLine(e);
				return -1;
			}

			return 0;
    	}
    }
}
