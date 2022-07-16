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
        
        private readonly NodeFilterManager _nodeFilterManager = NodeFilterManager.Instance;
        
        public delegate void EdgeSelectionDelegate();
        public EdgeSelectionDelegate EdgeSelected;
        public delegate void NodeSelectionDelegate();
        public NodeSelectionDelegate NodeSelected;

        private readonly DbRepository _repo = new DbRepository();

        private readonly List<int> _nodeIds = new List<int>();
        private List<Node> _nodes;
        public IEnumerable<Node> AllNodes =>_nodes;
        public int NodeCount => _nodes.Count;

        private readonly List<int> _edgeIds = new List<int>();
        private List<Edge> _edges;
        public IEnumerable<Edge> Edges => _edges;

        public List<Node> FilteredNodes => _nodeFilterManager.FilterNodes(_nodes.ToList(), _selectedNode);

        public IEnumerable<Edge> FilteredEdges(IEnumerable<Node> filteredNodes)
        {
            var filteredNodeIds = filteredNodes.Select(n => n.NodeId);
            return _edges.FindAll(e => filteredNodeIds.Contains(e.SourceId) && filteredNodeIds.Contains(e.TargetId));
        }

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
            
            if (!_nodes.Any())
            {
                CreateNodes();
            }
            else
            {
                _edges = _repo.LoadEdges().ToList();
            }
            
            _nodeIds.AddRange(_nodes.Select(n => n.NodeId));
            _edgeIds.AddRange(_edges.Select(e => e.EdgeId));
            
            SelectedNode = _nodes.First();
        }

        private void CreateNodes()
        {
            var initializer = new GraphInitializer();
            _nodes = initializer.CreateDuskvolFactions();
            _edges = initializer.CreateDuskvolRelations();
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(_nodes, _edges);
        }

        public Node CreateNewNodeWithEdgeFrom(int nodeId)
        {
            var newNode = CreateNode("New Node");

            AddEdge(nodeId, "", newNode.NodeId, Edge.Relationship.Neutral);

            return newNode;
        }

        public Node CreateNode(string title, string body = "")
        {
            var newNode = new Node(GetNextNodeId(), title, body);
            _nodes.Add(newNode);
            return newNode;
        }

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
            _nodeIds.Remove(node.NodeId);
        }

        public Node GetNode(int nodeId)
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

        public IEnumerable<Node> GetNeighborsForNode(Node node)
        {
            if (!node.Edges.Any()) return new List<Node>();

            var neighbors = node.Edges.Select(e =>
            {
                if (e.SourceId == node.NodeId)
                {
                    return e.TargetNode;
                }
                else
                {
                    return e.SourceNode;
                }
            });

            return neighbors;
        }

        public Edge AddEdge(int nodeFromId, string text, int nodeToId, Edge.Relationship relationship)
        {
            var newEdge = new Edge(GetNextEdgeId(), nodeFromId, nodeToId, text, relationship);
            
            _edges.Add(newEdge);
            return newEdge;
        }

        public void RemoveEdge(int edgeId)
        {
            _edges.RemoveAll( e => e.EdgeId == edgeId);
            _edgeIds.Remove(edgeId);
        }

        public Edge GetEdge(int edgeId)
        {
            return _edges.Find(edge => edge.EdgeId == edgeId);
        }

        public IEnumerable<Edge> GetEdgesForNode(int nodeId)
        {
            return _edges.FindAll(edge => Equals(edge.SourceId, nodeId) || Equals(edge.TargetId, nodeId));
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