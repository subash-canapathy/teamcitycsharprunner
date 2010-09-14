using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace CsharpCompiler
{
    public abstract class Compiler
    {
        protected const string GeneratedFileName = "TempFile.cs";

        protected const string MainTemplate =
            @"class Program 
{{
    static void Main() 
    {{
#line 1 ""{1}""
{0};
    }}
}}";

        public abstract bool CanCompile(string expression);
        public abstract CompilerResults Compile(string expression);

        private readonly string[] defaultNamespaces = new[]
                                                          {
                                                              "System", "System.IO", "System.Text",
                                                              "System.Text.RegularExpressions", "System.Diagnostics",
                                                              "System.Threading", "System.Reflection",
                                                              "System.Collections", "System.Collections.Generic",
                                                              "System.Linq", "System.Linq.Expressions",
                                                              "System.Xml", "System.Xml.Linq", "System.Xml.XPath"
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
    }
}