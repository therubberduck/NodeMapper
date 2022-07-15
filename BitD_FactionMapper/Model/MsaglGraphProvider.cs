using System.Linq;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;

namespace BitD_FactionMapper.Model
{
    public class MsaglGraphProvider
    {
        private static MsaglGraphProvider _instance;
        public static MsaglGraphProvider Instance => _instance ?? (_instance = new MsaglGraphProvider());

        private NodeDataManager _nodeDataManager;

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
            if (_nodeDataManager == null)
            {
                _nodeDataManager = NodeDataManager.Instance;
            }

            var graph = new Graph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            graph.LayoutAlgorithmSettings = settings;
            graph.Attr.LayerDirection = LayerDirection.TB;

            foreach (var node in _nodeDataManager.AllNodes)
            {
                var graphNode = new Microsoft.Msagl.Drawing.Node(node.Title);
                graphNode.Attr.Id = node.NodeId;
                graphNode.Attr.Color = Color.Black;
                graphNode.Label.Text = node.Title;
                graphNode.UserData = node.Body;
                graph.AddNode(graphNode);
            }

            foreach (var edge in _nodeDataManager.Edges)
            {
                var graphEdge = graph.AddEdge(edge.SourceId, edge.LabelText, edge.TargetId);
                graphEdge.Attr.Id = edge.EdgeId;
            }

            _graph = graph;

            return graph;
        }

        
        public Graph GetUpdatedGraph()
        {
            // Update colors and such
            throw new System.NotImplementedException();
        }

        public void MarkNodeSelected(string deSelectedId, string selectedId)
        {
            _graph.Nodes.First(n => n.Attr.Id == deSelectedId).Attr.Color = Color.Black;
            _graph.Nodes.First(n => n.Attr.Id == selectedId).Attr.Color = Color.Red;
        }

        public void SelectNode(string nodeId)
        {
            var node = _nodeDataManager.GetNode(nodeId);
            _nodeDataManager.SelectedNode = node;
        }

        public void SelectEdge(string edgeId)
        {
            var edge = _nodeDataManager.GetEdge(edgeId);
            _nodeDataManager.SelectedEdge = edge;
        }
    }
}