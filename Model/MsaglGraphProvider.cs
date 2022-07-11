using System.Collections.Generic;
using System.Windows.Ink;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;

namespace NodeMapper.Model
{
    public class MsaglGraphProvider
    {
        private static MsaglGraphProvider _instance;
        public static MsaglGraphProvider Instance => _instance ?? (_instance = new MsaglGraphProvider());

        private GraphManager _graphManager;

        public delegate void ReloadGraphDelegate();

        public ReloadGraphDelegate ReloadGraph;

        public delegate void InvalidateGraphDelegate();

        public InvalidateGraphDelegate InvalidateGraph;

        private Graph _graph;

        public Graph GetGraph()
        {
            return _graph ?? GetNewGraph();
        }

        public Graph GetNewGraph()
        {
            if (_graphManager == null)
            {
                _graphManager = GraphManager.Instance;
            }

            var graph = new Graph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            graph.LayoutAlgorithmSettings = settings;
            graph.Attr.LayerDirection = LayerDirection.TB;

            foreach (var node in _graphManager.AllNodes)
            {
                var graphNode = new Microsoft.Msagl.Drawing.Node(node.Title);
                graphNode.Attr.Id = node.NodeId;
                graphNode.Label.Text = node.Title;
                graphNode.UserData = node.Body;
                graph.AddNode(graphNode);
            }

            foreach (var edge in _graphManager.Edges)
            {
                var graphEdge = graph.AddEdge(edge.SourceId, edge.LabelText, edge.TargetId);
                graphEdge.Attr.Id = edge.EdgeId;
            }

            _graph = graph;

            return graph;
        }
    }
}