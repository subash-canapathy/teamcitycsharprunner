namespace CSharpCompiler.Messages
{
    internal abstract class TeamCityServiceMessageSimple : TeamCityServiceMessage
    {
        private readonly object value;

        protected TeamCityServiceMessageSimple(string name, object value) : base(name)
        {
            this.value = value;
        }

        protected override string FormattedAndEscapedValue
        {
            get { return " '" + Escape(value) + "'"; }
        }
    }
}