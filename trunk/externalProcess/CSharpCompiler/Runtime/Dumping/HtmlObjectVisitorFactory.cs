using System.IO;

namespace CSharpCompiler.Runtime.Dumping
{
    public class HtmlObjectVisitorFactory : IFileOutputObjectVisitorFactory
    {
        public IFileOutputObjectVisitor Create(string outputPath, int maximumDepth)
        {
            return new HtmlObjectVisitor(new StreamWriter(outputPath), maximumDepth);
        }
    }
}