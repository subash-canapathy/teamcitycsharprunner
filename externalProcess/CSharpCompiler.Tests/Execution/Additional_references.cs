using System.Collections.Generic;
using NUnit.Framework;

namespace CSharpCompiler.Tests.Execution
{
	public class Additional_references : OutputReporting
	{
		protected override string Expression
		{
			get { return "Console.Write(1.Nop());"; }
		}

		protected override IEnumerable<string> AdditionalReferences
		{
			get { yield return @"ExternalReferences\One.dll"; }
		}

		protected override IEnumerable<string> AdditionalNamespaces
		{
			get { yield return "One"; }
		}

		[Test]
		public void Outcome()
		{
			Assert.AreEqual("1", Output);
		}
	}
}