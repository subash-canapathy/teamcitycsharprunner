using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI;

namespace CSharpCompiler.Runtime.Dumping
{
    public class HtmlObjectVisitor : DefaultObjectVisitor, IFileOutputObjectVisitor
    {
        private readonly HtmlTextWriter writer;

        public HtmlObjectVisitor(TextWriter inner, int maximumDepth) : base(maximumDepth)
        {
            writer = new HtmlTextWriter(inner);
        }

        public override void Visit(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Visit(value);
            writer.RenderEndTag();
        }

        protected override void VisitPrimitiveType(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(value);
            writer.RenderEndTag();
        }

        protected override void VisitType(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            base.VisitType(value);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerable(IEnumerable value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            base.VisitEnumerable(value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeHeader(Type type)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "typeheader");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(type);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableHeader(Type enumerableType, int count, int numberOfMembers)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "typeheader");
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, numberOfMembers.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(enumerableType);
            writer.Write(" (");
            writer.Write(count);
            writer.Write(" item" + AddPluralSuffix(count) + ")");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitTypeSummary(Type type)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "summary");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(type);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitTypeMember(MemberInfo member, object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitTypeMember(member, value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeMemberValue(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitTypeMemberValue(value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeMemberName(string name)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "member");
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(name);
            writer.RenderEndTag();
        }

        private static string AddPluralSuffix(int count)
        {
            return count == 1 ? "" : "s";
        }

        protected override void VisitPrimitiveTypeInEnumerable(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitPrimitiveTypeInEnumerable(value);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableElement(object element)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitEnumerableElement(element);
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableElementValue(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitTypeInEnumerableElementValue(value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableMembers(IEnumerable<MemberInfo> members)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitTypeInEnumerableMembers(members);
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableMember(MemberInfo member)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(member.Name);
            writer.RenderEndTag();
        }

        public void Dispose()
        {
            writer.Flush();
            writer.Dispose();
        }
    }
}