using NUnit.Framework;

namespace CsharpCompiler.Tests.Compilation
{
	[TestFixture]
	public class Statement
	{
		private CompositeCompiler compiler;

		[SetUp]
		public void Setup()
		{
			compiler = new CompositeCompiler();
		}

		[Test]
		public void Simple_statement()
		{
			var result = compiler.Compile("var a = 1;");

			Assert.IsEmpty(result.Errors);
		}


		[Test]
		[Platform("Net-4.0")]
		public void Simple_statement_with_dynamic_succeeds_on_NET_v4()
		{
			var result = compiler.Compile("dynamic a = 1;");

			Assert.IsEmpty(result.Errors);
		}

		[Test]
		[Platform("Net-2.0")]
		public void Simple_statement_with_dynamic_fails_on_NET_v2()
		{
			var result = compiler.Compile("dynamic a = 1;");

			Assert.IsNotEmpty(result.Errors);
		}
	}
}