using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Drawing;

namespace GraphMapper.Model
{
    public class NodeStore
    {
        public Point Center { get; }
        public Node Node { get; }

        public NodeStore(Point center, Node node)
        {
            Center = center;
            Node = node;
        }
    }
}