using System;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;

namespace NodeMapper.Ui.Main
{
    public class GraphViewModel
    {
        public readonly Graph Graph = new Graph();
        private readonly Random _rand = new Random();
        
        public GraphViewModel()
        {
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.RectilinearToCenter };
            Graph.LayoutAlgorithmSettings = settings;
            Graph.Attr.LayerDirection = LayerDirection.LR;
            
            Graph.AddEdge("1", "2");
            Graph.AddEdge("2", "3");
            Graph.AddEdge("1", "4");
            Graph.AddEdge("4", "5");
            Graph.AddEdge("5", "2");
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
    }
}