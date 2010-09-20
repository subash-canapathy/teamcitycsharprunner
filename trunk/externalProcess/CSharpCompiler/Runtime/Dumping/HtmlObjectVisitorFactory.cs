using System.IO;

namespace CSharpCompiler.Runtime.Dumping
{
    public class HtmlObjectVisitorFactory : IFileOutputObjectVisitorFactory
    {
        public IFileOutputObjectVisitor Create(string outputFilePath, int maximumDepth)
        {
            return new HtmlObjectVisitor(new StreamWriter(outputFilePath), maximumDepth);
        }
    }
}