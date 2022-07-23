using System.Collections.Generic;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.DataRepository
{
   public struct NodeEdgesResult
    {
        public readonly List<Node> Nodes;
        public readonly List<Edge> Edges;

        public NodeEdgesResult(List<Node> nodes, List<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }
    }
}