﻿using System.IO;
using CSharpCompiler.Runtime.Messages;

namespace CSharpCompiler.Runtime.Dumping
{
    public class ArtifactObjectDumper : IObjectDumper
    {
        private readonly IFileOutputObjectVisitorFactory factory;
        private int currentSequence;

        public ArtifactObjectDumper(IFileOutputObjectVisitorFactory factory)
        {
            this.factory = factory;
        }

        public void Dump(object value, int maximumDepth)
        {
            var tempFileName = Path.Combine(Path.GetTempPath(), currentSequence++ + ".html");

            using (var visitor = factory.Create(tempFileName, maximumDepth))
                new VisitableObject(value).AcceptVisitor(visitor);

            tempFileName.Publish();
        }
    }
}