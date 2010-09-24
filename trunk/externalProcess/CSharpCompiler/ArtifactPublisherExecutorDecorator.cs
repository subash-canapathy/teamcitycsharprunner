using System.CodeDom.Compiler;
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

        public void Execute(CompilerResults results)
        {
            try
            {
                PublishReportStart();
                inner.Execute(results);
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
        	PublishProgressIndicator();
        	PublishReport();
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