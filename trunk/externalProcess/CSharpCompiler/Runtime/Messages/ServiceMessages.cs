using System;
using System.IO;

namespace CSharpCompiler.Runtime.Messages
{
    public class ServiceMessages : IServiceMessages
    {
        private TextWriter Output { get; set; }

        public static readonly IServiceMessages Default;

        static ServiceMessages()
        {
            Default = new ServiceMessages(Console.Out);
        }

        public ServiceMessages(TextWriter output)
        {
            Output = output;
        }

        public void LogMessage<T>(T message)
        {
            Run(new BuildLogNormalMessage(message));
        }

        public void LogWarning<T>(T message)
        {
            Run(new BuildLogWarningMessage(message));
        }

        public void LogFailure<T>(T message)
        {
            Run(new BuildLogFailureMessage(message));
        }

        public void LogError<T>(T message)
        {
            Run(new BuildLogErrorMessage(message));
        }

        public void LogError<T>(T message, string errorDetails)
        {
            Run(new BuildLogErrorMessageWithDetails(message, errorDetails));
        }

        public void Failure<T>(T value, string format)
        {
            Run(new BuildFailureMessage(value, format));
        }

        private void Run(TeamCityServiceMessage message)
        {
            message.Run(Output);
        }

        public void Failure<T>(T value)
        {
            Failure(value, "{0}");
        }

        public void Success<T>(T value, string format)
        {
            Run(new BuildSuccessMessage(value, format));
        }

        public void Success<T>(T value)
        {
            Success(value, "{0}");
        }

        public void Publish(string artifact, params string[] targets)
        {
            Run(new PublishArtifactsMessage(artifact, targets));
        }

        public void Progress<T>(T message)
        {
            Run(new ProgressMessage(message));
        }

        public IDisposable ProgressBlock<T>(T message)
        {
            return new DisposableAction(() => Run(new ProgressStartMessage(message)),
                                        () => Run(new ProgressFinishMessage(message)));
        }

        public void BuildNumber<T>(T buildNumber)
        {
            Run(new BuildNumberMessage(buildNumber));
        }

        public void Statistic(int value, object key)
        {
            Run(new BuildStatisticMessage(key, value));
        }
    }
}