using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler
{
    internal class Program
    {
        private readonly string program;
	    private string artifactsPath;
	    private string reportName;

	    private readonly List<string> namespaces;
	    private readonly List<string> references;

	    private static TextWriter Out { get; set; }
	    private static TextWriter Error { get; set; }
	    private static IServiceMessages serviceMessages { get; set; }

	    private static int Main(string[] args)
        {
            Out = Console.Out;
            Error = Console.Error;
            serviceMessages = ServiceMessages.Default;

            serviceMessages.LogMessage(string.Format("Running on runtime {0}", RuntimeEnvironment.GetSystemVersion()));

            if (args.Length == 0)
                return -"fuck".Length;

            return new Program(args).Run();
        }

	    private Program(IList<string> args)
        {
            program = Encoding.UTF8.GetString(Convert.FromBase64String(args[0]));
		    artifactsPath = args[1].Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
		    reportName = args[2];

		    namespaces = new List<string>();
            references = new List<string>();

            if (args.Count > 3)
                namespaces.AddRange(args[3].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

            if (args.Count > 4)
                references.AddRange(args[4].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

	    private int Run()
        {
            var compiler = new CompositeCompiler(serviceMessages)
                           {
                               AdditionalNamespaces = namespaces,
                               AdditionalReferences = references
                           };

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

            var executor = new ArtifactPublisherExecutorDecorator(artifactsPath, reportName, new Executor(artifactsPath, serviceMessages), serviceMessages);

            try
            {
                executor.Execute(results, references);
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