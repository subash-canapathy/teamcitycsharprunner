using System;
using System.IO;
using CSharpCompiler.Runtime;
using CSharpCompiler.Runtime.Dumping;
using NUnit.Framework;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class HtmlObjectVisitorTest
    {
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
        public void Object_complex()
        {
            var obj = new
                          {
                              Prop1 = new[]
                                          {
                                              new {Prop1 = "ciao"}
                                          },
                              Prop2 = 1
                          };

            Render(obj);
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