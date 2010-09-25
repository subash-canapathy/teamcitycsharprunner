namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogWarningMessage : BuildLogMessageBase
    {
        public BuildLogWarningMessage(object message) : base(message, "WARNING")
        {
            
        }
    }
}