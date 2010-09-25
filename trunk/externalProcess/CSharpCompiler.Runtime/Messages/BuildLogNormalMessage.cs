namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogNormalMessage : BuildLogMessageBase
    {
        public BuildLogNormalMessage(object message) : base(message, "NORMAL")
        {
        }
    }
}