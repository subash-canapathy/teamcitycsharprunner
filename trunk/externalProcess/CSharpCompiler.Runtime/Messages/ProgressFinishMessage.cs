namespace CSharpCompiler.Runtime.Messages
{
	internal class ProgressFinishMessage : TeamCityServiceMessageSimple
	{
		public ProgressFinishMessage(object value)
			: base("progressFinish", value)
		{
		}
	}
}