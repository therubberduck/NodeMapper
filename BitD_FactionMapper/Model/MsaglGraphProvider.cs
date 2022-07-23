using System;
using System.Linq;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;

namespace BitD_FactionMapper.Model
{
    public class MsaglGraphProvider
    {
        private static MsaglGraphProvider _instance;
        public static MsaglGraphProvider Instance => _instance ?? (_instance = new MsaglGraphProvider());

        private NodeDataManager _nodeDataManager;

        private Graph _graph;

        private readonly Random _rand = new Random();
        private int _seed = 1;

        public Graph GetNewGraph()
        {
            if (_nodeDataManager == null)
            {
                _nodeDataManager = NodeDataManager.Instance;
            }

            var nodes = _nodeDataManager.FilteredNodes;
            var edges = _nodeDataManager.FilteredEdges(nodes);


            var graph = new Graph();
            // var settings = new SugiyamaLayoutSettings();
            // settings.EdgeRoutingSettings = new EdgeRoutingSettings
            //     { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            // settings.RandomSeedForOrdering = _seed;
            // settings.RepetitionCoefficientForOrdering =  10;
            var settings = new MdsLayoutSettings();
            settings.IterationsWithMajorization = 50;
            settings.AdjustScale = true;
            // var settings = new FastIncrementalLayoutSettings();
            // settings.RungeKuttaIntegration = true;

            graph.LayoutAlgorithmSettings = settings;
            graph.Attr.LayerDirection = LayerDirection.TB;
            graph.Attr.BackgroundColor = Color.Beige;

            foreach (var node in nodes)
            {
                var graphNode = new Microsoft.Msagl.Drawing.Node(node.Title);
                graphNode.Attr.Id = node.NodeId.ToString();

                graphNode.UserData = node.Body;
                graph.AddNode(graphNode);
            }

            foreach (var edge in edges)
            {
                bool twoWay = false;
                // If the target node already has an edge targeting our source
                var targetNode = edge.TargetNode;
                var targetNodeEdges = targetNode.Edges;
                if (targetNodeEdges.Any(e => e.TargetId == edge.SourceId))
                {
                    var competingEdge = edge.TargetNode.Edges.First(e => e.TargetId == edge.SourceId);
                    // If competing edge and our edge are basically identical, just in opposite directions
                    if (competingEdge.LabelText == edge.LabelText && competingEdge.Relation == edge.Relation)
                    {
                        if (!graph.Edges.Any(e => e.Attr.Id == competingEdge.EdgeId.ToString()))
                        {
                            // If we have not already add one edge, make it two-way
                            twoWay = true;
                        }
                        else
                        {
                            // If we have already added one edge, skip making this edge
                            continue;
                        }
                    }
                }

                var graphEdge = graph.AddEdge(edge.SourceId.ToString(), edge.LabelText, edge.TargetId.ToString());
                graphEdge.Attr.Id = edge.EdgeId.ToString();
                if (twoWay)
                {
                    graphEdge.Attr.ArrowheadAtSource = ArrowStyle.Normal;
                }
            }

            _graph = graph;

            GetUpdatedGraph();

            return graph;
        }


        public Graph GetUpdatedGraph()
        {
            if (_graph == null) return GetNewGraph();

            foreach (var graphNode in _graph.Nodes)
            {
                UpdateGraphNode(int.Parse(graphNode.Attr.Id), graphNode);
            }

            foreach (var graphEdge in _graph.Edges)
            {
                UpdateGraphEdge(int.Parse(graphEdge.Attr.Id), graphEdge);
            }

            return _graph;
        }

        public Graph GetUpdatedGraphEdge(int edgeId)
        {
            UpdateGraphEdge(edgeId, FindEdge(edgeId.ToString()));
            return _graph;
        }

        public Graph GetUpdatedGraphNode(int nodeId)
        {
            UpdateGraphNode(nodeId, FindNode(nodeId.ToString()));
            return _graph;
        }

        private void UpdateGraphEdge(int edgeId, Microsoft.Msagl.Drawing.Edge graphEdge)
        {
            var edge = _nodeDataManager.GetEdge(edgeId);
            
            graphEdge.Attr.LineWidth = EdgeIsSelected(edgeId) ? 3 : 1;
            
            if (graphEdge.Label == null)
            {
                graphEdge.Label = new Label();
            }
            graphEdge.LabelText = edge.LabelText;       

            switch (edge.Relation)
            {
                case Edge.Relationship.Ally:
                    graphEdge.Attr.Color = Color.DarkGreen;
                    graphEdge.Label.FontColor = Color.DarkGreen;
                    break;
                case Edge.Relationship.Friend:
                    graphEdge.Attr.Color = Color.LightGreen;
                    graphEdge.Label.FontColor = Color.LightGreen;
                    break;
                case Edge.Relationship.Enemy:
                    graphEdge.Attr.Color = Color.Orange;
                    graphEdge.Label.FontColor = Color.Orange;
                    break;
                case Edge.Relationship.War:
                    graphEdge.Attr.Color = Color.Red;
                    graphEdge.Label.FontColor = Color.Red;
                    break;
                default:
                    graphEdge.Attr.Color = Color.Black;
                    graphEdge.Label.FontColor = Color.Black;
                    break;
            }
        }

        private void UpdateGraphNode(int nodeId, Microsoft.Msagl.Drawing.Node graphNode)
        {
            var node = _nodeDataManager.GetNode(nodeId);

            graphNode.Attr.Color = NodeIsSelected(nodeId) ? Color.Red : Color.Black;
            
            graphNode.Label.Text = node.Title;
            
            switch (node.FactionType)
            {
                case FactionType.Fringe:
                    graphNode.Attr.FillColor = Color.Purple;
                    graphNode.Label.FontColor = Color.White;
                    break;
                case FactionType.Institution:
                    graphNode.Attr.FillColor = Color.LightBlue;
                    graphNode.Label.FontColor = Color.Black;
                    break;
                case FactionType.Labor:
                    graphNode.Attr.FillColor = Color.Salmon;
                    graphNode.Label.FontColor = Color.Black;
                    break;
                case FactionType.Underworld:
                    graphNode.Attr.FillColor = Color.Black;
                    graphNode.Label.FontColor = Color.White;
                    break;
                case FactionType.Other:
                    graphNode.Attr.FillColor = Color.White;
                    graphNode.Label.FontColor = Color.Black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SelectNode(string nodeId)
        {
            var node = _nodeDataManager.GetNode(int.Parse(nodeId));
            _nodeDataManager.SelectedNode = node;
        }

        public void SelectEdge(string edgeId)
        {
            var edge = _nodeDataManager.GetEdge(int.Parse(edgeId));
            _nodeDataManager.SelectedEdge = edge;
        }

        private Microsoft.Msagl.Drawing.Edge FindEdge(string edgeId)
        {
            try
            {
                return _graph.Edges.First(e => e.Attr.Id == edgeId);
            }
            catch (InvalidOperationException)
            {
                GetNewGraph();
                return _graph.Edges.First(e => e.Attr.Id == edgeId);
            }
        }

        private Microsoft.Msagl.Drawing.Node FindNode(string nodeId)
        {
            try
            {
                return _graph.Nodes.First(n => n.Attr.Id == nodeId);
            }
            catch (InvalidOperationException)
            {
                GetNewGraph();
                return _graph.Nodes.First(n => n.Attr.Id == nodeId);
            }
        }

        private bool EdgeIsSelected(int edgeId)
        {
            return edgeId.ToString() == SelectedEdge()?.Attr?.Id;
        }
        
        private int _selectedEdgeId = -1;
        private Microsoft.Msagl.Drawing.Edge _selectedEdge;
        private Microsoft.Msagl.Drawing.Edge SelectedEdge()
        {
            if (_nodeDataManager.SelectedEdge == null)
            {
                return null;
            }
            
            var modelSelectedEdgeId = _nodeDataManager.SelectedEdge.EdgeId;
            if (_selectedEdgeId != modelSelectedEdgeId)
            {
                _selectedEdge = FindEdge(modelSelectedEdgeId.ToString());
                _selectedEdgeId = modelSelectedEdgeId;
            }

            return _selectedEdge;
        }

        private bool NodeIsSelected(int nodedId)
        {
            return nodedId.ToString() == SelectedNode()?.Attr?.Id;
        }

        private int _selectedNodeId = -1;
        private Microsoft.Msagl.Drawing.Node _selectedNode;
        private Microsoft.Msagl.Drawing.Node SelectedNode()
        {
            if (_nodeDataManager.SelectedNode == null)
            {
                return null;
            }
            
            var modelSelectedNodeId = _nodeDataManager.SelectedNode.NodeId;
            if (_selectedNodeId != modelSelectedNodeId)
            {
                _selectedNode = FindNode(modelSelectedNodeId.ToString());
                _selectedNodeId = modelSelectedNodeId;
            }

            return _selectedNode;
        }

        public Graph Randomize()
        {
            _seed = _rand.Next();
            return GetNewGraph();
        }
    }
}