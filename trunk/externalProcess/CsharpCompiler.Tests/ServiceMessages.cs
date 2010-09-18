using System;
using System.IO;
using CSharpCompiler.Runtime;
using CSharpCompiler.Runtime.Messages;
using NUnit.Framework;
using System.Linq;

namespace CSharpCompiler.Tests
{
    [TestFixture]
    public class ServiceMessages
    {
        private StringWriter outputWriter;

        [SetUp]
        public void Setup()
        {
            outputWriter = new StringWriter();
            TeamCityServiceMessagesExtensions.OutputWriter = outputWriter;
        }

        private string Output { get { return outputWriter.GetStringBuilder().ToString(); } }

        [Test]
        public void Success()
        {
            "ciao".Success();

            AssertMessage("##teamcity[buildStatus status='SUCCESS' text='ciao']");
        }

        [Test]
        public void Success_formatted()
        {
            "ciao".Success("hello {0}");

            AssertMessage("##teamcity[buildStatus status='SUCCESS' text='hello ciao']");
        }

        [Test]
        public void Failure()
        {
            "ciao".Failure();

            AssertMessage("##teamcity[buildStatus status='FAILURE' text='ciao']");
        }

        [Test]
        public void Failure_formatted()
        {
            "ciao".Failure("hello {0}");

            AssertMessage("##teamcity[buildStatus status='FAILURE' text='hello ciao']");
        }

        [Test]
        public void Publish()
        {
            "*.*".Publish();

            AssertMessage("##teamcity[publishArtifacts '*.*']");
        }

        [Test]
        public void Publish_single_target()
        {
            "*.*".Publish("mydir");

            AssertMessage("##teamcity[publishArtifacts '*.* => mydir']");
        }

        [Test]
        public void Publish_multiple_targets()
        {
            "*.*".Publish("mydir", "archive.zip");

            AssertMessage("##teamcity[publishArtifacts '*.* => mydir, archive.zip']");
        }

        [Test]
        public void Progress()
        {
            "wait".Progress();

            AssertMessage("##teamcity[progressMessage 'wait']");
        }

        [Test]
        public void Build_number()
        {
            "1.1.0".BuildNumber();

            AssertMessage("##teamcity[buildNumber '1.1.0']");
        }

        [Test]
        public void Statistics()
        {
            1.Statistic("myKey");

            AssertMessage("##teamcity[buildStatisticValue key='myKey' value='1']");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Statistics_cannot_use_default_keys()
        {
            1.Statistic(BuildStatisticMessage.PredefinedStatisticsKeys.First());
        }

        private void AssertMessage(string message)
        {
            Assert.AreEqual(message + Environment.NewLine, Output);
        }
    }
}