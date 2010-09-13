using NUnit.Framework;

namespace CsharpCompiler.Tests.Compilation
{
    [TestFixture]
    public class Expression
    {
        private Compiler compiler;

        [SetUp]
        public void Setup()
        {
            compiler = new Compiler();            
        }

        [Test]
        public void Does_it_throw()
        {
            compiler.Compile("System.Console.WriteLine(\"hello\")");
        }

        [Test]
        public void Does_it_compile_successfully()
        {
            var results = compiler.Compile("System.Console.WriteLine(\"hello\")");
            
            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void With_non_qualified_name()
        {
            var results = compiler.Compile("Console.WriteLine(\"hello\")");

            Assert.IsEmpty(results.Errors);
        }

        [Test]
        public void Should_generate_assembly_with_entrypoint()
        {
            var results = compiler.Compile("Console.WriteLine(\"hello\")");

            Assert.IsNotNull(results.CompiledAssembly.EntryPoint);
        }
    }
}