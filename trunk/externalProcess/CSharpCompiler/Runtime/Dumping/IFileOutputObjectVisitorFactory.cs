namespace CSharpCompiler.Runtime.Dumping
{
    public interface IFileOutputObjectVisitorFactory
    {
        IFileOutputObjectVisitor Create(string outputFilePath, int maximumDepth);
    }
}