﻿using System.IO;

namespace CSharpCompiler.Runtime.Messages
{
    public abstract class TeamCityServiceMessage
    {
        protected TeamCityServiceMessage(string name)
        {
            Name = name;
        }

        private string Name { get; set; }

        public void Run(TextWriter outputWriter)
        {
            outputWriter.WriteLine("##teamcity[{0}{1}]", Name, FormattedAndEscapedValue);
        }

        protected abstract string FormattedAndEscapedValue { get; }

        protected static string Escape(object message)
        {
            return message.ToString()
                .Replace("|", "||")
                .Replace("'", "|'")
                .Replace("\n", "|n")
                .Replace("\r", "|r")
                .Replace("]", "|]");
        }
    }
}