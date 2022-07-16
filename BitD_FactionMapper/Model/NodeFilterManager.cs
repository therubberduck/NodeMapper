using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BitD_FactionMapper.Model
{
    public class NodeFilterManager
    {
        private static NodeFilterManager _instance;
        private static readonly object Padlock = new object();
        public static NodeFilterManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new NodeFilterManager());
                }
            }
        }

        private int _degreesOfSeparation = 100;

        public List<Node> FilterNodes(List<Node> nodes, Node selectedNode)
        {
            if (_degreesOfSeparation == -1)
            {
                return nodes;
            }
            else
            {
                return FilterNodesByDegreesOfSeparation(nodes, selectedNode, 1);                
            }
            
        }

        public List<Node> FilterNodesByDegreesOfSeparation(List<Node> candidateNodes, Node activeNode, int iteration, List<Node> collector = null)
        {
            if (collector == null)
            {
                collector = new List<Node>();
                collector.Add(activeNode);
            }

            var neighbors = activeNode.Neighbors;
            neighbors = neighbors.Intersect(candidateNodes).ToList();
            collector.AddRange(neighbors);
            foreach (var neighbor in neighbors)
            {
                candidateNodes.Remove(neighbor);
            }
            if (iteration < _degreesOfSeparation)
            {
                foreach (var neighbor in neighbors)
                {
                    FilterNodesByDegreesOfSeparation(candidateNodes, neighbor, iteration + 1, collector);                    
                }
            }

            return collector;
        }
        
        public bool FilterDegreesOfSeparation(int degrees)
        {
            if (_degreesOfSeparation != degrees)
            {
                _degreesOfSeparation = degrees;
                return true;
            }

            return false;
        }
    }
}