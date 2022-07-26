﻿using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace BitD_FactionMapper.Model
{
    public class Node
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;

        public string Title { get; set; }
        public string Body { get; set; }
        public FactionType FactionType { get; set; }

        public IEnumerable<Edge> Edges => _nodeDataManager.GetEdgesForNode(NodeId);

        public IEnumerable<Node> Neighbors => _nodeDataManager.GetNeighborsForNode(this);
        public IEnumerable<Node> NeighborsWhereNodeIsSource => _nodeDataManager.GetNeighborsForNode(this, false, true);
        public IEnumerable<Node> NeighborsWhereNodeIsTarget => _nodeDataManager.GetNeighborsForNode(this, true, false);

        public int NodeId { get; }
        public Node(int nodeId, string title, string body, FactionType factionType)
        {
            NodeId = nodeId;
            Title = title;
            Body = body;
            FactionType = factionType;
        }
    }
}