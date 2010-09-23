using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpCompiler.Tests
{
    public class SpyVisitor : SpyVisitorProperties
    {
        public readonly IList Visits = new ArrayList();
        public int MaximumReachedDepth;
        private int nestingLevel;

        protected override void VisitPrimitiveType(object value)
        {
            Visit(Primitive);
        }

        private void Visit(string value)
        {
            Visits.Add(value);
        }

        protected override void VisitEnumerableHeader(Type enumerableEnumerableType, int elements, int numberOfMembers)
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

        protected override void VisitNull()
        {
        }

        protected override void VisitTypeInEnumerableMembers(IEnumerable<MemberInfo> members)
        {
            Visit(TypeInEnumerableHeader);
        }

        private void ComputeMaxNestingLevel()
        {
            MaximumReachedDepth = Math.Max(nestingLevel, MaximumReachedDepth);
        }

        protected override void VisitTypeSummary(object summary)
        {
            Visit(ObjectSummary);
        }

        protected override void VisitTypeMemberName(MemberInfo member, Type memberType)
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