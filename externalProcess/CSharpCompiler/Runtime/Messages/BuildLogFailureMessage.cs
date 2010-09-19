namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogFailureMessage : BuildLogMessageBase
    {
        public BuildLogFailureMessage(object message) : base(message, "FAILURE")
        {
            
        }
    }
}