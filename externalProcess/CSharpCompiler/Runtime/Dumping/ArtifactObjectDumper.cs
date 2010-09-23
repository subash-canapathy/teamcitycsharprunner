using System.IO;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler.Runtime.Dumping
{
    public class ArtifactObjectDumper : IObjectDumper
    {
        private readonly IFileOutputObjectVisitorFactory factory;
        private readonly IServiceMessages serviceMessages;
        private int currentSequence;
        public static readonly IObjectDumper Default;

        static ArtifactObjectDumper()
        {
            Default = new ArtifactObjectDumper(new HtmlObjectVisitorFactory(), ServiceMessages.Default);
        }

        private ArtifactObjectDumper(IFileOutputObjectVisitorFactory factory, IServiceMessages serviceMessages)
        {
            this.factory = factory;
            this.serviceMessages = serviceMessages;
        }

        public void Dump(object value, int maximumDepth)
        {
            var tempFileName = Path.Combine(Path.GetTempPath(), currentSequence++ + ".html");

            using (var visitor = factory.Create(tempFileName, maximumDepth))
                new VisitableObject(value).AcceptVisitor(visitor);

            serviceMessages.Publish(tempFileName);
        }
    }
}