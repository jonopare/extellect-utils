using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Extellect.Nuvo
{
    public sealed class ResourcePool<T> : IDisposable
        where T : class
    {
        private readonly ConcurrentBag<T> _items = [];
        private readonly SemaphoreSlim _semaphore;
        private readonly int _capacity;
        private readonly Func<T> _factory;

        public ResourcePool(int capacity, Func<T> factory)
        {
            ArgumentOutOfRangeException.ThrowIfZero(capacity, nameof(capacity));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _semaphore = new SemaphoreSlim(capacity, capacity);
            _capacity = capacity;
        }

        public bool TryAcquire(TimeSpan timeout, CancellationToken cancellationToken, [MaybeNullWhen(false)] out Lease lease)
        {
            if (_semaphore.Wait(timeout, cancellationToken))
            {
                var item = _items.TryTake(out var existing) ? existing : _factory();
                lease = new Lease(this, item);
                return true;
            }
            lease = default;
            return false;
        }

        public async Task<Maybe<Lease>> AcquireAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (await _semaphore.WaitAsync(timeout, cancellationToken))
            {
                var item = _items.TryTake(out var existing) ? existing : _factory();
                return Maybe.Some(new Lease(this, item));
            }
            return Maybe.None<Lease>();
        }

        public void Dispose()
        {
            while (_items.TryTake(out var item))
                if (item is IDisposable disposable)
                    disposable.Dispose();

            if (_semaphore.CurrentCount != _capacity)
                throw new InvalidOperationException("ResourcePool disposed while items are still checked out.");

            _semaphore.Dispose();
        }

        private void Release(T item)
        {
            _items.Add(item);
            _semaphore.Release();
        }

        public readonly struct Lease : IDisposable
        {
            private readonly ResourcePool<T> _pool;
            public T Item { get; }

            internal Lease(ResourcePool<T> pool, T item)
            {
                _pool = pool;
                Item = item;
            }

            public void Dispose()
            {
                if (Item is not null)
                    _pool.Release(Item);
            }
        }
    }

}
