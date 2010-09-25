using System;
using System.Linq;
using System.Text;

namespace CSharpCompiler.Runtime.Messages
{
    internal class PublishArtifactsMessage : TeamCityServiceMessageSimple
    {
        public PublishArtifactsMessage(string artifact, params string[] targets) : base("publishArtifacts", Compute(artifact, targets))
        {
        }

        private static object Compute(string artifact, params string[] targets)
        {
            var artifactSpecification = new StringBuilder(artifact);

            if (targets.Any())
                artifactSpecification.Append(" => ").Append(String.Join(", ", targets));

            return artifactSpecification.ToString();
        }
    }
}