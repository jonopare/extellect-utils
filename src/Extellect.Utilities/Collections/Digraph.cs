using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Extellect.Collections
{
    public class Digraph<T> : IEnumerable<DigraphEdge<T>>
    {
        private readonly Dictionary<T, HashSet<T>> _dependencies;
        
        public Digraph(IEqualityComparer<T> comparer)
        {
            _dependencies = new Dictionary<T, HashSet<T>>(comparer);
        }

        public void Add(T dependent, T precedent)
        {
            if (!_dependencies.TryGetValue(dependent, out HashSet<T> precedents))
            {
                precedents = new HashSet<T>();
                _dependencies.Add(dependent, precedents);
            }
            precedents.Add(precedent);
        }

        public bool ContainsKey(T key) => _dependencies.ContainsKey(key);

        public Digraph<T> Reverse()
        {
            var result = new Digraph<T>(_dependencies.Comparer);
            foreach (var dependency in _dependencies.SelectMany(x => x.Value.Select(y => new { Source = x.Key, Target = y })))
            {
                Add(dependency.Target, dependency.Source);
            }
            return result;
        }

        public IEnumerator<DigraphEdge<T>> GetEnumerator()
        {
            return _dependencies.SelectMany(x => x.Value.Select(y => new DigraphEdge<T>(x.Key, y)))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
