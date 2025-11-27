using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// Allows locking of resources within an async block of code
    /// </summary>
    /// <remarks>
    /// http://www.hanselman.com/blog/ComparingTwoTechniquesInNETAsynchronousCoordinationPrimitives.aspx 
    /// </remarks>
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> _releaser;

        public AsyncLock()
        {
            _releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync(CancellationToken ct = default(CancellationToken))
        {
            var wait = _semaphore.WaitAsync(ct);
            return wait.IsCompleted
                ? _releaser
                : wait.ContinueWith((_, state) => (IDisposable)state,
                    _releaser.Result, ct,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
        }

        private sealed class Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;

            internal Releaser(AsyncLock toRelease)
            {
                m_toRelease = toRelease;
            }

            public void Dispose()
            {
                m_toRelease._semaphore.Release();
            }
        }
    }
}
