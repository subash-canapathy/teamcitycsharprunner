namespace CSharpCompiler.Runtime.Messages
{
    internal class BuildSuccessMessage : BuildStatusMessage
    {
        public BuildSuccessMessage(object value, string format) : base("SUCCESS", value, format)
        {
        }
    }
}