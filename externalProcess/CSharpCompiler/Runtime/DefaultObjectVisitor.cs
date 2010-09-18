namespace CSharpCompiler.Runtime
{
    public class DefaultObjectVisitor : AbstractObjectVisitor
    {
        protected override void VisitPrimitiveType(object value)
        {
        }

        protected override void VisitObjectFooter()
        {
        }

        protected override void VisitObjectSummary(string fullTypeName)
        {
        }

        protected override void VisitObjectHeader(string typeName)
        {
        }

        protected override void VisitMemberName(string name)
        {
        }

        protected override void VisitEnumerableHeader(string typeName, int count)
        {
        }

        protected override void VisitEnumerableFooter()
        {
        }
    }
}