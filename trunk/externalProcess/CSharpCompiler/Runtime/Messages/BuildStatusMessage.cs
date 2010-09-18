using System;
using System.Collections.Generic;

namespace CSharpCompiler.Runtime.Messages
{
    public class BuildStatusMessage : TeamCityServiceMessageWithAttributes
    {
        private readonly string status;
        private readonly object value;
        private readonly string format;

        protected BuildStatusMessage(string status, object value, string format) : base("buildStatus")
        {
            this.status = status;
            this.value = value;
            this.format = format;
        }

        protected override IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
                yield return new KeyValuePair<string, object>("status", status);   
                yield return new KeyValuePair<string, object>("text", String.Format(format, value));   
            }
        }
    }
}