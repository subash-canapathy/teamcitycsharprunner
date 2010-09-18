using System;
using CSharpCompiler.Runtime;
using NUnit.Framework;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class Dump : SpyVisitor
    {
        private SpyVisitor visitor;

        [SetUp]
        public void Setup()
        {
            visitor = new SpyVisitor();
            DumpExtensions.Visitor = visitor;
        }

        [TestCase(1)]
        [TestCase(1.1)]
        [TestCase("ciao")]
        public void Simple_types(object value)
        {
            value.Dump();

            AssertVisited(Primitive);
        }

        [Test]
        public void DateTime_is_simple()
        {
            DateTime.Now.Dump();

            AssertVisited(Primitive);
        }

        [Test]
        public void SimpleEnumerable()
        {
            var oneTwoThree = new[] { 1, 2, 3 };
            oneTwoThree.Dump();

            AssertVisited(EnumerableHeader, Primitive, Primitive, Primitive, EnumerableFooter);
        }

        [Test]
        public void Plain_object()
        {
            var obj = new {Prop1 = "ciao", Prop2 = "hello"};
            obj.Dump();

            AssertVisited(ObjectHeader, 
                          ObjectSummary, 
                            MemberName, Primitive, 
                            MemberName, Primitive, 
                         ObjectFooter);
        }

        [Test]
        public void Structured_object()
        {
            var obj = new
                          {
                              Prop1 = new
                                          {
                                              Prop1 = "ciao",
                                              Prop2 = DateTime.Now
                                          },
                              Prop2 = 3
                          };

            obj.Dump();

            AssertVisited(ObjectHeader,
                          ObjectSummary,
                            MemberName,
                                ObjectHeader,
                                ObjectSummary,
                                    MemberName, Primitive,
                                    MemberName, Primitive,
                                ObjectFooter,
                            MemberName, Primitive,
                        ObjectFooter);
        }

        [Test]
        public void Complex_object()
        {
            var obj = new
                          {
                              Prop1 = new[]
                                          {
                                              new {Prop1 = "ciao"}
                                          },
                              Prop2 = 1
                          };

            obj.Dump();

            AssertVisited(  ObjectHeader,
                            ObjectSummary,
                                MemberName,
                                    EnumerableHeader,
                                        TypeInEnumerableHeader,
                                            Primitive,
                                        TypeInEnumerableFooter,
                                    EnumerableFooter,
                                MemberName, Primitive,
                            ObjectFooter);
        }

        [Test]
        public void Default_nesting_limit()
        {
            Func<Func<object>, Func<object>> create = c => () => new { Prop1 = c() };
            Func<Func<object>> ciao = () => () => "ciao";

            var obj = create(create(create(create(create(create(create(ciao())))))))();

            obj.Dump();

            Assert.AreEqual(5, visitor.MaxNestingLevel);
        }

        [Test]
        public void Custom_nesting_limit()
        {
            Func<Func<object>, Func<object>> create = c => () => new { Prop1 = c() };
            Func<Func<object>> ciao = () => () => "ciao";

            var obj = create(create(create(create(create(create(create(ciao())))))))();

            obj.Dump(2);

            Assert.AreEqual(2, visitor.MaxNestingLevel);
        }


        private void AssertVisited(params string[] expectedVisited)
        {
            CollectionAssert.AreEqual(expectedVisited, visitor.Visits);
        }
    }
}