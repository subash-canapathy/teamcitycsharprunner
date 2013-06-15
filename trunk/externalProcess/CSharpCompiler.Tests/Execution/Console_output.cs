using NUnit.Framework;

namespace CSharpCompiler.Tests.Execution
{
	public class Console_output : OutputReporting
	{
		protected override string Expression
		{
			get { return "Console.Write(\"hello\");"; }
		}

		[Test]
		public void Test()
		{
			Assert.AreEqual("hello", Output);
		}
	}
}