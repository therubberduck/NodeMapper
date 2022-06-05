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
            else
            {
                foreach (var graphNode in _graph.Nodes)
                {
                    if (graphNode.Label != null)
                    {
                        graphNode.Label.UserData = graphNode.Attr.Id;
                    }
                }
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

            var edge1 = _graph.AddEdge(nodeVillage.Id, "4h", nodeHill.Id);
            edge1.Attr.Id = "1";
            var edge2 = _graph.AddEdge(nodeVillage.Id, "4h", nodeForest.Id);
            edge2.Attr.Id = "2";
            var edge3 = _graph.AddEdge(nodeForest.Id, "2h", nodeHill.Id);
            edge3.Attr.Id = "3";
            var edge4 = _graph.AddEdge(nodeHill.Id, "2h", nodeForest.Id);
            edge4.Attr.Id = "4";
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

        public Node GetNode(string nodeId)
        {
            return _graph.FindNode(nodeId);
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
            var graphEdge = _graph.AddEdge(nodeFromId, text, nodeToId);
            return MakeEdge(graphEdge);
        }

        public void RemoveEdge(Edge edge)
        {
            var graphEdge = _graph.EdgeById(edge.EdgeId);
            _graph.RemoveEdge(graphEdge);
        }

        public Edge GetEdge(string edgeId)
        {
            return MakeEdge(_graph.EdgeById(edgeId));
        }

        public IEnumerable<Edge> GetEdgesForNode(Node node)
        {
            return node.Edges.Select(MakeEdge);
        }

        public Edge FirstEdgeOf(Node node)
        {
            var graphEdge = node.Edges.First();
            return MakeEdge(graphEdge);
        }

        private Edge MakeEdge(Microsoft.Msagl.Drawing.Edge graphEdge)
        {
            return new Edge(
                graphEdge.Attr.Id,
                graphEdge.Source,
                graphEdge.Target,
                graphEdge.Label != null ? graphEdge.LabelText : ""
            );
        }
    }
}