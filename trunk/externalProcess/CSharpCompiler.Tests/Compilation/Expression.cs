using System.IO;
using CSharpCompiler.Runtime.Messages;
using NUnit.Framework;

namespace CSharpCompiler.Tests.Compilation
{
    [TestFixture]
    public class Expression
    {
        private CompositeCompiler compiler;

        [SetUp]
        public void Setup()
        {
            compiler = new CompositeCompiler(new ServiceMessages(new StringWriter()));            
        }

        [Test]
        public void Does_it_throw()
        {
            compiler.Compile("\"hello\"");
        }

        [Test]
        public void Does_it_compile_successfully()
        {
            var results = compiler.Compile("\"hello\"");
            
            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void With_non_qualified_name()
        {
            var results = compiler.Compile("String.IsNullOrEmpty(\"hello\")");

            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void Should_generate_assembly_with_entrypoint()
        {
            var results = compiler.Compile("\"hello\"");

            Assert.IsNotNull(results.CompiledAssembly.EntryPoint);
        }

        [Test]
        public void When_does_not_compile_should_return_error_line()
        {
            var results = compiler.Compile("String.IsNULLOrEmpty(\"hello\")");

            Assert.AreEqual(1, results.Errors[0].Line);
        }

        [Test]
        public void When_does_not_compile_should_return_error_colum()
        {
            const string expression = "string.ISNullOrempty(\"hello\")";
            var results = compiler.Compile(expression);

            Assert.AreEqual(expression.IndexOf("SNull"), results.Errors[0].Column);
        }

        [Test]
        public void Should_call_utility_methods_as_extensions()
        {
            var results = compiler.Compile("\"hello\".Success(\"hello\")");

            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void Should_call_utility_methods_as_statics()
        {
            var results = compiler.Compile("TeamCityServiceMessagesExtensions.Success<string>(\"hello\")");

            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void Should_try_to_add_Dump()
        {
            var results = compiler.Compile("\"hello\"");

            Assert.IsEmpty(results.Errors);
        }
    }
}