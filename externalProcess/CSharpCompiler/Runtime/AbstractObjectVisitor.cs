using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSharpCompiler.Runtime
{
    public abstract class AbstractObjectVisitor : IObjectVisitor
    {
        private int currentNesting;

        private IDisposable Nest
        {
            get { return new DisposableAction(() => ++currentNesting, () => --currentNesting); }
        }

        protected AbstractObjectVisitor()
        {
            MaximumDepth = 5;
        }

        public virtual void Visit(object value)
        {
            VisitObject(value);
        }

        private void VisitObject(object value)
        {
            VisitObject(value, false);
        }

        private void VisitObject(object value, bool inEnumerable)
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
                    if (inEnumerable)
                        VisitTypeInEnumerable(value);
                    else
                        VisitType(value);
            }
        }

        private static bool IsPrimitive(object value)
        {
            return value.GetType().IsPrimitive || value is string || value is DateTime;
        }

        private void VisitTypeInEnumerable(object value)
        {
            Type type = value.GetType();
            var holder = value;

            var values = new ArrayList();

            foreach (PropertyInfo property in GetProperties(type))
                values.Add(property.GetValue(holder, null));

            foreach (FieldInfo field in GetFields(type))
                values.Add(field.GetValue(holder));

            VisitTypeInEnumerableRow(values);

            VisitTypeInEnumerableFooter();
        }

        protected abstract void VisitTypeInEnumerableFooter();

        protected virtual void VisitTypeInEnumerableRow(IEnumerable values)
        {
            foreach (var value in values)
                VisitTypeInEnumerableValue(value);
        }

        protected virtual void VisitTypeInEnumerableValue(object value)
        {
            VisitObject(value);
        }

        protected virtual void VisitTypeInEnumerableHeader(IEnumerable<MemberInfo> members)
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

            Type type = value.GetType();
            VisitTypeHeader(type);
            VisitTypeSummary(type);

            IEnumerable<PropertyInfo> properties = GetProperties(type);
            IEnumerable<FieldInfo> fields = GetFields(type);

            foreach (PropertyInfo property in properties)
                VisitProperty(property, value);

            foreach (FieldInfo field in fields)
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
            VisitTypeMemberName(member.Name);
            VisitTypeMemberValue(value);
        }

        protected virtual void VisitTypeMemberValue(object value)
        {
            VisitObject(value);
        }

        protected abstract void VisitTypeMemberName(string name);

        private void VisitProperty(PropertyInfo property, object value)
        {
            VisitTypeMember(property, property.GetValue(value, null));
        }

        protected virtual void VisitEnumerable(IEnumerable value)
        {
            object[] enumerable = value.Cast<object>().ToArray();

            VisitEnumerableHeader(enumerable.GetType(), enumerable.Length);

            if(enumerable.Any() && !IsPrimitive(enumerable[0]))
                VisitTypeInEnumerableHeader(GetMembers(enumerable[0].GetType()));

            foreach (object entry in enumerable)
                VisitEnumerableEntry(entry);

            VisitEnumerableFooter();
        }

        private static bool HasMembers(Type type)
        {
            return GetMembers(type).Any();
        }

        private static IEnumerable<MemberInfo> GetMembers(Type type)
        {
            return GetProperties(type).Concat<MemberInfo>(GetFields(type));
        }

        protected virtual void VisitEnumerableEntry(object entry)
        {
            VisitObject(entry, true);
        }

        protected abstract void VisitEnumerableHeader(Type enumerableType, int count);

        protected abstract void VisitEnumerableFooter();
    }
}