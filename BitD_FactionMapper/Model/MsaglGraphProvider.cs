using System;
using System.Linq;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Incremental;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;

namespace BitD_FactionMapper.Model
{
    public class MsaglGraphProvider
    {
        private static MsaglGraphProvider _instance;
        public static MsaglGraphProvider Instance => _instance ?? (_instance = new MsaglGraphProvider());

        private NodeDataManager _nodeDataManager;

        public delegate void ReloadGraphDelegate();

        public ReloadGraphDelegate ReloadGraph;

        public delegate void InvalidateGraphDelegate();

        public InvalidateGraphDelegate InvalidateGraph;

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
                if (graph.Label != null)
                {
                    graphNode.Label.Text = node.Title;
                }

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
                switch (edge.Relation)
                {
                    case Edge.Relationship.Ally:
                        graphEdge.Attr.Color = Color.DarkGreen;
                        if (graphEdge.Label != null)
                        {
                            graphEdge.Label.FontColor = Color.DarkGreen;
                        }
                        break;
                    case Edge.Relationship.Friend:
                        graphEdge.Attr.Color = Color.LightGreen;
                        if (graphEdge.Label != null)
                        {
                            graphEdge.Label.FontColor = Color.LightGreen;
                        }
                        break;
                    case Edge.Relationship.Enemy:
                        graphEdge.Attr.Color = Color.Orange;
                        if (graphEdge.Label != null)
                        {
                            graphEdge.Label.FontColor = Color.Orange;
                        }
                        break;
                    case Edge.Relationship.War:
                        graphEdge.Attr.Color = Color.Red;
                        if (graphEdge.Label != null)
                        {
                            graphEdge.Label.FontColor = Color.Red;
                        }
                        break;
                    default:
                        graphEdge.Attr.Color = Color.Black;
                        if (graphEdge.Label != null)
                        {
                            graphEdge.Label.FontColor = Color.Black;
                        }
                        break;
                }
            }

            _graph = graph;

            GetUpdatedGraph();

            return graph;
        }

        
        public Graph GetUpdatedGraph()
        {
            if(_graph == null) return GetNewGraph();
            
            foreach (var node in _graph.Nodes)
            {
                node.Attr.Color = Color.Black;
            }
            
            SelectedNode().Attr.Color = Color.Red;

            foreach (var edge in _graph.Edges)
            {
                if (edge.Label != null)
                {
                    edge.Label.FontColor = Color.Black;
                }
            }

            if (_nodeDataManager.SelectedEdge != null && SelectedEdge().Label != null)
            {
                SelectedEdge().Label.FontColor = Color.Red;                
            }

            return _graph;
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

        private Microsoft.Msagl.Drawing.Edge SelectedEdge()
        {
            return FindEdge(_nodeDataManager.SelectedEdge.EdgeId.ToString());
        }

        private Microsoft.Msagl.Drawing.Node SelectedNode()
        {
            return FindNode(_nodeDataManager.SelectedNode.NodeId.ToString());
        }

        public Graph Randomize()
        {
            _seed = _rand.Next();
            return GetNewGraph();
        }
    }
}