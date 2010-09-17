using System;
using System.Collections;
using System.Text;
using CSharpCompiler.Messages;
using NUnit.Framework;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class Dump
    {
        private SimpleVisitor visitor;

        [SetUp]
        public void Setup()
        {
            visitor = new SimpleVisitor();
            DumpExtensions.OutputVisitor = visitor;
        }

        [TestCase(1, "1")]
        [TestCase(1.1, "1.1")]
        [TestCase("ciao", "ciao")]
        [SetCulture("en-US")]
        public void Simple_types(object value, string expectedOutcome)
        {
            value.Dump();

            AssertOutput(expectedOutcome);
        }
        
        [Test]
        public void Enumerable()
        {
            var oneTwoThree = new[] {1, 2, 3};
            oneTwoThree.Dump();

            AssertOutput(string.Join(Environment.NewLine, oneTwoThree));
        }

        private void AssertOutput(string expectedOutput)
        {
            Assert.AreEqual(expectedOutput + Environment.NewLine, visitor.Value);
        }
    }

    public class SimpleVisitor : IOutputVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Value { get { return builder.ToString(); } }

        public void VisitObject(object value)
        {
            builder.AppendLine(value.ToString());
        }

        public void VisitEnumerable(IEnumerable enumerable)
        {
            foreach (var entry in enumerable)
            {
                builder.AppendLine(entry.ToString());
            }
        }
    }
}