using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public class NodeItem
    {
        public readonly Node Node;
        public NodeItem(Node n)
        {
            Node = n;
        }

        public override string ToString()
        {
            return Node.LabelText;
        }
    }
}