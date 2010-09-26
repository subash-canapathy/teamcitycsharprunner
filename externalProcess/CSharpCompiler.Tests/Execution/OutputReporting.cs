using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using CSharpCompiler.Runtime.Messages;
using NUnit.Framework;
using System.Linq;

namespace CsharpCompiler.Tests.Execution
{
    [TestFixture]
    public abstract class OutputReporting
    {
    	private TextWriter defaultOut;
    	private StringWriter newOut;

    	[SetUp]
		public void Setup()
		{
			defaultOut = Console.Out;
			newOut = new StringWriter();
			Console.SetOut(newOut);

			var messages = new ServiceMessages(new StringWriter());

    		var results = new CompositeCompiler(messages)
    		              	{
    		              		AdditionalNamespaces = AdditionalNamespaces,
    		              		AdditionalReferences = AdditionalReferences
    		              	}.Compile(Expression);

			Assume.That(results.Errors.HasErrors, Is.False, "Compilation failed: " + string.Join(Environment.NewLine, results.Errors.Cast<CompilerError>().Select(e => e.ToString()).ToArray()));

			new Executor(messages).Execute(results, AdditionalReferences.ToArray());
		}

    	protected virtual IEnumerable<string> AdditionalReferences { get { yield break; } }

    	protected virtual IEnumerable<string> AdditionalNamespaces { get { yield break; } }

    	protected abstract string Expression { get; }

    	protected string Output
    	{
			get { return newOut.GetStringBuilder().ToString(); }
    	}

		[TearDown]
		public void Teardown()
		{
			Console.SetOut(defaultOut);
		}
    }
}