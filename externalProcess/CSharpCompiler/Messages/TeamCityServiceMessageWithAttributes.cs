using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpCompiler.Messages
{
    public abstract class TeamCityServiceMessageWithAttributes : TeamCityServiceMessage
    {
        protected TeamCityServiceMessageWithAttributes(string name) : base(name)
        {
        }

        protected override string FormattedAndEscapedValue
        {
            get
            {
                return Properties.Aggregate(new StringBuilder(),
                                            (builder, pair) =>
                                            builder.AppendFormat(" {0}='{1}'", pair.Key, Escape(pair.Value)))
                    .ToString();
            }
        }

        protected abstract IEnumerable<KeyValuePair<string, object>> Properties { get; }
    }
}