using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CSharpCompiler.Runtime;

namespace CSharpCompiler.Tests
{
    public class SpyVisitor : DefaultObjectVisitor
    {
        protected const string Primitive = "Primitive";
        protected const string EnumerableHeader = "EnumerableHeader";
        protected const string EnumerableFooter = "EnumerableFooter";
        protected const string ObjectHeader = "ObjectHeader";
        protected const string ObjectSummary = "ObjectSummary";
        internal const string MemberName = "MemberName";
        protected const string ObjectFooter = "ObjectFooter";
        protected const string TypeInEnumerableHeader = "TypeInEnumerableHeader";
        protected const string TypeInEnumerableFooter = "TypeInEnumerableFooter";

        public readonly IList Visits = new ArrayList();
        public int MaxNestingLevel;
        private int nestingLevel;

        protected override void VisitPrimitiveType(object value)
        {
            Visit(Primitive);
        }

        private void Visit(string value)
        {
            Visits.Add(value);
        }

        protected override void VisitTypeInEnumerableFooter()
        {
            Visit(TypeInEnumerableFooter);
        }

        protected override void VisitEnumerableHeader(Type enumerableEnumerableType, int elements)
        {
            Visit(EnumerableHeader);
        }

        protected override void VisitEnumerableFooter()
        {
            Visit(EnumerableFooter);
        }

        protected override void VisitTypeHeader(Type @object)
        {
            nestingLevel++;
            ComputeMaxNestingLevel();
            Visit(ObjectHeader);
        }

        protected override void VisitTypeInEnumerableHeader(IEnumerable<MemberInfo> members)
        {
            Visit(TypeInEnumerableHeader);
        }

        private void ComputeMaxNestingLevel()
        {
            MaxNestingLevel = Math.Max(nestingLevel, MaxNestingLevel);
        }

        protected override void VisitTypeSummary(Type summary)
        {
            Visit(ObjectSummary);
        }

        protected override void VisitTypeMemberName(string name)
        {
            Visit(MemberName);
        }

        protected override void VisitTypeFooter()
        {
            Visit(ObjectFooter);
            nestingLevel--;
        }
    }
}