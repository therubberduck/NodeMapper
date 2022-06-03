using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public class NodeViewModel
    {
        private Node _selectedNode;

        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                if (!_selectedNode.Edges.Contains(SelectedEdge))
                {
                    SelectedEdge = _selectedNode.Edges.Any() ? _selectedNode.Edges.First() : null;                    
                }
            }
        }

        public Edge SelectedEdge { get; set; }

        public string NodeDescription => SelectedNode != null ? SelectedNode.LabelText : "None Selected";

        public IEnumerable<EdgeItem> EdgeItems => SelectedNode != null
            ? SelectedNode.Edges.Select(e => new EdgeItem(e))
            : Enumerable.Empty<EdgeItem>();

        public class EdgeItem
        {
            public readonly Edge Edge;
            public EdgeItem(Edge e)
            {
                Edge = e;
            }

            public override string ToString()
            {
                return Edge.Source + " to " + Edge.Target;
            }
        }
    }
}