using System.CodeDom.Compiler;
using System.IO;
using CSharpCompiler;
using CSharpCompiler.Runtime.Messages;

namespace CsharpCompiler
{
    class ArtifactPublisherExecutorDecorator : IExecutor
    {
        private readonly IExecutor inner;

        public ArtifactPublisherExecutorDecorator(IExecutor inner)
        {
            this.inner = inner;
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

        private static void PublishReportEnd()
        {
            var path = Path.Combine(Path.GetTempPath(), "end");
            File.WriteAllText(path, string.Empty);
            path.Publish();
        }

        private static void PublishReportStart()
        {
            var path = Path.Combine(Path.GetTempPath(), "CSharpOutput.html");
            File.WriteAllText(path, Reports.CSharpOutput);
            path.Publish();
        }
    }
}