using System.Collections.Generic;
using System.Linq;
using BitD_FactionMapper.Model;
using Microsoft.Msagl.Drawing;
using Edge = BitD_FactionMapper.Model.Edge;
using Node = BitD_FactionMapper.Model.Node;

namespace BitD_FactionMapper.Ui.Main
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

        private readonly GraphManager _graphManager = GraphManager.Instance;
        
        private Node _selectedNode;
        private Edge _selectedEdge;
        
        public delegate void NodeSelectedDelegate(Node node);
        public NodeSelectedDelegate OnNodeSelected;
        
        public delegate void EdgeSelectedDelegate(Edge edge);
        public EdgeSelectedDelegate OnEdgeSelected;
        
        public delegate void EdgeDeselectedDelegate();
        public EdgeDeselectedDelegate OnEdgeDeselected;
        
        public delegate void UpdateNodeDetailsDelegate();
        public UpdateNodeDetailsDelegate UpdateNodeDetails;
        
        public delegate void UpdateGraphDelegate();
        public UpdateGraphDelegate UpdateGraph;

        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode != null)
                {
                    _selectedNode.FillColor = Color.White;
                }
                _selectedNode = value;
                if (_selectedNode != null)
                {
                    _selectedNode.FillColor = Color.Aquamarine;
                    
                    if (_selectedEdge != null && _selectedEdge.SourceNode != value && _selectedEdge.TargetNode != value)
                    {
                        SelectedEdge = null;
                    }
                }
                else
                {
                    SelectedEdge = null;
                }
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
                    //_selectedEdge.Attr.Color = Color.Black;
                }
                _selectedEdge = value;
                if (_selectedEdge != null)
                {
                    OnEdgeSelected(value);
                    //_selectedEdge.Attr.Color = Color.DarkGray;
                }
                else
                {
                    OnEdgeDeselected();
                }

                UpdateGraph();
            }
        }

        public string NodeName => SelectedNode != null ? SelectedNode.Title : "No Node Selected";

        public string NodeDescription => SelectedNode != null ? SelectedNode.Body : "";

        public IEnumerable<EdgeItem> EdgeItems => SelectedNode != null
            ? _graphManager.GetEdgesForNode(SelectedNode.NodeId).Select(e => new EdgeItem(e))
            : Enumerable.Empty<EdgeItem>();

        public IEnumerable<NodeItem> AllNodeItems => _graphManager.AllNodes.Select(n => new NodeItem(n));

        public void Init()
        {
            SelectedNode = _graphManager.FirstNode;
        }
    }
}