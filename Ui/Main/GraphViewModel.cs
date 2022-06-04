using System;
using System.Linq;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using NodeMapper.DataRepository;

namespace NodeMapper.Ui.Main
{
    public class GraphViewModel
    {
        private readonly Random _rand = new Random();
        private DbRepository _repo = new DbRepository();
        
        public readonly Graph Graph;
        public Node SelectedNode; 
        
        public GraphViewModel()
        {
            Graph = _repo.LoadGraph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            Graph.LayoutAlgorithmSettings = settings;
            Graph.Attr.LayerDirection = LayerDirection.None;

            if (Graph.Nodes.Any())
            {
                SelectedNode = Graph.Nodes.First();
            }
            else
            {
                var nodeVillage = Graph.AddNode("Village");
                var nodeHill = Graph.AddNode("Hill");
                var nodeForest = Graph.AddNode("Forest");

                nodeVillage.UserData = "This is an isolated village.";
                nodeHill.UserData = "A ruined table lies at the top this hill.";
                nodeForest.UserData = "Rumors of a werewolf are connected to this forest.";

                Graph.AddEdge(nodeVillage.Id, "4h", nodeHill.Id);
                Graph.AddEdge(nodeVillage.Id, "4h", nodeForest.Id);
                Graph.AddEdge(nodeForest.Id, "2h", nodeHill.Id);
                Graph.AddEdge(nodeHill.Id, "2h", nodeForest.Id);
                
                SelectedNode = Graph.Nodes.First();
            }
        }
        public Node CreateNewEdge(Node selectedNode)
        {
            var newEdge = Graph.AddEdge(selectedNode.Id, "New Node");
            return newEdge.TargetNode;
        }

        public Node AddNode(string name)
        {
            var node = new Node(name);
            node.Attr.Shape = Shape.Box;
            node.Attr.FillColor = Color.Transparent;
            node.Label.FontColor = Color.Black;
            node.Label.FontSize = 6;
            node.Label.Text = name;
            Graph.AddNode(node);
            return node;
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(Graph);
        }

        public bool SelectNode(Point point)
        {
            foreach (var graphNode in Graph.Nodes)
            {
                if (graphNode.BoundingBox.Contains(point))
                {
                    SelectedNode = graphNode;
                    return true;
                }
            }

            return false;
        }

        public void RemoveEdge(Edge edgeToRemove)
        {
            Graph.RemoveEdge(edgeToRemove);
        }
    }
}