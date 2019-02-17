using Microsoft.Glee;
using Microsoft.Glee.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GPoint = Microsoft.Glee.Splines.Point;

namespace Extellect.AutomaticGraphLayout
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

        public GleeGraph ToGraph(IGraphicsAdapter graphics, Font font, Func<T, string> nameSelector)
        {
            var graph = new GleeGraph();

            var nodes = Nodes(graphics, font, nameSelector)
                .ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

            foreach (var node in nodes.Values)
            {
                graph.AddNode(node);
            }

            foreach (var edge in Edges(nodes, false, nameSelector))
            {
                graph.AddEdge(edge);
            }

            return graph;
        }

        private IEnumerable<Edge> Edges(Dictionary<string, Node> nodes, bool flow, Func<T, string> nameSelector)
        {
            foreach (var dependency in _dependencies.SelectMany(x => x.Value.Select(y => new { Source = x.Key, Target = y })))
            {
                yield return new Edge(nodes[nameSelector(dependency.Source)], nodes[nameSelector(dependency.Target)])
                {
                    ArrowHeadAtSource = flow,
                    ArrowHeadAtTarget = !flow,
                };
            }
        }

        private IEnumerable<Node> Nodes(IGraphicsAdapter graphics, Font font, Func<T, string> nameSelector)
        {
            var keys = _dependencies.Keys.Select(nameSelector);
            var values = _dependencies.SelectMany(x => x.Value.Select(nameSelector));
            foreach (var text in keys.Concat(values).Distinct())
            {
                yield return NewNode(graphics, text, font);
            }
        }

        private Node NewNode(IGraphicsAdapter graphics, string text, Font font, bool isEllipse = false)
        {
            var size = graphics.MeasureString(text, font);

            float w = Math.Max(30, size.Width + 10);
            float h = Math.Max(20, size.Height + 5);

            ICurve curve;
            if (isEllipse)
            {
                curve = new Ellipse(w, h, new GPoint());
            }
            else
            {
                curve = CurveFactory.CreateBox(w, h, new GPoint());
            }

            return new Node(text, curve);
        }

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
