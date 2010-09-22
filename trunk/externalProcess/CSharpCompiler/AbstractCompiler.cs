using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CSharpCompiler;
using CSharpCompiler.Runtime.Dumping;
using CSharpCompiler.Runtime.Messages;
using Microsoft.CSharp;

namespace CsharpCompiler
{
    public abstract class AbstractCompiler : ICompiler
    {
        protected const string MainTemplate =
            @"class Program 
{{
    static void Main() 
    {{
#line 1 ""TempFile.cs""
{0}
    }}
}}";

        private readonly string[] defaulReferences = new[]
                                                         {
                                                             "System.dll",
                                                             "System.Core.dll",
                                                             "System.Xml.dll", "System.Xml.Linq.dll",
                                                             "System.Web.dll"
                                                         };

        private readonly string[] defaultNamespaces = new[]
                                                          {
                                                              "System", "System.IO", "System.Text",
                                                              "System.Text.RegularExpressions", "System.Diagnostics",
                                                              "System.Threading", "System.Reflection",
                                                              "System.Collections", "System.Collections.Generic",
                                                              "System.Linq", "System.Linq.Expressions",
                                                              "System.Xml", "System.Xml.Linq", "System.Xml.XPath",
                                                              "System.Web",
                                                              typeof (TeamCityServiceMessagesExtensions).Namespace,
                                                              typeof(DumpExtensions).Namespace
                                                          };

        protected AbstractCompiler()
        {
            AdditionalNamespaces = Enumerable.Empty<string>();
            AdditionalReferences = Enumerable.Empty<string>();
        }

        private static IEnumerable<string> AdditionalCode
        {
            get
            {
                return Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false)
                    .Cast<DictionaryEntry>().Select(d => d.Value.ToString());
            }
        }

        public IEnumerable<string> AdditionalNamespaces { private get; set; }
        public IEnumerable<string> AdditionalReferences { private get; set; }

    	public abstract bool CanCompile(string expression);

        public CompilerResults Compile(string expression)
        {
        	using ("Compiling script".ProgressBlock())
        		return CompilePrivate(expression);
        }

    	private CompilerResults CompilePrivate(string expression)
    	{
    		var program = new StringBuilder();

    		AddUsingStatements(program);

    		CreateProgram(expression, program);

    		return Compile(program);
    	}

    	protected abstract void CreateProgram(string expression, StringBuilder program);

        private void AddUsingStatements(StringBuilder program)
        {
            program.AppendLine(FormatNamespaces()).AppendLine();
        }

    	private string FormatNamespaces()
    	{
    		return string.Join(Environment.NewLine, ComputeNamespaces().Select(n => "using " + n + ";").ToArray());
    	}

    	private IEnumerable<string> ComputeNamespaces()
    	{
    		var dupicates = AdditionalNamespaces.Intersect(defaultNamespaces);

    		foreach (var dupicate in dupicates)
    			string.Format("Importing the namespace {0} is not needed as it is imported by default", dupicate).LogWarning();

    		return defaultNamespaces.Union(AdditionalNamespaces);
    	}

    	private CompilerResults Compile(StringBuilder program)
        {
            var compilerParameters = new CompilerParameters(ComputeReferences())
                                         {
                                             GenerateExecutable = true, 
                                             GenerateInMemory = true,
                                         };

        	var compilerVersion = ChooseCompilerVersion();
        	var options = new Dictionary<string, string> { { "CompilerVersion", compilerVersion } };

        	string.Format("Compiling code for compiler {0}", compilerVersion).LogMessage();

            using (var provider = new CSharpCodeProvider(options))
                return provider.CompileAssemblyFromSource(compilerParameters, GetSources(program).ToArray());
        }

    	private string[] ComputeReferences()
    	{
    		var duplicates = defaulReferences.Intersect(AdditionalReferences);

    		foreach (var duplicate in duplicates)
    			string.Format("Referencing assembly {0} is not needed as it is referenced by default", duplicate).LogWarning();

    		return defaulReferences.Union(AdditionalReferences).ToArray();
    	}

    	private static string ChooseCompilerVersion()
    	{
    		var runtime = RuntimeEnvironment.GetSystemVersion();

    		var runtimeSubstring = runtime.Substring(0, "vX.X".Length);

			return runtimeSubstring.Equals("v2.0", StringComparison.InvariantCultureIgnoreCase) ? "v3.5" : "v4.0";
    	}

    	private static IEnumerable<string> GetSources(StringBuilder program)
        {
            yield return program.ToString();

            foreach (var additionalCode in AdditionalCode)
                yield return additionalCode;
        }

        protected static bool ContainsSemicolumn(string expression)
        {
            return expression.Contains(";");
        }

        protected static bool ContainsClassDefinition(string expression)
        {
            return expression.Contains("class ");
        }

        protected static bool ContainsMainMethod(string expression)
        {
            var mains = new[] {"void main(", "int main("};
            var lowercaseExpression = expression.ToLowerInvariant();

            return mains.Any(lowercaseExpression.Contains);
        }
    }
}