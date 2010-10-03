using System;
using System.Reflection;

namespace CSharpCompiler.Runtime.Dumping
{
    public abstract class DefaultObjectVisitor : AbstractObjectVisitor
    {
        protected DefaultObjectVisitor(int maximumDepth) : base(maximumDepth)
        {
        }

    	protected override void VisitCyclicReferenceFound()
    	{
    	}

    	protected override void VisitNull()
        {
        }

    	protected override void VisitCyclicReferenceTypeHeader(Type type)
    	{
    	}

    	protected override void VisitTypeInEnumerableMember(MemberInfo member)
        {
        }

        protected override void VisitPrimitiveType(object value)
        {
        }

    	protected override void VisitExpandedTypeHeader(Type type)
    	{
    	}

    	protected override void VisitCollapsedTypeHeader(Type type)
    	{
    	}

    	protected override void VisitNestingLimitReached()
    	{
    	}

    	protected override void VisitTypeFooter()
        {
        }

        protected override void VisitTypeSummary(object type)
        {
        }

        protected override void VisitStaticTypeHeader(Type type)
        {
        }

    	protected override void VisitHiddenTypeMember()
    	{
    	}

    	protected override void VisitVisibleTypeMember()
    	{
    	}

    	protected override void VisitTypeMemberName(MemberInfo member, Type memberType)
        {
        }

        protected override void VisitEnumerableHeader(Type enumerableType, int count, int numberOfMembers)
        {
        }

        protected override void VisitEnumerableFooter()
        {
        }
    }
}