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
            VisitObject(value);
        }

        private void VisitObject(object value)
        {
            if(value == null)
            {
                VisitNull();
                return;
            }
            if (value is IEnumerable && !(value is string))
            {
                VisitEnumerable(value as IEnumerable);
            }
            else if (IsPrimitive(value))
                VisitPrimitiveType(value);
            else
            {
                using (Nest)
                    VisitType(value);
            }
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
            {
                if(memberInfo.DeclaringType == type)
                {
                    values.Add(GetMemberValue(memberInfo, holder));
                }
                else
                {
                    values.Add("");
                }
            }

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
            if (currentNesting > MaximumDepth)
                return;

            var type = value.GetType();

            VisitTypeHeader(type);
            VisitTypeSummary(type);

            foreach (var property in GetProperties(type))
                VisitMember(property, value);

            foreach (var field in GetFields(type))
                VisitMember(field, value);

            VisitTypeFooter();
        }

        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        protected abstract void VisitTypeFooter();

        protected abstract void VisitTypeSummary(Type type);

        protected abstract void VisitTypeHeader(Type type);

        protected virtual void VisitTypeMember(MemberInfo member, object value)
        {
            VisitTypeMemberName(member);
            VisitTypeMemberValue(value);
        }

        protected virtual void VisitTypeMemberValue(object value)
        {
            VisitObject(value);
        }

        protected abstract void VisitTypeMemberName(MemberInfo member);

        private void VisitMember(MemberInfo property, object holder)
        {
            VisitTypeMember(property, GetMemberValue(property, holder));
        }

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
                if(!IsPrimitive(o))
                    foreach (var member in GetMembers(o.GetType()))
                        members.Add(member);
            }

            return members;
        }

        private static IEnumerable<MemberInfo> GetMembers(Type type)
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