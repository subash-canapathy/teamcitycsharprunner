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

    	protected override void VisitNestingLimitReached()
    	{
			writer.AddAttribute(HtmlTextWriterAttribute.Title, "Maximum nesting limit reached");
    		writer.AddAttribute(HtmlTextWriterAttribute.Class, "limit");
    	}

    	public override void Visit(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            base.Visit(value);
            writer.RenderEndTag();
        }

        protected override void VisitNull()
        {
			writer.RenderBeginTag(HtmlTextWriterTag.I);
            writer.Write("null");
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

        protected override void VisitStaticTypeHeader(Type type)
        {
            BeforeTypeHeader();
        	writer.Write(FormatTypeNameForHeader(type));
            AfterTypeHeader();
        }

    	protected override void VisitCollapsedTypeHeader(Type type)
    	{
    		VisitToggleableTypeHeader(6, type, "show");
    	}

    	protected override void VisitExpandedTypeHeader(Type type)
    	{
    		VisitToggleableTypeHeader(5, type, "hide");	
    	}

    	private void VisitToggleableTypeHeader(int toggleGliph, Type type, string onClick)
    	{
    		BeforeTypeHeader();
			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("return {0}(this);", onClick));
			writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
    				writer.AddAttribute(HtmlTextWriterAttribute.Class, "typeglyph");
    				writer.RenderBeginTag(HtmlTextWriterTag.Span);
    				writer.Write(toggleGliph);
    				writer.RenderEndTag();
    			writer.Write(FormatTypeNameForHeader(type));
    		writer.RenderEndTag();
    		AfterTypeHeader();
    	}

    	private void BeforeTypeHeader()
    	{
    		writer.RenderBeginTag(HtmlTextWriterTag.Tr);
    		writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
    		writer.AddAttribute(HtmlTextWriterAttribute.Class, "typeheader");
    		writer.RenderBeginTag(HtmlTextWriterTag.Td);
    	}

    	private void AfterTypeHeader()
    	{
    		writer.RenderEndTag();
    		writer.RenderEndTag();
    	}

    	private static string FormatTypeNameForHeader(Type type)
        {
            return IsAnonymous(type) ? "anonymous" : type.Name;
        }

    	private static bool IsAnonymous(Type type)
        {
            return type.Name.StartsWith("<>");
        }

    	protected override void VisitEnumerableHeader(Type enumerableType, int count, int numberOfMembers)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "typeheader");
            
            if(numberOfMembers > 1)
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, numberOfMembers.ToString());
            
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(FormatTypeNameForHeader(enumerableType));
            writer.Write(" (");
            writer.Write(count);
            writer.Write(" item" + AddPluralSuffix(count) + ")");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

    	protected override void VisitTypeSummary(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, 2.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "summary");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(value.ToString());
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

    	protected override void VisitHiddenTypeMember(MemberInfo member, Type memberType, object value)
    	{
    		writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden");
			VisitTypeMember(member, memberType, value);
    	}

    	protected override void VisitTypeMember(MemberInfo member, Type memberType, object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitTypeMember(member, memberType, value);
            writer.RenderEndTag();
        }

    	protected override void VisitTypeMemberValue(object value)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitTypeMemberValue(value);
            writer.RenderEndTag();
        }

        protected override void VisitTypeMemberName(MemberInfo member, Type memberType)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "member");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, memberType.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Th);
            writer.Write(FormatMemberName(member));
            writer.RenderEndTag();
        }

        private static string AddPluralSuffix(int count)
        {
            return count == 1 ? "" : "s";
        }

        protected override void VisitPrimitiveTypeInEnumerable(object element, int numberOfMembers)
        {
            if(numberOfMembers > 1)
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, numberOfMembers.ToString());

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitPrimitiveTypeInEnumerable(element, numberOfMembers);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableInEnumerable(IEnumerable enumerable, int numberOfMembers)
        {
            if(numberOfMembers > 1)
                writer.AddAttribute(HtmlTextWriterAttribute.Colspan, numberOfMembers.ToString());

            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            base.VisitEnumerableInEnumerable(enumerable, numberOfMembers);
            writer.RenderEndTag();
        }

        protected override void VisitEnumerableElement(object element, IEnumerable<MemberInfo> members)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            base.VisitEnumerableElement(element, members);
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
            writer.Write(FormatMemberName(member));
            writer.RenderEndTag();
        }

        private static string FormatMemberName(MemberInfo member)
        {
            return member.Name.StartsWith("<") ? member.Name.Substring(1, member.Name.IndexOf(">") - 1) : member.Name;
        }

        public void Dispose()
        {
            writer.Flush();
            writer.Dispose();
        }
    }
}