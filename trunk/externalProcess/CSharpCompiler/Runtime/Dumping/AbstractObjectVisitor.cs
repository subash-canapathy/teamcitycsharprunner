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

        private static bool IsPrimitive(object value)
        {
            return value.GetType().IsPrimitive || value is string || value is DateTime;
        }

        private void VisitTypeInEnumerable(object value)
        {
            var type = value.GetType();
            var holder = value;

            var values = new ArrayList();

            foreach (var property in GetProperties(type))
                values.Add(property.GetValue(holder, null));

            foreach (var field in GetFields(type))
                values.Add(field.GetValue(holder));

            VisitTypeInEnumerableElement(values);
        }

        protected virtual void VisitEnumerableElement(object element)
        {
            if (element is IEnumerable && !(element is string))
            {
                VisitEnumerable(element as IEnumerable);
            }
            else if (IsPrimitive(element))
                VisitTypeMemberValue(element);
            else
            {
                VisitTypeInEnumerable(element);
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
                VisitProperty(property, value);

            foreach (var field in GetFields(type))
                VisitField(field, value);

            VisitTypeFooter();
        }

        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        protected abstract void VisitTypeFooter();

        protected abstract void VisitTypeSummary(Type type);

        protected abstract void VisitTypeHeader(Type type);

        private void VisitField(FieldInfo field, object holder)
        {
            VisitTypeMember(field, field.GetValue(holder));
        }

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

        private void VisitProperty(PropertyInfo property, object value)
        {
            VisitTypeMember(property, property.GetValue(value, null));
        }

        protected virtual void VisitEnumerable(IEnumerable value)
        {
            var members = GetMembersFromEnumerableElements(value);

            var numberOfMembers = members.Count() == 0 ? 1 : members.Count();

            VisitEnumerableHeader(value.GetType(), value.Cast<object>().Count(), numberOfMembers);

            if(members.Any())
                VisitTypeInEnumerableMembers(members);

            foreach (var element in value)
                VisitEnumerableElement(element);

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
            return GetProperties(type).Concat<MemberInfo>(GetFields(type));
        }

        protected abstract void VisitEnumerableHeader(Type enumerableType, int count, int numberOfMembers);

        protected abstract void VisitEnumerableFooter();
    }
}