using NUnit.Framework;

namespace CsharpCompiler.Tests.Execution
{
    [TestFixture]
    public class OutputReporting
    {
        [Test]
        public void Test()
        {
            var results = new Compiler().Compile("Console.WriteLine(\"hello\")");

            new Executor().Execute(results);
        }
    }
}