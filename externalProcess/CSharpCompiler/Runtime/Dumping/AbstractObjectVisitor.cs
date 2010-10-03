using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCompiler.Runtime.Dumping
{
    public abstract class AbstractObjectVisitor : IObjectVisitor
    {
        private int currentNesting;
    	private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;

    	private IDisposable Nest
        {
            get { return new DisposableAction(() => ++currentNesting, () => --currentNesting); }
        }

        protected AbstractObjectVisitor(int maximumDepth)
        {
            MaximumDepth = maximumDepth;
        }

        public virtual void Visit(object value)
        {
            VisitImpl(value);
        }

        private void VisitImpl(object value)
        {
            if(value == null)
            {
                VisitNull();
                return;
            }

            if (IsEnumerable(value))
                VisitEnumerable(value as IEnumerable);
            else if (IsPrimitive(value))
            	VisitPrimitiveType(value);
            else
            	using (Nest)
            	{
            		if (NestingLimitReached)
            			VisitNestingLimitReached();

            		VisitType(value);
            	}
        }

    	private bool NestingLimitReached
    	{
    		get { return currentNesting > MaximumDepth; }
    	}

    	private static bool IsEnumerable(object value)
        {
            return value is IEnumerable && !(value is string);
        }

        protected abstract void VisitNull();

        private static bool IsPrimitive(object value)
        {
            return value.GetType().IsPrimitive || value is string || value is DateTime;
        }

        protected virtual void VisitEnumerableElement(object element, IEnumerable<MemberInfo> members)
        {
            if (element is IEnumerable && !(element is string))
            {
                VisitEnumerableInEnumerable(element as IEnumerable, members.Count());
            }
            else if (IsPrimitive(element))
                VisitPrimitiveTypeInEnumerable(element, members.Count());
            else
            {
                VisitTypeInEnumerable(element, members);
            }
        }

        protected virtual void VisitEnumerableInEnumerable(IEnumerable enumerable, int numberOfMembers)
        {
            VisitEnumerable(enumerable);
        }

        protected virtual void VisitPrimitiveTypeInEnumerable(object element, int numberOfMembers)
        {
            VisitPrimitiveType(element);
        }

        private void VisitTypeInEnumerable(object value, IEnumerable<MemberInfo> members)
        {
            var type = value.GetType();
            var holder = value;

            var values = new ArrayList();

        	foreach (var memberInfo in members)
        		values.Add(memberInfo.DeclaringType == type ? GetMemberValue(memberInfo, holder) : "");

        	VisitTypeInEnumerableElement(values);
        }

        private static object GetMemberValue(MemberInfo memberInfo, object holder)
        {
            if (memberInfo is PropertyInfo)
            {
                return GetReturnValueOrExceptionMessage(() => ((PropertyInfo)memberInfo).GetValue(holder, null));
            }
            return GetReturnValueOrExceptionMessage(() => ((FieldInfo)memberInfo).GetValue(holder));
        }

        private static object GetReturnValueOrExceptionMessage(Func<object> func)
        {
            try
            {
                return func();
            }
            catch(TargetInvocationException e)
            {
                return e.InnerException.Message;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        protected virtual void VisitTypeInEnumerableElement(IEnumerable elementValues)
        {
            foreach (var value in elementValues)
                VisitTypeMemberValue(value);
        }

        protected virtual void VisitTypeInEnumerableMembers(IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
                VisitTypeInEnumerableMember(member);
        }

        protected abstract void VisitTypeInEnumerableMember(MemberInfo member);

        public int MaximumDepth { get; set; }

        protected abstract void VisitPrimitiveType(object value);

        protected virtual void VisitType(object value)
        {
            var type = value.GetType();

			if (NestingLimitReached)
				VisitStaticTypeHeader(type);
			else if (IsCollapsed(type))
				VisitCollapsedTypeHeader(type);
			else
				VisitExpandedTypeHeader(type);

        	VisitTypeSummary(value);

        	if (NestingLimitReached)
        		return;

        	foreach (var property in GetProperties(type))
                VisitTypeMember(property, property.PropertyType, value, IsCollapsed(type) == false);

            foreach (var field in GetFields(type))
				VisitTypeMember(field, field.FieldType, value, IsCollapsed(type) == false);

            VisitTypeFooter();
        }

    	protected abstract void VisitExpandedTypeHeader(Type type);

    	protected abstract void VisitCollapsedTypeHeader(Type type);

    	private static bool IsCollapsed(IReflect type)
    	{
    		return GetMembers(type).Count() > 5;
    	}

    	protected abstract void VisitNestingLimitReached();

    	private static IEnumerable<FieldInfo> GetFields(IReflect type)
        {
            return type.GetFields(Flags);
        }

        private static IEnumerable<PropertyInfo> GetProperties(IReflect type)
        {
            return type.GetProperties(Flags);
        }

        protected abstract void VisitTypeFooter();

        protected abstract void VisitTypeSummary(object type);

        protected abstract void VisitStaticTypeHeader(Type type);

    	private void VisitTypeMember(MemberInfo member, Type memberType, object holder, bool visible)
    	{
			if (visible)
				VisitVisibleTypeMember();
			else
				VisitHiddenTypeMember();

			VisitTypeMember(member, memberType, GetMemberValue(member, holder));
    	}

    	protected abstract void VisitVisibleTypeMember();

    	protected abstract void VisitHiddenTypeMember();

    	protected virtual void VisitTypeMember(MemberInfo member, Type memberType, object value)
        {
            VisitTypeMemberName(member, memberType);
            VisitTypeMemberValue(value);
        }

    	protected virtual void VisitTypeMemberValue(object value)
        {
            VisitImpl(value);
        }

    	protected abstract void VisitTypeMemberName(MemberInfo member, Type memberType);

    	protected virtual void VisitEnumerable(IEnumerable value)
        {
            var members = GetMembersFromEnumerableElements(value);

            var numberOfMembers = members.Count() == 0 ? 1 : members.Count();

            VisitEnumerableHeader(value.GetType(), value.Cast<object>().Count(), numberOfMembers);

            if(members.Any())
                VisitTypeInEnumerableMembers(members);

            foreach (var element in value)
                VisitEnumerableElement(element, members);

            VisitEnumerableFooter();
        }

        private static IEnumerable<MemberInfo> GetMembersFromEnumerableElements(IEnumerable enumerable)
        {
            var members = new HashSet<MemberInfo>();

            foreach (var o in enumerable)
            {
                if(!IsPrimitive(o) && !IsEnumerable(o))
                    foreach (var member in GetMembers(o.GetType()))
                        members.Add(member);
            }

            return members;
        }

        private static IEnumerable<MemberInfo> GetMembers(IReflect type)
        {
            foreach (var property in GetProperties(type))
                yield return property;

            foreach (var field in GetFields(type))
                yield return field;
        }

        protected abstract void VisitEnumerableHeader(Type enumerableType, int count, int numberOfMembers);

        protected abstract void VisitEnumerableFooter();
    }
}