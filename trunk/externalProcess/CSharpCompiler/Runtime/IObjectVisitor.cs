namespace CSharpCompiler.Runtime
{
    public interface IObjectVisitor
    {
        void Visit(object value);
        int MaximumDepth { set; get; }
    }
}