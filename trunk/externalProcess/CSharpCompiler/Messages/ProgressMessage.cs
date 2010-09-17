namespace CSharpCompiler.Messages
{
    internal class ProgressMessage : TeamCityServiceMessageSimple
    {
        public ProgressMessage(object value) : base("progressMessage", value)
        {
        }
    }
}