using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Microsoft.CSharp;
using CSharpCompiler;
using System.Reflection;

namespace CsharpCompiler
{
    public abstract class Compiler : ICompiler
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

        public abstract bool CanCompile(string expression);

        public CompilerResults Compile(string expression)
        {
            var program = new StringBuilder();

            AddUsingStatements(program);

            CreateProgram(expression, program);

            return Compile(program);
        }

        protected abstract void CreateProgram(string expression, StringBuilder program);

        private StringBuilder AddUsingStatements(StringBuilder program)
        {
            return program.Append(string.Join(Environment.NewLine, Namespaces.Select(n => "using " + n + ";")))
                            .AppendLine();
        }

        private CompilerResults Compile(StringBuilder program)
        {
            var options = new CompilerParameters(References.ToArray()) { GenerateExecutable = true };

            return new CSharpCodeProvider().CompileAssemblyFromSource(options, GetSources(program).ToArray());
        }

        private IEnumerable<string> GetSources(StringBuilder program)
        {
            yield return program.ToString();

            foreach (var additionalCode in AdditionalCode)
                yield return additionalCode;
        }

        public IEnumerable<string> AdditionalCode 
        { 
            get 
            {
                yield return Resources.TeamCityExtensions;
            } 
        }

        private readonly string[] defaultNamespaces = new[]
                                                          {
                                                              "System", "System.IO", "System.Text",
                                                              "System.Text.RegularExpressions", "System.Diagnostics",
                                                              "System.Threading", "System.Reflection",
                                                              "System.Collections", "System.Collections.Generic",
                                                              "System.Linq", "System.Linq.Expressions",
                                                              "System.Xml", "System.Xml.Linq", "System.Xml.XPath", typeof(TeamCityExtensions).Namespace
                                                          };

        private readonly string[] defaulReferences = new[]
                                                         {
                                                             "System.dll", "Microsoft.CSharp.dll",
                                                             "System.Core.dll",
                                                             "System.Xml.dll", "System.Xml.Linq.dll"
                                                         };

        private IEnumerable<string> additionalNamespaces = Enumerable.Empty<string>();

        public virtual IEnumerable<string> AdditionalNamespaces
        {
            private get { return additionalNamespaces; }
            set { additionalNamespaces = value; }
        }

        private IEnumerable<string> additionalReferences = Enumerable.Empty<string>();

        public virtual IEnumerable<string> AdditionalReferences
        {
            private get { return additionalReferences; }
            set { additionalReferences = value; }
        }

        protected IEnumerable<string> Namespaces
        {
            get { return defaultNamespaces.Union(AdditionalNamespaces); }
        }

        protected IEnumerable<string> References
        {
            get { return defaulReferences.Union(AdditionalReferences); }
        }

        protected bool ContainsSemicolumn(string expression)
        {
            return expression.Contains(";");
        }

        protected bool ContainsClassDefinition(string expression)
        {
            return expression.Contains("class ");
        }

        protected bool ContainsMainMethod(string expression)
        {
            var mains = new[] { "void main(", "int main(" };
            var lowercaseExpression = expression.ToLowerInvariant();

            return mains.Any(m => lowercaseExpression.Contains(m));
        }
    }
}