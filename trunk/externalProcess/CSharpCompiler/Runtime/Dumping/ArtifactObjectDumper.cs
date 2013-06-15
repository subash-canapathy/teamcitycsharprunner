using System;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler.Runtime.Dumping
{
    public class ArtifactObjectDumper : IObjectDumper
    {
        private readonly IFileOutputObjectVisitorFactory factory;
        private readonly IServiceMessages serviceMessages;
	    private readonly string artifactsPath;
	    private int currentSequence;

        public static readonly IObjectDumper Default;

        static ArtifactObjectDumper()
        {
            Default = new ArtifactObjectDumper(new HtmlObjectVisitorFactory(), ServiceMessages.Default, Environment.GetEnvironmentVariable("CSharpRunner_ArtifactsPath"));
        }

        private ArtifactObjectDumper(IFileOutputObjectVisitorFactory factory, IServiceMessages serviceMessages, string artifactsPath)
        {
            this.factory = factory;
            this.serviceMessages = serviceMessages;
	        this.artifactsPath = artifactsPath;
        }

        public void Dump(object value, int maximumDepth)
        {
	        var tempFile = PathUtilities.MakeTempPath(currentSequence++ + ".html");

            using (var visitor = factory.Create(tempFile, maximumDepth))
                new VisitableObject(value).AcceptVisitor(visitor);

            serviceMessages.Publish(tempFile, artifactsPath);
        }
    }
}