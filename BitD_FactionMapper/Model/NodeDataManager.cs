using System.Collections.Generic;
using System.Linq;
using BitD_FactionMapper.DataRepository;

namespace BitD_FactionMapper.Model
{
    public class NodeDataManager
    {
        private static NodeDataManager _instance;
        private static readonly object Padlock = new object();
        public static NodeDataManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new NodeDataManager());
                }
            }
        }
        
        public delegate void EdgeSelectionDelegate();
        public EdgeSelectionDelegate EdgeSelected;
        public delegate void NodeSelectionDelegate();
        public NodeSelectionDelegate NodeSelected;

        private readonly MsaglGraphProvider _graphProvider = MsaglGraphProvider.Instance;
        private readonly DbRepository _repo = new DbRepository();

        private readonly List<int> _nodeIds = new List<int>();
        private List<Node> _nodes;
        public IEnumerable<Node> AllNodes =>_nodes;
        public int NodeCount => _nodes.Count;

        private readonly List<int> _edgeIds = new List<int>();
        private List<Edge> _edges;
        public IEnumerable<Edge> Edges => _edges;

        public Node PreviousSelectedNode { get; private set; }
        private Node _selectedNode;
        public Node SelectedNode
        {
            get => _selectedNode;
            set
            {
                PreviousSelectedNode = _selectedNode;
                _selectedNode = value;
                if (_selectedNode != null)
                {
                    // Only keep SelectedEdge if either end connects to the newly selected node
                    if (SelectedEdge != null && SelectedEdge.SourceNode != value && SelectedEdge.TargetNode != value)
                    {
                        SelectedEdge = null;
                    }
                }
                else
                {
                    SelectedEdge = null;
                }

                NodeSelected();
            }
        }

        private Edge _selectedEdge;
        public Edge SelectedEdge
        {
            get => _selectedEdge;
            set
            {
                _selectedEdge = value;

                if (_selectedEdge != null)
                {
                    // If SelectedNode is not at either end of this edge, change the SelectedNode
                    if (_selectedNode.NodeId != value.SourceId &&
                        _selectedNode.NodeId != value.TargetId)
                    {
                        SelectedNode = value.SourceNode;
                    }
                }
                EdgeSelected();
            }
        }

        public void LoadData()
        {
            _nodes = _repo.LoadNodes();
            foreach (var node in _nodes)
            {
                _nodeIds.Add(int.Parse(node.NodeId));                
            }
            if (!_nodes.Any())
            {
                CreateNodes();
            }
            
            _edges = _repo.LoadEdges().ToList();
            foreach (var edge in _edges)
            {
                _edgeIds.Add(int.Parse(edge.EdgeId));
            }
            if (!_edges.Any())
            {
                CreateEdges();
            }

            SelectedNode = _nodes.First();
        }

        private void CreateNodes()
        {
            CreateNode("Village", "This is an isolated village.");
            CreateNode("Hill", "A ruined stable lies at the top this hill.");
            CreateNode("Forest", "Rumors of a werewolf are connected to this forest.");
        }

        private void CreateEdges()
        {
            _edges = new List<Edge>();
            string lastNodeId = null, secondLastNodeId = null;
            foreach (var node in _nodes)
            {
                if (lastNodeId != null)
                {
                    _edges.Add(new Edge(GetNextEdgeId().ToString(), lastNodeId, node.NodeId, "4h", Edge.Relationship.Neutral));
                    if (secondLastNodeId != null)
                    {
                        _edges.Add(new Edge(GetNextEdgeId().ToString(), lastNodeId, secondLastNodeId, "3h", Edge.Relationship.Neutral));
                    }
                    secondLastNodeId = lastNodeId;
                }
                lastNodeId = node.NodeId;
            }

            _graphProvider.ReloadGraph?.Invoke();
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(_nodes, _edges);
        }

        public Node CreateNewNodeWithEdgeFrom(string nodeId)
        {
            var newNode = CreateNode("New Node");

            AddEdge(nodeId, "", newNode.NodeId, Edge.Relationship.Neutral);

            _graphProvider.ReloadGraph();
            return newNode;
        }

        public Node CreateNode(string title, string body = "")
        {
            var newNode = new Node(GetNextNodeId().ToString(), title, body);
            _nodes.Add(newNode);
            return newNode;
        }

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
            _nodeIds.Remove(int.Parse(node.NodeId));

            _graphProvider.ReloadGraph();
        }

        public Node GetNode(string nodeId)
        {
            return _nodes.Find(node => node.NodeId == nodeId);
        }

        public Node GetNeighborNode(Node node)
        {
            if (!node.Edges.Any()) return null;

            var firstEdge = node.Edges.First();
            // ReSharper disable once PossibleUnintendedReferenceComparison
            return firstEdge.SourceNode == node ? firstEdge.TargetNode : firstEdge.SourceNode;
        }

        public Edge AddEdge(string nodeFromId, string text, string nodeToId, Edge.Relationship relationship)
        {
            var newEdge = new Edge(GetNextEdgeId().ToString(), nodeFromId, nodeToId, text, relationship);
            
            _edges.Add(newEdge);

            _graphProvider.ReloadGraph();
            return newEdge;
        }

        public void RemoveEdge(string edgeId)
        {
            _edges.RemoveAll( e => e.EdgeId == edgeId);
            _edgeIds.Remove(int.Parse(edgeId));

            _graphProvider.ReloadGraph();
        }

        public Edge GetEdge(string edgeId)
        {
            return _edges.Find(edge => edge.EdgeId == edgeId);
        }

        public IEnumerable<Edge> GetEdgesForNode(string nodeId)
        {
            return _edges.FindAll(edge => Equals(edge.SourceNode.NodeId, nodeId) || Equals(edge.TargetNode.NodeId, nodeId));
        }

        public Edge FirstEdgeOf(Node node)
        {
            return _edges.Find(edge => Equals(edge.SourceNode, node) || Equals(edge.TargetNode, node));
        }

        private int GetNextNodeId()
        {
            if (_nodeIds.Count == 0)
            {
                _nodeIds.Add(1);
                return 1;
            }
            var nextId = _nodeIds.Max() + 1; 
            _nodeIds.Add(nextId);
            return nextId;
        }

        private int GetNextEdgeId()
        {
            if (_edgeIds.Count == 0)
            {
                _edgeIds.Add(1);
                return 1;
            }

            var nextId = _edgeIds.Max() + 1; 
            _edgeIds.Add(nextId);
            return nextId;
        }
    }
}