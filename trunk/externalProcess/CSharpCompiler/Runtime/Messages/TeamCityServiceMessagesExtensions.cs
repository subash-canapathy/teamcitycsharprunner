using System;
using System.IO;

namespace CSharpCompiler.Runtime.Messages
{
    public static class TeamCityServiceMessagesExtensions
    {
        static TeamCityServiceMessagesExtensions()
        {
            OutputWriter = Console.Out;
        }

        public static TextWriter OutputWriter { get; set; }

        public static T Failure<T>(this T value, string format)
        {
            Run(new BuildFailureMessage(value, format));

            return value;
        }

        private static void Run(TeamCityServiceMessage message)
        {
            message.Run(OutputWriter);
        }

        public static T Failure<T>(this T value)
        {
            return Failure(value, "{0}");
        }

        public static T Success<T>(this T value, string format)
        {
            Run(new BuildSuccessMessage(value, format));

            return value;
        }

        public static T Success<T>(this T value)
        {
            return Success(value, "{0}");
        }

        public static string Publish(this string artifact, params string[] targets)
        {
            Run(new PublishArtifactsMessage(artifact, targets));

            return artifact;
        }

        public static T Progress<T>(this T message)
        {
            Run(new ProgressMessage(message));

            return message;
        }

        public static T BuildNumber<T>(this T buildNumber)
        {
            Run(new BuildNumberMessage(buildNumber));

            return buildNumber;
        }

        public static int Statistic(this int value, object key)
        {
            Run(new BuildStatisticMessage(key, value));

            return value;
        }
    }
}