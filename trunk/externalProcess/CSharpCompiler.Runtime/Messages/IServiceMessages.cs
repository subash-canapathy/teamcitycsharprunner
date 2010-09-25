using System;

namespace CSharpCompiler.Runtime.Messages
{
    public interface IServiceMessages
    {
        void LogMessage<T>(T message);
        void LogWarning<T>(T message);
        void LogFailure<T>(T message);
        void LogError<T>(T message);
        void LogError<T>(T message, string errorDetails);
        void Failure<T>(T value, string format);
        void Failure<T>(T value);
        void Success<T>(T value, string format);
        void Success<T>(T value);
        void Publish(string artifact, params string[] targets);
        void Progress<T>(T message);
        IDisposable ProgressBlock<T>(T message);
        void BuildNumber<T>(T buildNumber);
        void Statistic(int value, object key);
    }
}