using System;
using System.IO;
using CSharpCompiler.Runtime.Dumping;
using NUnit.Framework;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class HtmlObjectVisitorTest
    {
        [Test]
        public void Primitive()
        {
            Render(1);
        }

        [Test]
        public void Object_simple()
        {
            Render(new
                       {
                           Prop1 = 1,
                           Prop2 = "ciao",
                           Prop3 = DateTime.Now
                       });
        }

        [Test]
        public void Enumerable_ints()
        {
            Render(new[] { 1, 2 });
        }

        [Test]
        public void Enumerable_strings()
        {
            Render(new[] { "a", "b" });
        }

        [Test]
        public void Enumerable_structured()
        {
            Render(new[]
                       {
                           new { Prop1 = 1, Prop2 = "a" },
                           new { Prop1 = 2, Prop2 = "b" }
                       });
        }

        [Test]
        public void Object_nested()
        {
            Render(new
                       {
                           Prop1 = new {Prop11 = 1, Prop12 = 2},
                           Prop2 = new {Prop21 = "ciao", Prop22 = "hello", Prop23 = DateTime.Now}
                       });
        }

        [Test]
        public void Object_complex()
        {
            var obj = new
                          {
                              Prop1 = new[] { new { Prop11 = "ciao"} },
                              Prop2 = 1
                          };

            Render(obj);
        }

        [Test]
        public void Hetereogeneous_object()
        {
            Render(new object[]
                       {
                           1, "ciao", new {Prop1 = "a", Prop2 = 234}, "pollo", new {Prop2 = new[] {1, 2, 3}},
                           new {Prop3 = DateTime.Now}
                       });
        }

        private static void Render(object obj)
        {
            var stringWriter = new StringWriter();
            var htmlReportingVisitor = new HtmlObjectVisitor(stringWriter, 5);
            htmlReportingVisitor.Visit(obj);

            Console.WriteLine(stringWriter.ToString());
        }
    }
}