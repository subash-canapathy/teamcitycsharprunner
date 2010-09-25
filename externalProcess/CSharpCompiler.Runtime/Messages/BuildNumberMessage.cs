namespace CSharpCompiler.Runtime.Messages
{
    internal class BuildNumberMessage : TeamCityServiceMessageSimple
    {
        public BuildNumberMessage(object buildNumber) : base("buildNumber", buildNumber)
        {
        }
    }
}