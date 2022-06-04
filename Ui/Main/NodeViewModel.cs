using System.Collections.Generic;
using System.Linq;
using Microsoft.Msagl.Drawing;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public class NodeViewModel
    {
        private static NodeViewModel _instance;
        public static NodeViewModel Instance
        {
            get
            {
                var model = _instance;
                if (model != null)
                {
                    return model;
                }

                _instance = new NodeViewModel();
                _instance.OnNodeSelected += delegate {  };
                _instance.OnEdgeSelected += delegate {  };
                _instance.OnEdgeDeselected += delegate {  };
                _instance.UpdateGraph += delegate {  };

                return _instance;
            }
        }

        private GraphProvider _graphProvider = GraphProvider.Instance;
        
        private Node _selectedNode;
        private Edge _selectedEdge;
        
        public delegate void NodeSelectedDelegate(Node node);
        public NodeSelectedDelegate OnNodeSelected;
        
        public delegate void EdgeSelectedDelegate(Edge edge);
        public EdgeSelectedDelegate OnEdgeSelected;
        
        public delegate void EdgeDeselectedDelegate();
        public EdgeDeselectedDelegate OnEdgeDeselected;
        
        public delegate void UpdateGraphDelegate();
        public UpdateGraphDelegate UpdateGraph;

        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                OnNodeSelected(value);
            }
        }

        public Edge SelectedEdge
        {
            get => _selectedEdge;
            set
            {
                if (_selectedEdge != null)
                {
                    _selectedEdge.Attr.Color = Color.Black;
                }
                _selectedEdge = value;
                if (_selectedEdge != null)
                {
                    OnEdgeSelected(value);
                    _selectedEdge.Attr.Color = Color.DarkGray;
                }
                else
                {
                    OnEdgeDeselected();
                }

                UpdateGraph();
            }
        }

        public string NodeName => SelectedNode != null ? SelectedNode.LabelText : "No Node Selected";

        public string NodeDescription => SelectedNode != null ? (SelectedNode.UserData as string) : "";

        public IEnumerable<EdgeItem> EdgeItems => SelectedNode != null
            ? SelectedNode.Edges.Select(e => new EdgeItem(e))
            : Enumerable.Empty<EdgeItem>();
        
        public void Init()
        {
            SelectedNode = _graphProvider.Graph.Nodes.First();
        }
    }
}