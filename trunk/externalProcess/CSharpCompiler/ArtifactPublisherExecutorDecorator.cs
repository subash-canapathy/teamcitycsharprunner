using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using CSharpCompiler;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    class ArtifactPublisherExecutorDecorator : IExecutor
    {
        private readonly IExecutor inner;
        private readonly IServiceMessages serviceMessages;

        public ArtifactPublisherExecutorDecorator(IExecutor inner, IServiceMessages serviceMessages)
        {
            this.inner = inner;
            this.serviceMessages = serviceMessages;
        }

        public void Execute(CompilerResults results, IEnumerable<string> additionalReferences)
        {
            try
            {
                PublishReportStart();
                inner.Execute(results, additionalReferences);
            }
            finally
            {
                PublishReportEnd();
            }
        }

        private void PublishReportEnd()
        {
            var path = Path.Combine(Path.GetTempPath(), "end");
            File.WriteAllText(path, string.Empty);
            serviceMessages.Publish(path);
        }

        private void PublishReportStart()
        {
        	PublishResources();
        	PublishReport();
        }

    	private void PublishResources()
    	{
    		PublishProgressIndicator();
    		PublishUpImage();
    		PublishDownImage();
    	}

    	private void PublishDownImage()
    	{
			var path = Path.Combine(Path.GetTempPath(), "down.png");
			Reports.down.Save(path);
			serviceMessages.Publish(path);
    	}

    	private void PublishUpImage()
    	{
			var path = Path.Combine(Path.GetTempPath(), "up.png");
			Reports.up.Save(path);
			serviceMessages.Publish(path);
    	}

    	private void PublishProgressIndicator()
    	{
    		var path = Path.Combine(Path.GetTempPath(), "ajax-loader.gif");
    		Reports.ajax_loader.Save(path);
    		serviceMessages.Publish(path);
    	}

    	private void PublishReport()
    	{
    		var path = Path.Combine(Path.GetTempPath(), "CSharpOutput.html");
    		File.WriteAllText(path, Reports.CSharpOutput);
    		serviceMessages.Publish(path);
    	}
    }
}