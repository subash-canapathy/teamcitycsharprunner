using System;
using System.Collections;
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

        public void Visit(object value)
        {
            VisitObject(value);
        }

        public int MaximumDepth { private get; set; }

        private void VisitObject(object value)
        {
            if (value is IEnumerable && !(value is string))
            {
                VisitEnumerable(value as IEnumerable);
            }
            else if (value.GetType().IsPrimitive || value is string || value is DateTime)
                VisitPrimitiveType(value);
            else
            {
                using (Nest)
                    VisitStructuredObject(value);
            }
        }

        protected abstract void VisitPrimitiveType(object value);

        private void VisitStructuredObject(object value)
        {
            if (currentNesting > MaximumDepth)
                return;

            Type type = value.GetType();
            VisitObjectHeader(type.Name);
            VisitObjectSummary(type.FullName);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (PropertyInfo property in properties)
                VisitProperty(property, value);

            foreach (FieldInfo field in fields)
                VisitField(field, value);

            VisitObjectFooter();
        }

        protected abstract void VisitObjectFooter();

        protected abstract void VisitObjectSummary(string fullTypeName);

        protected abstract void VisitObjectHeader(string typeName);

        private void VisitField(FieldInfo field, object value)
        {
            VisitMemberName(field.Name);
            VisitObject(field.GetValue(value));
        }

        protected abstract void VisitMemberName(string name);

        private void VisitProperty(PropertyInfo property, object value)
        {
            VisitMemberName(property.Name);
            VisitObject(property.GetValue(value, null));
        }

        private void VisitEnumerable(IEnumerable value)
        {
            object[] enumerable = value.Cast<object>().ToArray();

            VisitEnumerableHeader(enumerable.GetType().Name, enumerable.Length);

            foreach (object o in enumerable)
                VisitObject(o);

            VisitEnumerableFooter();
        }

        protected abstract void VisitEnumerableHeader(string typeName, int count);

        protected abstract void VisitEnumerableFooter();
    }
}