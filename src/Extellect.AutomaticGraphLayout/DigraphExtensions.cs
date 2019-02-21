using Extellect.Collections;
using Microsoft.Glee;
using Microsoft.Glee.Splines;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPoint = Microsoft.Glee.Splines.Point;

namespace Extellect.AutomaticGraphLayout
{
    public static class DigraphExtensions
    {
        public static GleeGraph ToGraph<T>(this Digraph<T> digraph, IGraphicsAdapter graphics, Font font, Func<T, string> nameSelector)
        {
            var graph = new GleeGraph();

            var nodes = Nodes(digraph, graphics, font, nameSelector)
                .ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);

            foreach (var node in nodes.Values)
            {
                graph.AddNode(node);
            }

            foreach (var edge in Edges(digraph, nodes, false, nameSelector))
            {
                graph.AddEdge(edge);
            }

            return graph;
        }

        private static IEnumerable<Edge> Edges<T>(Digraph<T> digraph, Dictionary<string, Node> nodes, bool flow, Func<T, string> nameSelector)
        {
            foreach (var digraphEdge in digraph)
            {
                yield return new Edge(nodes[nameSelector(digraphEdge.Source)], nodes[nameSelector(digraphEdge.Target)])
                {
                    ArrowHeadAtSource = flow,
                    ArrowHeadAtTarget = !flow,
                };
            }
        }

        private static IEnumerable<Node> Nodes<T>(Digraph<T> digraph, IGraphicsAdapter graphics, Font font, Func<T, string> nameSelector)
        {
            var texts = digraph.SelectMany(x => new[] { nameSelector(x.Source), nameSelector(x.Target) }).Distinct();
            foreach (var text in texts)
            {
                yield return NewNode(graphics, text, font);
            }
        }

        private static Node NewNode(IGraphicsAdapter graphics, string text, Font font, bool isEllipse = false)
        {
            var size = graphics.MeasureString(text, font);

            float w = System.Math.Max(30, size.Width + 10);
            float h = System.Math.Max(20, size.Height + 5);

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
    }
}
