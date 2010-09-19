using System.Collections.Generic;

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
                foreach (var property in base.Properties)
                {
                    yield return property;
                }

                yield return new KeyValuePair<string, object>("errorDetails", errorDetails);
            }
        }
    }
}