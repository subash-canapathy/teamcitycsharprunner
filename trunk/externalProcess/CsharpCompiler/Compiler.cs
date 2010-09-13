using System;
using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CSharp;
using System.Linq;

namespace CsharpCompiler
{
    public class Compiler
    {
        private readonly string[] defaultNamespaces = new[]
                                                          {
                                                              "System", "System.IO", "System.Text",
                                                              "System.Text.RegularExpressions", "System.Diagnostics",
                                                              "System.Threading", "System.Reflection",
                                                              "System.Collections", "System.Collections.Generic",
                                                              "System.Linq", "System.Linq.Expressions", 
                                                              "System.Xml", "System.Xml.Linq", "System.Xml.XPath"
                                                          };

        private readonly string[] defaultAssemblyReferences = new[]
                                                         {
                                                             "System.dll", "Microsoft.CSharp.dll", "System.Core.dll",
                                                             "System.Xml.dll", "System.Xml.Linq.dll"
                                                         };


        private const string MainTemplate =
            @"class Program {{
    static void Main() {{
        {0};
    }}
}}";

        public CompilerResults Compile(string expression)
        {
            var program = new StringBuilder()
                .Append(string.Join(Environment.NewLine, defaultNamespaces.Select(n => "using " + n + ";")))
                .AppendLine()
                .AppendFormat(MainTemplate, expression);

            return new CSharpCodeProvider().CompileAssemblyFromSource(
                new CompilerParameters(defaultAssemblyReferences){GenerateExecutable = true}, program.ToString());
        }
    }
}