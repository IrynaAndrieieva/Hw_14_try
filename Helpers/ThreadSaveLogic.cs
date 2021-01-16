using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hw_14_try.Helpers
{
    public class ThreadSaveLogic
    {
        private static readonly Lazy<ThreadSaveLogic> _instance = new Lazy<ThreadSaveLogic>(() => new ThreadSaveLogic());
        public static ThreadSaveLogic Instance => _instance.Value;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private ThreadSaveLogic()
        {

        }

        public async Task<TResult> ThreadSaveAction<TResult>(Func<TResult> func)
        {

            await _semaphoreSlim.WaitAsync();

            var result = default(TResult);
            try
            {
                result = await Task.Run(() => func.Invoke());
            }
            finally
            {
                _semaphoreSlim.Release();
            }
            return result;
        }
    }
}
