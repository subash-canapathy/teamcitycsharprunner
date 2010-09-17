using System.Collections;

namespace CSharpCompiler.Messages
{
    public static class DumpExtensions
    {
        public static IOutputVisitor OutputVisitor;

        public static T Dump<T>(this T value)
        {
            new DumpCommand(OutputVisitor).Execute(value);
            return value;
        }
    }

    public interface IOutputVisitor
    {
        void VisitObject(object value);
        void VisitEnumerable(IEnumerable enumerable);
    }

    public class DumpCommand
    {
        private readonly IOutputVisitor outputVisitor;

        public DumpCommand(IOutputVisitor outputVisitor)
        {
            this.outputVisitor = outputVisitor;
        }

        public void Execute(object value)
        {
            if (value is IEnumerable && !(value is string))
                outputVisitor.VisitEnumerable(value as IEnumerable);
            else
                outputVisitor.VisitObject(value);
        }
    }
}