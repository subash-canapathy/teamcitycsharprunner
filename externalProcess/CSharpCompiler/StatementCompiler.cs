﻿using System.Text;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler
{
    internal class StatementCompiler : AbstractCompiler
    {
        public StatementCompiler(IServiceMessages serviceMessages) : base(serviceMessages)
        {
        }

        public override bool CanCompile(string expression)
        {
            return !ContainsClassDefinition(expression) &&
                    ContainsStatement(expression);
        }

        private bool ContainsStatement(string expression)
        {
            return ContainsSemicolon(expression);
        }
        
        protected override void CreateProgram(string expression, StringBuilder program)
        {
            program.AppendFormat(MainTemplate, expression);
        }
    }
}