using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpCompiler
{
    public static class TeamCityExtensions
    {
        public static T Failure<T>(this T value, string format)
        {
            Status("FAILURE", value, format);

            return value;
        }

        public static T Failure<T>(this T value)
        {
            return Failure(value, "{0}");
        }

        public static T Success<T>(this T value, string format)
        {
            Status("SUCCESS", value, format);

            return value;
        }

        public static T Success<T>(this T value)
        {
            return Success(value, "{0}");
        }

        private static void Status(string statusType, object value, string format)
        {
            Console.Out.WriteLine(string.Format("##teamcity[buildStatus status='{0}' text='{1}']", statusType, BuildMessage(value, format)));
        }

        private static string BuildMessage(object value, string format)
        {
            return Escape(string.Format(format, value.ToString()));
        }

        private static string Escape(string message)
        {
            return message.Replace("|", "||")
                          .Replace("'", "|'")
                          .Replace("\n", "|n")
                          .Replace("\r", "|r")
                          .Replace("]", "|]");
        }
    }
}
