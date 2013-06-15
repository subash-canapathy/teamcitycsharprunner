using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using CSharpCompiler;
using CSharpCompiler.Runtime;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler
{
    class ArtifactPublisherExecutorDecorator : IExecutor
    {
	    private readonly string artifactsPath;
	    private readonly string reportName;
	    private readonly IExecutor inner;
        private readonly IServiceMessages serviceMessages;

        public ArtifactPublisherExecutorDecorator(string artifactsPath, string reportName, IExecutor inner, IServiceMessages serviceMessages)
        {
	        this.artifactsPath = artifactsPath;
	        this.reportName = reportName;
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

	    private void PublishReport()
	    {
		    var path = PathUtilities.MakeTempPath(reportName);
		    
			File.WriteAllText(path, Reports.CSharpOutput);
		    serviceMessages.Publish(path, artifactsPath);
	    }

	    private void PublishProgressIndicator()
	    {
		    var path = PathUtilities.MakeTempPath(Constants.AjaxLoaderFileName);
		    
			Reports.ajax_loader.Save(path);
			serviceMessages.Publish(path, artifactsPath);
	    }

	    private void PublishUpImage()
	    {
		    var path = PathUtilities.MakeTempPath(Constants.UpImageFileName);
		    
			Reports.up.Save(path);
			serviceMessages.Publish(path, artifactsPath);
	    }

	    private void PublishDownImage()
	    {
		    var path = PathUtilities.MakeTempPath(Constants.DownImageFileName);
		    
			Reports.down.Save(path);
			serviceMessages.Publish(path, artifactsPath);
	    }

	    private void PublishReportEnd()
        {
            var path = PathUtilities.MakeTempPath(Constants.ReportEndFileName);
            
			File.WriteAllText(path, string.Empty);
			serviceMessages.Publish(path, artifactsPath);
        }
    }
}