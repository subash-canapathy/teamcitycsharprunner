using System.Collections.Generic;
using System.Linq;

namespace CSharpCompiler.Runtime.Messages
{
    public class BuildLogErrorMessageWithDetails : BuildLogErrorMessage
    {
        private readonly object errorDetails;

        public BuildLogErrorMessageWithDetails(object message, object errorDetails) : base(message)
        {
            this.errorDetails = errorDetails;
        }

        protected override IEnumerable<KeyValuePair<string, object>> Properties
        {
            get
            {
				return base.Properties.Concat(new[]{ new KeyValuePair<string, object>("errorDetails", errorDetails) });
            }
        }
    }
}