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
            Graph.Attr.LayerDirection = LayerDirection.LR;

            if (Graph.Nodes.Any())
            {
                SelectedNode = Graph.Nodes.First();
            }
            else
            {
                Graph.AddEdge("1", "2");
                Graph.AddEdge("2", "3");
                Graph.AddEdge("3", "1");
                Graph.AddEdge("3", "2");
                SelectedNode = Graph.Nodes.First();
            }
        }
        public void CreateNewEdge()
        {
            var node1 = _rand.Next(1,11).ToString();
            var node2 = _rand.Next(1,11).ToString();
            Graph.AddEdge(node1, node2);
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