using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                                                             "System.dll", "Microsoft.CSharp.dll",
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

        private IEnumerable<string> Namespaces
        {
            get { return defaultNamespaces.Union(AdditionalNamespaces); }
        }

        private IEnumerable<string> References
        {
            get { return defaulReferences.Union(AdditionalReferences); }
        }

        public abstract bool CanCompile(string expression);

        public CompilerResults Compile(string expression)
        {
            var program = new StringBuilder();

            AddUsingStatements(program);

            CreateProgram(expression, program);

            return Compile(program);
        }

        protected abstract void CreateProgram(string expression, StringBuilder program);

        private void AddUsingStatements(StringBuilder program)
        {
            program.Append(string.Join(Environment.NewLine, Namespaces.Select(n => "using " + n + ";")))
                .AppendLine();
        }

        private CompilerResults Compile(StringBuilder program)
        {
            var options = new CompilerParameters(References.ToArray()) {GenerateExecutable = true};

            return new CSharpCodeProvider().CompileAssemblyFromSource(options, GetSources(program).ToArray());
        }

        private static IEnumerable<string> GetSources(StringBuilder program)
        {
            yield return program.ToString();

            foreach (string additionalCode in AdditionalCode)
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