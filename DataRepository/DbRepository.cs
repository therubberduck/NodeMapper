using Microsoft.Msagl.Drawing;

namespace NodeMapper.DataRepository
{
    public class DbRepository
    {
        private DbInterface _db;

        public DbRepository()
        {
            _db = DbInterface.GetDevInterface();
        }

        public Graph LoadGraph()
        {
            var edges = _db.Edge.GetAll();
            var nodes = _db.Node.GetAll();

            var graph = new Graph();
            foreach (var nodeStore in nodes)
            {
                graph.AddNode(nodeStore.Node);
                //nodeStore.Node.GeometryNode.Center = nodeStore.Center;
            }

            foreach (var edge in edges)
            {
                var newEdge = graph.AddEdge(edge.Source, edge.LabelText, edge.Target);
                newEdge.Attr.ArrowheadAtSource = edge.Attr.ArrowheadAtSource;
                newEdge.Attr.ArrowheadAtTarget = edge.Attr.ArrowheadAtTarget;
                newEdge.Attr.Color = edge.Attr.Color;
                newEdge.Attr.ArrowheadAtSource = edge.Attr.ArrowheadAtSource;
                newEdge.Attr.ArrowheadAtTarget = edge.Attr.ArrowheadAtTarget;
                if (newEdge.Label != null)
                {
                    newEdge.Label.FontColor = edge.Label.FontColor;
                    newEdge.Label.FontSize = edge.Label.FontSize;
                }
            }

            return graph;
        }

        public void SaveGraph(Graph graph)
        {
            _db.Node.ClearTable();
            _db.Edge.ClearTable();
            _db.Node.InsertAll(graph.Nodes);
            _db.Edge.InsertAll(graph.Edges);
        }
    }
}
