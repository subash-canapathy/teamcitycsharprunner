namespace CSharpCompiler.Runtime.Messages
{
	internal class ProgressStartMessage : TeamCityServiceMessageSimple
	{
		public ProgressStartMessage(object value)
			: base("progressStart", value)
		{
		}
	}
}