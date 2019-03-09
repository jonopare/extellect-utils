using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extellect.Collections
{
    public class Digraph<T> : IEnumerable<DigraphEdge<T>>
    {
        private readonly Dictionary<T, HashSet<T>> _dependencies;

        public Digraph()
            : this(EqualityComparer<T>.Default)
        {
        }

        public Digraph(IEqualityComparer<T> comparer)
        {
            _dependencies = new Dictionary<T, HashSet<T>>(comparer);
        }

        /// <summary>
        /// Adds a node that is dependent on the specified precedent
        /// </summary>
        public void Add(T dependent, T precedent)
        {
            if (!_dependencies.TryGetValue(dependent, out HashSet<T> precedents))
            {
                precedents = new HashSet<T>();
                _dependencies.Add(dependent, precedents);
            }
            precedents.Add(precedent);
        }

        /// <summary>
        /// Adds an independent node with no precedents
        /// </summary>
        public void Add(T independent)
        {
            if (!_dependencies.TryGetValue(independent, out HashSet<T> precedents))
            {
                precedents = new HashSet<T>();
                _dependencies.Add(independent, precedents);
            }
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

        public IEnumerable<T> Roots
        {
            get
            {
                return _dependencies.SelectMany(x => x.Value).Except(_dependencies.Where(x => x.Value.Any()).Select(x => x.Key), _dependencies.Comparer);
            }
        }

        public IEnumerable<T> Dependents(T precedent)
        {
            var result = new HashSet<T>(_dependencies.Comparer);

            Dependents(precedent, result);

            return result;
        }

        public IEnumerable<T> DirectDependents(T precedent) => _dependencies.Where(x => x.Value.Contains(precedent)).Select(x => x.Key);

        private void Dependents(T precedent, HashSet<T> seenDependents)
        {
            var unseenDependents = new HashSet<T>(DirectDependents(precedent).Except(seenDependents, _dependencies.Comparer), _dependencies.Comparer);

            foreach (var dependent in unseenDependents)
            {
                seenDependents.Add(dependent);
            }

            foreach (var dependent in unseenDependents)
            {
                Dependents(dependent, seenDependents);
            }
        }

        public IEnumerable<T> Precedents(T dependent)
        {
            var result = new HashSet<T>(_dependencies.Comparer);

            Precedents(dependent, result);

            return result;
        }

        public IEnumerable<T> DirectPrecedents(T dependent) => _dependencies.TryGetValue(dependent, out HashSet<T> directPrecedents) ? directPrecedents : Enumerable.Empty<T>();

        private void Precedents(T dependent, HashSet<T> seenPrecedents)
        {
            var unseenPrecedents = new HashSet<T>(DirectPrecedents(dependent).Except(seenPrecedents, _dependencies.Comparer), _dependencies.Comparer);

            foreach (var precedent in unseenPrecedents)
            {
                seenPrecedents.Add(precedent);
            }

            foreach (var precedent in unseenPrecedents)
            {
                Precedents(precedent, seenPrecedents);
            }
        }

        public bool ContainsCycles
        {
            get
            {
                return Nodes.Any(x => Precedents(x).Any(y => _dependencies.Comparer.Equals(x, y)));
            }
        }

        private IEnumerable<T> Nodes => _dependencies.Keys.Union(_dependencies.Values.SelectMany(x => x), _dependencies.Comparer);

        public void Print(TextWriter writer, Func<T, string> toString = null)
        {
            if (toString == null)
            {
                toString = x => x?.ToString();
            }
            foreach (var key in _dependencies.Keys)
            {
                writer.Write(toString(key));
                writer.Write(" -> [");
                var separator = false;
                foreach (var value in _dependencies[key])
                {
                    if (separator)
                    {
                        writer.Write(", ");
                    }
                    else
                    {
                        separator = true;
                    }
                    writer.Write(toString(value));
                }
                writer.WriteLine("]");
            }
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
