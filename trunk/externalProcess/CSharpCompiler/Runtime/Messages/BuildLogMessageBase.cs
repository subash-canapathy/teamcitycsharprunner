using System.Collections.Generic;

namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogMessageBase : TeamCityServiceMessageWithAttributes
    {
        private readonly object message;
        private readonly string status;

        protected BuildLogMessageBase(object message, string status) : base("message")
        {
            this.message = message;
            this.status = status;
        }

        protected override IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
                yield return new KeyValuePair<string, object>("text", message);
                yield return new KeyValuePair<string, object>("status", status);
            }
        }
    }
}