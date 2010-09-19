using System.IO;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler.Runtime.Dumping
{
    public class ArtifactObjectDumper : IObjectDumper
    {
        private readonly HtmlObjectVisitorFactory factory;
        private int currentSequence;

        public ArtifactObjectDumper(HtmlObjectVisitorFactory factory)
        {
            this.factory = factory;
        }

        public void Dump(object value, int maximumDepth)
        {
            var tempFileName = Path.GetTempPath();

            using (var visitor = factory.Create(tempFileName, maximumDepth))
                new VisitableObject(value).AcceptVisitor(visitor);

            tempFileName.Publish(currentSequence++.ToString());
        }
    }
}