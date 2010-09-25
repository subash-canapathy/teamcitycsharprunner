namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogErrorMessage : BuildLogMessageBase
    {
        public BuildLogErrorMessage(object message) : base(message, "ERROR")
        {
            
        }
    }
}