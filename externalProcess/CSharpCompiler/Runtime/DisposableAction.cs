using System;

namespace CSharpCompiler.Runtime
{
    public class DisposableAction : IDisposable
    {
        private readonly Action onDispose;

        public DisposableAction(Action onInitialize, Action onDispose)
        {
            this.onDispose = onDispose;
            onInitialize();
        }

        public void Dispose()
        {
            onDispose();
        }
    }
}