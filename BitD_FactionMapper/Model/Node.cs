using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace BitD_FactionMapper.Model
{
    public class Node
    {
        private readonly GraphManager _graphManager = GraphManager.Instance;

        public string Title { get; set; }
        public string Body { get; set; }

        public IEnumerable<Edge> Edges => _graphManager.GetEdgesForNode(NodeId);

        public string NodeId { get; }
        public Node(string nodeId, string title, string body)
        {
            NodeId = nodeId;
            Title = title;
            Body = body;
        }
    }
}