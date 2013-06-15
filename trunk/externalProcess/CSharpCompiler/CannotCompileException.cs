using System;

namespace CSharpCompiler
{
    public class CannotCompileException : Exception
    {
        public CannotCompileException(string expression) : base("Cannot compile expression " + expression)
        {
        }
    }
}