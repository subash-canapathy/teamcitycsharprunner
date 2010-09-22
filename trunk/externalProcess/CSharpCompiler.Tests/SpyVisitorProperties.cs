using CSharpCompiler.Runtime;
using CSharpCompiler.Runtime.Dumping;

namespace CSharpCompiler.Tests
{
    public class SpyVisitorProperties : DefaultObjectVisitor
    {
        public SpyVisitorProperties() : base(int.MaxValue)
        {
        }

        protected const string Primitive = "Primitive";
        protected const string EnumerableHeader = "EnumerableHeader";
        protected const string EnumerableFooter = "EnumerableFooter";
        protected const string ObjectHeader = "ObjectHeader";
        protected const string ObjectSummary = "ObjectSummary";
        internal const string MemberName = "MemberName";
        protected const string ObjectFooter = "ObjectFooter";
        protected const string TypeInEnumerableHeader = "TypeInEnumerableHeader";
        protected const string TypeInEnumerableFooter = "TypeInEnumerableFooter";
    }
}