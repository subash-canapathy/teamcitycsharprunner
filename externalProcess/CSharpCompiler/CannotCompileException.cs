using System;

namespace CsharpCompiler
{
    public class CannotCompileException : Exception
    {
        public CannotCompileException(string expression) : base("Cannot compile expression " + expression)
        {
        }
    }
}