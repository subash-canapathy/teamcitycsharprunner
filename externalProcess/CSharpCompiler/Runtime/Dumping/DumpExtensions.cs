namespace CSharpCompiler.Runtime.Dumping
{
    public static class DumpExtensions
    {
        private static readonly IObjectDumper Dumper;

        static DumpExtensions()
        {
            Dumper = ArtifactObjectDumper.Default;
        }

        public static T Dump<T>(this T value)
        {
            return Dump(value, 3);
        }

        public static T Dump<T>(this T value, int maximumDepth)
        {
            Dumper.Dump(value, maximumDepth);
            
            return value;
        }
    }
}