using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public class NodeItem
    {
        public readonly Node Node;
        public NodeItem(Node n)
        {
            Node = n;
        }

        public string NodeId => Node.NodeId;

        public override string ToString()
        {
            return Node.Title;
        }
    }
}