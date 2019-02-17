using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace Extellect.IO
{
    /// <summary>
    /// Wraps a FileSystemWatcher in an observable stream of throttled events.
    /// Events will only be raised after the specified quiet interval has elapsed
    /// for the group. I like to think of this as a gourmet chef operating 
    /// multiple microwaves to make popcorn for diners. Each microwave represents
    /// a group, a popping kernel is an event, and the chef must stop each microwave
    /// as soon as the gap in between pops exceeds a threshold to prevent
    /// the bag of popcorn from burning.
    /// </summary>
    public class ThrottledFileSystemWatcher<TKey> : IDisposable
    {
        private IDisposable _subscription;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public ThrottledFileSystemWatcher(FileSystemWatcher fileSystemWatcher, TimeSpan quietInterval, Func<FileSystemEventArgs, bool> filter, Func<FileSystemEventArgs, TKey> groupKeySelector, Action<FileSystemEventArgs> onNext)
        {
            var created = Observable.FromEventPattern(fileSystemWatcher, "Created"); 
            var changed = Observable.FromEventPattern(fileSystemWatcher, "Changed"); 
            var renamed = Observable.FromEventPattern(fileSystemWatcher, "Renamed"); 

            var composite = created.Merge(changed).Merge(renamed);

            _subscription = composite
                .Select(x => (FileSystemEventArgs)x.EventArgs)
                .Where(x => filter(x))
                .GroupBy(x => groupKeySelector(x))
                .SelectMany(x => x.Throttle(quietInterval))
                .Subscribe(x => onNext(x));
        }
    
        /// <summary>
        /// Disposes the underlying event stream subscription
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription.Dispose();
            }
        }
    }
}
