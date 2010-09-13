using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;

namespace CsharpCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = args[0];

            var provider = new CSharpCodeProvider();

            provider.CompileAssemblyFromSource(new CompilerParameters
                                                   {

                                                   }, program);

        }
    }
}
