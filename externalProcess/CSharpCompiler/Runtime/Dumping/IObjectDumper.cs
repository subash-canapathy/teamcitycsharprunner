namespace CSharpCompiler.Runtime.Dumping
{
    public interface IObjectDumper
    {
        void Dump(object value, int maximumDepth);
    }
}