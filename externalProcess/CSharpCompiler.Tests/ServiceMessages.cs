using System;
using System.IO;
using CSharpCompiler.Runtime.Messages;
using NUnit.Framework;
using System.Linq;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class ServiceMessages
    {
        private StringWriter outputWriter;
    	private int currentIndex;

    	[SetUp]
        public void Setup()
    	{
    		currentIndex = 0;
            outputWriter = new StringWriter();
            TeamCityServiceMessagesExtensions.OutputWriter = outputWriter;
        }

    	private string Output
    	{
    		get
    		{
    			var currentOutput = outputWriter.GetStringBuilder().ToString().Substring(currentIndex);

    			currentIndex = currentOutput.Length;

    			return currentOutput;
    		}
    	}

        [Test]
        public void Success()
        {
            "ciao".Success();

            AssertMessage("buildStatus status='SUCCESS' text='ciao'");
        }

        [Test]
        public void Success_formatted()
        {
            "ciao".Success("hello {0}");

            AssertMessage("buildStatus status='SUCCESS' text='hello ciao'");
        }

        [Test]
        public void Failure()
        {
            "ciao".Failure();

            AssertMessage("buildStatus status='FAILURE' text='ciao'");
        }

        [Test]
        public void Failure_formatted()
        {
            "ciao".Failure("hello {0}");

            AssertMessage("buildStatus status='FAILURE' text='hello ciao'");
        }

        [Test]
        public void Publish()
        {
            "*.*".Publish();

            AssertMessage("publishArtifacts '*.*'");
        }

        [Test]
        public void Publish_single_target()
        {
            "*.*".Publish("mydir");

            AssertMessage("publishArtifacts '*.* => mydir'");
        }

        [Test]
        public void Publish_multiple_targets()
        {
            "*.*".Publish("mydir", "archive.zip");

            AssertMessage("publishArtifacts '*.* => mydir, archive.zip'");
        }

        [Test]
        public void Progress()
        {
            "wait".Progress();

            AssertMessage("progressMessage 'wait'");
        }

		[Test]
		public void Progress_block()
		{
			var block = "waiting".ProgressBlock();

			AssertMessage("progressStart 'waiting'");

			block.Dispose();

			AssertMessage("progressFinish 'waiting'");
		}

        [Test]
        public void Build_number()
        {
            "1.1.0".BuildNumber();

            AssertMessage("buildNumber '1.1.0'");
        }

        [Test]
        public void Statistics()
        {
            1.Statistic("myKey");

            AssertMessage("buildStatisticValue key='myKey' value='1'");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Statistics_cannot_use_default_keys()
        {
            1.Statistic(BuildStatisticMessage.PredefinedStatisticsKeys.First());
        }

        [Test]
        public void LogMessage()
        {
            1.LogMessage();

            AssertMessage("message text='1' status='NORMAL'");
        }

        [Test]
        public void LogWarning()
        {
            1.LogWarning();

            AssertMessage("message text='1' status='WARNING'");
        }

        [Test]
        public void LogFailure()
        {
            1.LogFailure();

            AssertMessage("message text='1' status='FAILURE'");
        }

        [Test]
        public void LogError_no_details()
        {
            1.LogError();

            AssertMessage("message text='1' status='ERROR'");
        }
        
        [Test]
        public void LogError_with_details()
        {
            1.LogError("some details");

            AssertMessage("message text='1' status='ERROR' errorDetails='some details'");
        }

        private void AssertMessage(string message)
        {
            Assert.AreEqual("##teamcity[" + message + "]" + Environment.NewLine, Output);
        }
    }
}