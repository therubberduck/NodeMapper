using NodeMapper.Model;

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
            return Edge.SourceNode.LabelText + " to " + Edge.TargetNode.LabelText;
        }
    }
}