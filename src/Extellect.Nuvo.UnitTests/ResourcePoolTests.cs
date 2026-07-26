namespace Extellect.Nuvo
{
    public class ResourcePoolTests
    {
        [Fact]
        public void TryAcquire()
        {
            var timeout = TimeSpan.FromSeconds(2);

            var resources = new Stack<string>(
            [
                "One",
                "Two",
                "Three",
                "Four",
            ]);

            var sut = new ResourcePool<string>(3, resources.Pop);

            var leases = new List<ResourcePool<string>.Lease>();
            for (var i = 0; i < 3; i++)
            {
                Assert.True(sut.TryAcquire(timeout, CancellationToken.None, out var lease));
                leases.Add(lease);
            }

            // Only 1 resource should not have been requested by the resource pool
            Assert.Single(resources);

            Assert.Equal(3, leases.Count);

            for (var i = leases.Count - 1; i >= 0; i--)
            {
                leases[i].Dispose();
                leases.RemoveAt(i);
            }

            Assert.Empty(leases);

            for (var i = 0; i < 3; i++)
            {
                Assert.True(sut.TryAcquire(timeout, CancellationToken.None, out var lease));
                leases.Add(lease);
            }

            Assert.False(sut.TryAcquire(timeout, CancellationToken.None, out var _));
        }

        [Fact]
        public async Task AcquireAsync()
        {
            var timeout = TimeSpan.FromSeconds(2);

            var resources = new Stack<string>(
            [
                "One",
                "Two",
                "Three",
                "Four",
            ]);

            var sut = new ResourcePool<string>(3, resources.Pop);

            var leases = new List<ResourcePool<string>.Lease>();
            for (var i = 0; i < 3; i++)
            {
                var (success, lease) = await sut.AcquireAsync(timeout, CancellationToken.None);
                Assert.True(success);
                leases.Add(lease);
            }

            // Only 1 resource should not have been requested by the resource pool
            Assert.Single(resources);

            Assert.Equal(3, leases.Count);

            for (var i = leases.Count - 1; i >= 0; i--)
            {
                leases[i].Dispose();
                leases.RemoveAt(i);
            }

            Assert.Empty(leases);

            for (var i = 0; i < 3; i++)
            {
                var (success, lease) = await sut.AcquireAsync(timeout, CancellationToken.None);
                Assert.True(success);
                leases.Add(lease);
            }

            {
                var (success, lease) = await sut.AcquireAsync(timeout, CancellationToken.None);
                Assert.False(success);
            }
        }
    }
}
