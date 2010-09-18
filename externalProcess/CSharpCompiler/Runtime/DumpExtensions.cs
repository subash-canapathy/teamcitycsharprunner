namespace CSharpCompiler.Runtime
{
    public static class DumpExtensions
    {
        public static IObjectVisitor Visitor;

        public static T Dump<T>(this T value)
        {
            return Dump(value, 5);
        }

        public static T Dump<T>(this T value, int maximumDepth)
        {
            Visitor.MaximumDepth = maximumDepth;
            new VisitableObject(value).AcceptVisitor(Visitor);
            return value;
        }
    }
}