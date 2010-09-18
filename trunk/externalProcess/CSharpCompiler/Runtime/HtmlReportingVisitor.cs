using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.UI;

namespace CSharpCompiler.Runtime
{
    public class HtmlReportingVisitor : DefaultObjectVisitor
    {
        private readonly HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());

        public string Output
        {
            get { return ((StringWriter) writer.InnerWriter).GetStringBuilder().ToString(); }
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

        protected override void VisitTypeFooter()
        {
        }

        protected override void VisitType(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            base.VisitType(value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeHeader(Type type)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(type);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitTypeSummary(Type type)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
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
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(name);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerable(IEnumerable value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            base.VisitEnumerable(value);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableHeader(Type enumerableType, int count)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(enumerableType);
            writer.Write(" (");
            writer.Write(count);
            writer.Write(" items)");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableEntry(object entry)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitEnumerableEntry(entry);
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableRow(IEnumerable values)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitTypeInEnumerableRow(values);
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableValue(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitTypeInEnumerableValue(value);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableFooter()
        {
        }

        protected override void VisitTypeInEnumerableHeader(IEnumerable<MemberInfo> members)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitTypeInEnumerableHeader(members);
            writer.RenderEndTag();
        }

        protected override void VisitTypeInEnumerableMember(MemberInfo member)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            VisitPrimitiveType(member.Name);
            writer.RenderEndTag();
        }
    }
}