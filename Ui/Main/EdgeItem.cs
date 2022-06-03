using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public class EdgeItem
    {
        public readonly Edge Edge;
        public EdgeItem(Edge e)
        {
            Edge = e;
        }

        public override string ToString()
        {
            return Edge.Source + " to " + Edge.Target;
        }
    }
}