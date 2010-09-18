namespace CSharpCompiler.Runtime
{
    public static class DumpExtensions
    {
        public static IObjectVisitor Visitor;

        static DumpExtensions()
        {
            Visitor = new HtmlReportingVisitor();
        }

        public static T Dump<T>(this T value)
        {
            return Dump(value, Visitor.MaximumDepth);
        }

        public static T Dump<T>(this T value, int maximumDepth)
        {
            Visitor.MaximumDepth = maximumDepth;
            new VisitableObject(value).AcceptVisitor(Visitor);
            return value;
        }
    }
}