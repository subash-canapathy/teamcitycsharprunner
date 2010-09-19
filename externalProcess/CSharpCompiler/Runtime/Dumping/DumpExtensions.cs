namespace CSharpCompiler.Runtime.Dumping
{
    public static class DumpExtensions
    {
        private static readonly IObjectDumper Dumper;

        static DumpExtensions()
        {
            Dumper = new ArtifactObjectDumper(new HtmlObjectVisitorFactory());
        }

        public static T Dump<T>(this T value)
        {
            return Dump(value, 5);
        }

        public static T Dump<T>(this T value, int maximumDepth)
        {
            Dumper.Dump(value, maximumDepth);
            
            return value;
        }
    }
}