using System.IO;
using CSharpCompiler.Runtime.Messages;
using NUnit.Framework;

namespace CsharpCompiler.Tests.Execution
{
    [TestFixture]
    public class OutputReporting
    {
        [Test]
        public void Test()
        {
            var messages = new ServiceMessages(new StringWriter());

            var results = new CompositeCompiler(messages).Compile("Console.WriteLine(\"hello\");");

            new Executor(messages).Execute(results);
        }
    }
}