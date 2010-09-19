namespace CSharpCompiler.Runtime.Dumping
{
    public interface IFileOutputObjectVisitorFactory
    {
        IFileOutputObjectVisitor Create(string outputPath, int maximumDepth);
    }
}