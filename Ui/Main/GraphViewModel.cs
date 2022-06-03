using System;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public class GraphViewModel
    {
        public readonly Graph Graph = new Graph();
        private readonly Random _rand = new Random();
        
        public GraphViewModel()
        {
            Graph.Attr.LayerDirection = LayerDirection.LR;
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