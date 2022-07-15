﻿using System;
using System.Linq;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;

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

        public Graph GetGraph()
        {
            return _graph ?? GetNewGraph();
        }

        public Graph GetNewGraph()
        {
            if (_nodeDataManager == null)
            {
                _nodeDataManager = NodeDataManager.Instance;
            }

            var graph = new Graph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            graph.LayoutAlgorithmSettings = settings;
            graph.Attr.LayerDirection = LayerDirection.TB;

            foreach (var node in _nodeDataManager.AllNodes)
            {
                var graphNode = new Microsoft.Msagl.Drawing.Node(node.Title);
                graphNode.Attr.Id = node.NodeId.ToString();
                graphNode.Label.Text = node.Title;
                graphNode.UserData = node.Body;
                graph.AddNode(graphNode);
            }

            foreach (var edge in _nodeDataManager.Edges)
            {
                var graphEdge = graph.AddEdge(edge.SourceId.ToString(), edge.LabelText, edge.TargetId.ToString());
                graphEdge.Attr.Id = edge.EdgeId.ToString();
                switch (edge.Relation)
                {
                    case Edge.Relationship.Ally:
                        graphEdge.Attr.Color = Color.DarkGreen;
                        break;
                    case Edge.Relationship.Friend:
                        graphEdge.Attr.Color = Color.LightGreen;
                        break;
                    case Edge.Relationship.Enemy:
                        graphEdge.Attr.Color = Color.Orange;
                        break;
                    case Edge.Relationship.War:
                        graphEdge.Attr.Color = Color.Red;
                        break;
                    default:
                        graphEdge.Attr.Color = Color.Black;
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

            if (_nodeDataManager.SelectedEdge != null)
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
            return _graph.Edges.First(e => e.Attr.Id == edgeId);
        }

        private Microsoft.Msagl.Drawing.Node FindNode(string nodeId)
        {
            return _graph.Nodes.First(n => n.Attr.Id == nodeId);
        }

        private Microsoft.Msagl.Drawing.Edge SelectedEdge()
        {
            return FindEdge(_nodeDataManager.SelectedEdge.EdgeId.ToString());
        }

        private Microsoft.Msagl.Drawing.Node SelectedNode()
        {
            return FindNode(_nodeDataManager.SelectedNode.NodeId.ToString());
        }
    }
}