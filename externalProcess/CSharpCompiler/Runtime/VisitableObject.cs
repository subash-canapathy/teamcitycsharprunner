namespace CSharpCompiler.Runtime
{
    public class VisitableObject
    {
        private readonly object value;

        public VisitableObject(object value)
        {
            this.value = value;
        }

        public void AcceptVisitor(IObjectVisitor visitor)
        {
            visitor.Visit(value);
        }
    }
}