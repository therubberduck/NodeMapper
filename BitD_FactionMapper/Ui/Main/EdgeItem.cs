using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
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
            return Edge.SourceNode.Title + " to " + Edge.TargetNode.Title;
        }
    }
}