using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CsharpCompiler
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
                return -"fuck".Length;

            var program = Encoding.UTF8.GetString(Convert.FromBase64String(args[0]));
            var namespaces = new List<string>();
            var references = new List<string>();

            if(args.Length > 1)
                namespaces.AddRange(args[1].Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries));

            if (args.Length > 2)
                references.AddRange(args[2].Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries));

            var compiler = new CompositeCompiler { AdditionalNamespaces = namespaces, AdditionalReferences = references };

            CompilerResults results;

            try
            {
                results = compiler.Compile(program);
            }
            catch (CannotCompileException e)
            {
                Console.Out.WriteLine(e.Message);
                return -1;
            }

            if(results.Errors.HasErrors)
            {
                foreach (CompilerError error in results.Errors)
                    Console.Error.WriteLine(error.ToString());

                return results.NativeCompilerReturnValue;
            }

            var executor = new ArtifactPublisherExecutorDecorator(new Executor());

            try
            {
                executor.Execute(results);
            }
            catch (TargetInvocationException e)
            {
                Console.Error.WriteLine(e.InnerException);
                return -1;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return -1;
            }

            return 0;
        }
    }
}
