using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using NodeMapper.DataRepository;

namespace NodeMapper.Model
{
    public class GraphProvider
    {
        private static GraphProvider _instance;
        public static GraphProvider Instance => _instance ?? (_instance = new GraphProvider());

        private DbRepository _repo = new DbRepository();

        private Graph _graph;
        public Graph GraphViewerGraph => _graph;

        public IEnumerable<Node> AllNodes => _graph.Nodes;
        public Node FirstNode => _graph.Nodes.First();
        public int NodeCount => _graph.NodeCount;

        public GraphProvider()
        {
            _graph = _repo.LoadGraph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            _graph.LayoutAlgorithmSettings = settings;
            _graph.Attr.LayerDirection = LayerDirection.TB;

            if (!_graph.Nodes.Any())
            {
                CreateBasicGraph();
            }
        }

        private void CreateBasicGraph()
        {
            var nodeVillage = _graph.AddNode("Village");
            var nodeHill = _graph.AddNode("Hill");
            var nodeForest = _graph.AddNode("Forest");

            nodeVillage.UserData = "This is an isolated village.";
            nodeHill.UserData = "A ruined table lies at the top this hill.";
            nodeForest.UserData = "Rumors of a werewolf are connected to this forest.";

            _graph.AddEdge(nodeVillage.Id, "4h", nodeHill.Id);
            _graph.AddEdge(nodeVillage.Id, "4h", nodeForest.Id);
            _graph.AddEdge(nodeForest.Id, "2h", nodeHill.Id);
            _graph.AddEdge(nodeHill.Id, "2h", nodeForest.Id);
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(_graph);
        }

        public Node CreateNeNodeWithEdgeFrom(Node node)
        {
            var newEdge = _graph.AddEdge(node.Id, "New Node");
            _graph.GeometryGraph.UpdateBoundingBox();
            return newEdge.TargetNode;
        }

        public void RemoveNode(Node node)
        {
            _graph.RemoveNode(node);
        }

        public Node GetNeighborNode(Node node)
        {
            if (!node.Edges.Any()) return null;
            
            var firstEdge = node.Edges.First();
            // ReSharper disable once PossibleUnintendedReferenceComparison
            return firstEdge.SourceNode == node ? firstEdge.TargetNode : firstEdge.SourceNode;
        }

        public Edge AddEdge(string nodeFromId, string text, string nodeToId)
        {
            return _graph.AddEdge(nodeFromId, text, nodeToId);
        }

        public void RemoveEdge(Edge edge)
        {
            _graph.RemoveEdge(edge);
        }
    }
}