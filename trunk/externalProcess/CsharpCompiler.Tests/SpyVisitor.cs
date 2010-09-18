using System;
using System.Collections;
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

        protected override void VisitEnumerableHeader(string enumerableTypeName, int elements)
        {
            Visit(EnumerableHeader);
        }

        protected override void VisitEnumerableFooter()
        {
            Visit(EnumerableFooter);
        }

        protected override void VisitObjectHeader(string objectName)
        {
            nestingLevel++;
            ComputeMaxNestingLevel();
            Visit(ObjectHeader);
        }

        private void ComputeMaxNestingLevel()
        {
            MaxNestingLevel = Math.Max(nestingLevel, MaxNestingLevel);
        }

        protected override void VisitObjectSummary(string summary)
        {
            Visit(ObjectSummary);
        }

        protected override void VisitMemberName(string name)
        {
            Visit(MemberName);
        }

        protected override void VisitObjectFooter()
        {
            Visit(ObjectFooter);
            nestingLevel--;
        }
    }
}