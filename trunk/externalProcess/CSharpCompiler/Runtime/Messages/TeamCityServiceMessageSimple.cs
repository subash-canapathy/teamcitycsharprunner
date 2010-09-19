namespace CSharpCompiler.Runtime.Messages
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
            get { return string.Format(" '{0}'", Escape(value)); }
        }
    }
}