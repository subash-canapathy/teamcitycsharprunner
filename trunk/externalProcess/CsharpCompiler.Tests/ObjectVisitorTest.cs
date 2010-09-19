using System;
using CSharpCompiler.Runtime;
using CSharpCompiler.Runtime.Dumping;
using NUnit.Framework;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class ObjectVisitorTest : SpyVisitorProperties
    {
        private SpyVisitor sut;

        [SetUp]
        public void Setup()
        {
            sut = new SpyVisitor();
        }

        [TestCase(1)]
        [TestCase(1.1)]
        [TestCase("ciao")]
        public void Simple_types(object value)
        {
            DoVisit(value);

            AssertVisited(Primitive);
        }

        private void DoVisit(object value)
        {
            new VisitableObject(value).AcceptVisitor(sut);
        }

        [Test]
        public void DateTime_is_simple()
        {
            DoVisit(DateTime.Now);

            AssertVisited(Primitive);
        }

        [Test]
        public void SimpleEnumerable()
        {
            var oneTwoThree = new[] { 1, 2, 3 };
            DoVisit(oneTwoThree);

            AssertVisited(EnumerableHeader, Primitive, Primitive, Primitive, EnumerableFooter);
        }

        [Test]
        public void Plain_object()
        {
            var obj = new {Prop1 = "ciao", Prop2 = "hello"};
            DoVisit(obj);


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

            DoVisit(obj);

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

            DoVisit(obj);

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

            sut.MaximumDepth = 5;
            DoVisit(obj);

            Assert.AreEqual(5, sut.MaximumReachedDepth);
        }

        [Test]
        public void Custom_nesting_limit()
        {
            Func<Func<object>, Func<object>> create = c => () => new { Prop1 = c() };
            Func<Func<object>> ciao = () => () => "ciao";

            var obj = create(create(create(create(create(create(create(ciao())))))))();

            sut.MaximumDepth = 2;
            DoVisit(obj);

            Assert.AreEqual(2, sut.MaximumReachedDepth);
        }

        private void AssertVisited(params string[] expectedVisited)
        {
            CollectionAssert.AreEqual(expectedVisited, sut.Visits);
        }
    }
}