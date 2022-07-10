using System.Collections.Generic;
using System.Linq;
using NodeMapper.DataRepository;
using DrawingNode = Microsoft.Msagl.Drawing.Node;

namespace NodeMapper.Model
{
    public class GraphManager
    {
        private static GraphManager _instance;
        private static readonly object Padlock = new object();
        public static GraphManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new GraphManager());
                }
            }
        }

        private readonly MsaglGraphProvider _graphProvider = MsaglGraphProvider.Instance;
        private readonly DbRepository _repo = new DbRepository();

        private readonly List<int> _nodeIds = new List<int>();
        private readonly List<DrawingNode> _nodes;
        public IEnumerable<Node> AllNodes =>_nodes;
        public Node FirstNode => _nodes.First();
        public int NodeCount => _nodes.Count;

        private readonly List<int> _edgeIds = new List<int>();
        private List<Edge> _edges;
        public IEnumerable<Edge> Edges => _edges;

        public GraphManager()
        {
            _nodes = _repo.LoadNodes();
            foreach (var node in _nodes)
            {
                _nodeIds.Add(int.Parse(node.Id));                
            }
            
            _edges = _repo.LoadEdges().ToList();
            foreach (var edge in _edges)
            {
                _edgeIds.Add(int.Parse(edge.EdgeId));
            }
            
            if (!_nodes.Any())
            {
                CreateNodes();
            }
        }

        public void InitEdges()
        {
            if (!_edges.Any())
            {
                CreateEdges();
            }
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
            Node lastNode = null, secondLastNode = null;
            foreach (var graphNode in _nodes)
            {
                if (lastNode != null)
                {
                    _edges.Add(new Edge(GetNextEdgeId().ToString(), lastNode.Id, graphNode.Id, "4h"));
                    if (secondLastNode != null)
                    {
                        _edges.Add(new Edge(GetNextEdgeId().ToString(), lastNode.Id, secondLastNode.Id, "3h"));
                    }
                    secondLastNode = lastNode;
                }
                lastNode = graphNode;
            }

            _graphProvider.ReloadGraph();
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(_nodes, _edges);
        }

        public Node CreateNewNodeWithEdgeFrom(Node node)
        {
            var newNode = CreateNode("New Node");

            AddEdge(node.Id, "", newNode.Id);

            _graphProvider.ReloadGraph();
            return newNode;
        }

        public Node CreateNode(string title, string body = "")
        {
            var newNode = new Node(GetNextNodeId().ToString());
            newNode.LabelText = title;
            newNode.Label.UserData = newNode.Attr.Id;
            newNode.UserData = body;
            _nodes.Add(newNode);
            return newNode;
        }

        public void RemoveNode(Node node)
        {
            _nodes.Remove(node);
            _nodeIds.Remove(int.Parse(node.Id));

            _graphProvider.ReloadGraph();
        }

        public Node GetNode(string nodeId)
        {
            return _nodes.Find(node => node.Id == nodeId);
        }

        public Node GetNeighborNode(Node node)
        {
            if (!node.Edges.Any()) return null;

            var firstEdge = node.Edges.First();
            // ReSharper disable once PossibleUnintendedReferenceComparison
            return firstEdge.SourceNode == node ? firstEdge.TargetNode : firstEdge.SourceNode;
        }

        public Edge AddEdge(string nodeFromId, string text, string nodeToId)
        {
            var newEdge = new Edge(GetNextEdgeId().ToString(), nodeFromId, nodeToId, text);
            
            _edges.Add(newEdge);

            _graphProvider.ReloadGraph();
            return newEdge;
        }

        public void RemoveEdge(Edge edge)
        {
            _edges.Remove(edge);
            _edgeIds.Remove(int.Parse(edge.EdgeId));

            _graphProvider.ReloadGraph();
        }

        public Edge GetEdge(string edgeId)
        {
            return _edges.Find(edge => edge.EdgeId == edgeId);
        }

        public IEnumerable<Edge> GetEdgesForNode(Node node)
        {
            return _edges.FindAll(edge => Equals(edge.SourceNode, node) || Equals(edge.TargetNode, node));
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