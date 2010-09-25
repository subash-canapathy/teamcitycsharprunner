using System;

namespace CSharpCompiler.Runtime.Messages
{
    public static class TeamCityServiceMessagesExtensions
    {
        static TeamCityServiceMessagesExtensions()
        {
            serviceMessages = ServiceMessages.Default;
        }

        public static IServiceMessages serviceMessages { get; set; }

        public static T LogMessage<T>(this T message)
        {
            serviceMessages.LogMessage(message);
            return message;
        }

        public static T LogWarning<T>(this T message)
        {
            serviceMessages.LogWarning(message);
            return message;
        }

        public static T LogFailure<T>(this T message)
        {
            serviceMessages.LogFailure(message);
            return message;
        }

        public static T LogError<T>(this T message)
        {
            serviceMessages.LogError(message);
            return message;
        }

        public static T LogError<T>(this T message, string errorDetails)
        {
            serviceMessages.LogError(message, errorDetails);
            return message;
        }

        public static T Failure<T>(this T message, string format)
        {
            serviceMessages.Failure(message, format);
            return message;
        }

        public static T Failure<T>(this T message)
        {
            serviceMessages.Failure(message);
            return message;
        }

        public static T Success<T>(this T message, string format)
        {
            serviceMessages.Success(message, format);
            return message;
        }

        public static T Success<T>(this T message)
        {
            serviceMessages.Success(message);
            return message;
        }

        public static string Publish(this string artifact, params string[] targets)
        {
            serviceMessages.Publish(artifact, targets);
            return artifact;
        }

        public static T Progress<T>(this T message)
        {
            serviceMessages.Progress(message);
            return message;
        }

        public static IDisposable ProgressBlock<T>(this T message)
        {
            return serviceMessages.ProgressBlock(message);
        }

        public static T BuildNumber<T>(this T buildNumber)
        {
            serviceMessages.BuildNumber(buildNumber);
            return buildNumber;
        }

        public static int Statistic(this int value, object key)
        {
            serviceMessages.Statistic(value, key);
            return value;
        }
    }
}