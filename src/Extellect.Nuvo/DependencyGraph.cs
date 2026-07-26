using System.Text;

namespace Extellect.Nuvo
{
    public class DependencyGraph<T> where T : notnull
    {
        private readonly Dictionary<T, HashSet<T>> _graph;

        public DependencyGraph()
        {
            _graph = new Dictionary<T, HashSet<T>>();
        }

        /// <summary>
        /// Right depends on Left. Do we need to change the parameter order to make for more understandable programming?!
        /// </summary>
        public void Add(T localIndependent, T localDependent)
        {
            _ = EnsureLocalDependents(localIndependent).Add(localDependent);
            _ = EnsureLocalDependents(localDependent);
        }

        public void Add(T localIndependent)
        {
            _ = EnsureLocalDependents(localIndependent);
        }

        public void Add(Dependency<T> dependency)
        {
            Add(dependency.Independent, dependency.Dependent);
        }

        public void AddRange(IEnumerable<Dependency<T>> dependencies)
        {
            foreach (var dependency in dependencies)
            {
                Add(dependency);
            }
        }

        public HashSet<T> EnsureLocalDependents(T localIndependent)
        {
            if (!_graph.TryGetValue(localIndependent, out var localDependents))
            {
                localDependents = new HashSet<T>();
                _graph.Add(localIndependent, localDependents);
            }
            return localDependents;
        }

        public bool Remove(T localIndependent, bool removeLocalIndependent = false)
        {
            if (!_graph.TryGetValue(localIndependent, out var dependents))
            {
                return false;
            }
            if (removeLocalIndependent)
            {
                return _graph.Remove(localIndependent);
            }
            return false;
        }

        public bool Remove(T localIndependent, T localDependent, bool removeLocalIndependent = false)
        {
            if (!_graph.TryGetValue(localIndependent, out var dependents))
            {
                return false;
            }
            var result = dependents.Remove(localDependent);
            if (!dependents.Any() && removeLocalIndependent)
            {
                _graph.Remove(localIndependent);
            }
            return result;
        }

        public bool ContainsLocalIndependent(T localIndependent)
        {
            return _graph.ContainsKey(localIndependent);
        }

        public bool ContainsCycles
        {
            get
            {
                foreach (var localIndependent in LocalIndependents)
                {
                    var chain = new HashSet<T>();
                    if (Navigate(localIndependent, chain))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool Navigate(T localIndependent, HashSet<T> chain)
        {
            if (chain.Contains(localIndependent))
            {
                return true;
            }
            chain.Add(localIndependent);
            foreach (var dependent in LocalDependents(localIndependent))
            {
                if (Navigate(dependent, chain))
                {
                    return true;
                }
            }
            chain.Remove(localIndependent);
            return false;
        }

        public DependencyGraph<T> Invert()
        {
            var result = new DependencyGraph<T>();
            foreach (var independent in LocalIndependents)
            {
                var count = 0;
                foreach (var dependent in LocalDependents(independent))
                {
                    count++;
                    result.Add(dependent, independent);
                }
                if (count == 0)
                {
                    result.Add(independent);
                }
            }
            return result;
        }

        public IEnumerable<T> LocalDependents(T localIndependent)
        {
            if (_graph.TryGetValue(localIndependent, out var localDependents))
            {
                return localDependents;
            }
            return Enumerable.Empty<T>();
        }

        public IEnumerable<T> LocalIndependents
        {
            get
            {
                return _graph.Keys;
            }
        }

        public IEnumerable<T> GlobalDependents(T globalIndependent)
        {
            var result = new HashSet<T>();
            if (_graph.TryGetValue(globalIndependent, out var localDependents))
            {
                GlobalDependents(globalIndependent, localDependents, result);
            }
            return result;
        }

        private void GlobalDependents(T independent, HashSet<T> dependents, HashSet<T> chain)
        {
            chain.Add(independent);
            foreach (var dependent in dependents)
            {
                if (!chain.Contains(dependent) && _graph.TryGetValue(dependent, out var localDependents))
                {
                    GlobalDependents(dependent, localDependents, chain);
                }
            }
        }

        public IEnumerable<T> GlobalIndependents
        {
            get
            {
                return _graph.Keys.Except(_graph.Values.SelectMany(x => x));
            }
        }

        public IEnumerable<IEnumerable<T>> Linearize()
        {
            if (ContainsCycles)
            {
                throw new InvalidOperationException("Cannot linearize a graph with cycles");
            }

            var localIndependents = LocalIndependents.ToList();

            while (localIndependents.Any())
            {
                var dependents = localIndependents.SelectMany(x => LocalDependents(x));

                var sack = localIndependents.Except(dependents)
                    .ToArray();

                if (!sack.Any())
                {
                    throw new InvalidOperationException("Something went horribly wrong!");
                }

                foreach (var cat in sack)
                {
                    localIndependents.Remove(cat);
                }

                yield return sack;
            }
        }

        public DependencyGraph<T> Merge(DependencyGraph<T> other)
        {
            var result = Copy();
            foreach (var independent in other.LocalIndependents)
            {
                result.Add(independent);
                foreach (var dependent in other.LocalDependents(independent))
                {
                    result.Add(independent, dependent);
                }
            }
            return result;
        }

        public DependencyGraph<T> Copy()
        {
            var result = new DependencyGraph<T>();
            foreach (var localIndependent in LocalIndependents)
            {
                result.Add(localIndependent);
                foreach (var localDependent in LocalDependents(localIndependent))
                {
                    result.Add(localIndependent, localDependent);
                }
            }
            return result;
        }

        public bool HasDependents(T localIndependent)
        {
            return _graph.TryGetValue(localIndependent, out var localDependents)
                && localDependents.Any();
        }

        /// <summary>
        /// Returns a string representation that approximates a neo4j Cypher match clause
        /// </summary>
        public override string ToString()
        {
            var text = new StringBuilder();
            var inverted = Invert(); // We work on an inverted graph for the string representation
            foreach (var independent in inverted.LocalIndependents)
            {
                if (inverted.HasDependents(independent))
                {
                    foreach (var dependent in inverted.LocalDependents(independent))
                    {
                        if (text.Length > 0)
                        {
                            text.AppendLine(",");
                        }
                        // The relationship appears flipped because we inverted the graph
                        text.Append($"(:{dependent})<-[:DEPENDS_ON]-(:{independent})");
                    }
                }
                else
                {
                    if (text.Length > 0)
                    {
                        text.AppendLine(",");
                    }
                    text.Append($"(:{independent})");
                }
            }
            return text.ToString();
        }

        /// <summary>
        /// Trims a graph to a subset of elements, but maintains the transitive
        /// dependencies that would otherwise have been destroyed if the elements
        /// were simply removed. For example: consider a graph A->B->C->D and
        /// a keep function that gets rid of items B and C. This function will
        /// return the graph A->D, instead of just A,D.
        /// </summary>
        public DependencyGraph<T> Trim(Func<T, bool> keep)
        {
            var result = new DependencyGraph<T>();

            foreach (var independent in LocalIndependents.Where(keep))
            {
                var localDependents = LocalDependents(independent);
                if (localDependents.Any())
                {
                    foreach (var localDependent in localDependents)
                    {
                        if (keep(localDependent))
                        {
                            result.Add(independent, localDependent);
                        }
                        else
                        {
                            void recurse(T dep, HashSet<T> visited)
                            {
                                if (!visited.Contains(dep))
                                    visited.Add(dep);
                                else
                                    return;
                                var subs = LocalDependents(dep);
                                if (subs.Any())
                                {
                                    foreach (var sub in subs)
                                    {
                                        if (keep(sub))
                                        {
                                            result.Add(independent, sub);
                                        }
                                        else
                                        {
                                            recurse(sub, visited);
                                        }
                                    }
                                }
                                else
                                {
                                    result.Add(independent);
                                }
                            }
                            recurse(localDependent, new HashSet<T>());
                        }
                    }
                }
                else
                {
                    result.Add(independent);
                }


                //foreach (var dependent in GlobalDependents(independent).Where(keep))
                //{
                //    result.Add(independent, dependent);
                //}
            }

            return result;
        }
    }
}
