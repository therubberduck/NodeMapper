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
                
                // Set selected node
                _selectedNode = value;
                
                // Save selected node for restart
                Properties.Settings.Default.SelectedNode = _selectedNode.NodeId;
                Properties.Settings.Default.Save();
                
                // Evaluate selected edge based on the newly selected node
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

                // Activate NodeSelected delegate
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
                RebuildNodeGraph();
            }
            else
            {
                _edges = _repo.LoadEdges().ToList();
            }
            
            FinishLoad();
        }

        public void RebuildNodeGraph()
        {
            var initializer = new GraphInitializer();
            _nodes = initializer.CreateDuskvolFactions();
            _edges = initializer.CreateDuskvolRelations();
        }

        public void Load(string dialogFileNam)
        {
            var result = _repo.Load(dialogFileNam);
            _nodes = result.Nodes;
            _edges = result.Edges;
            FinishLoad();
        }

        private void FinishLoad()
        {
            _nodeIds.AddRange(_nodes.Select(n => n.NodeId));
            _edgeIds.AddRange(_edges.Select(e => e.EdgeId));

            if (Properties.Settings.Default.SelectedNode != -1)
            {
                SelectedNode = GetNode(Properties.Settings.Default.SelectedNode);
            }
            else
            {
                SelectedNode = _nodes.First();                
            }
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(_nodes, _edges);
        }

        public void SaveGraphAs(string dialogFileName)
        {
            _repo.SaveGraph(_nodes, _edges, dialogFileName);
        }

        public Node CreateNewNodeWithEdgeFrom(int nodeId)
        {
            var newNode = CreateNode("New Node");

            AddEdge(nodeId, "", newNode.NodeId, Edge.Relationship.Neutral);

            return newNode;
        }

        public Node CreateNode(string title, string body = "", FactionType factionType = FactionType.Underworld)
        {
            var newNode = new Node(GetNextNodeId(), title, body, factionType);
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

        /// <summary>
        /// Return neighboring nodes (nodes that have edges to the central node).
        /// </summary>
        /// <param name="node">Central node</param>
        /// <param name="isTarget">Find neighbors where the central node is the target of the edge.</param>
        /// <param name="isSource">Find neighbors where the central node is the source of the edge.</param>
        public IEnumerable<Node> GetNeighborsForNode(Node node, bool isTarget = true, bool isSource = true)
        {
            if (!node.Edges.Any() || (!isTarget && !isSource)) return new List<Node>();

            var neighbors = node.Edges.Select(e =>
            {
                Node neighborNode = null;
                if (isSource && e.SourceId == node.NodeId)
                {
                    neighborNode = e.TargetNode;
                }
                else if(isTarget)
                {
                    neighborNode = e.SourceNode;
                }

                return neighborNode;
            }).Where(e => e != null);

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