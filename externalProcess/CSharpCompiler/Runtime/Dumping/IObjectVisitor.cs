namespace CSharpCompiler.Runtime.Dumping
{
    public interface IObjectVisitor
    {
        void Visit(object value);
    }
}