using BitD_FactionMapper.Model;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Edge = Microsoft.Msagl.Drawing.Edge;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class GraphControl
    {
        public delegate void RedrawGraphDelegate();
        public delegate void UpdateGraphDelegate();
        
        private readonly GraphViewer _graphViewer = new GraphViewer();
        private readonly MsaglGraphProvider _provider = MsaglGraphProvider.Instance;

        public GraphControl() {
            InitializeComponent();
            
            _graphViewer.BindToPanel(dockPanel);
            _graphViewer.LayoutEditingEnabled = false;
            
            (_graphViewer as IViewer).MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MsaglMouseEventArgs e)
        {
            var item = _graphViewer.ObjectUnderMouseCursor;
            if (item == null) return;
            
            if (item is VNode node)
            {
                _provider.SelectNode(node.Node.Attr.Id);
                RedrawGraph();
            }
            else if (item is IViewerEdge edge)
            {
                _provider.SelectEdge(edge.Edge.Attr.Id); 
            }
            else if (item.DrawingObject is Label label && label.Owner is Edge labelEdge)
            {
                _provider.SelectEdge(labelEdge.Attr.Id);
            }
        }

        public void RedrawGraph()
        {
            _graphViewer.Graph = _provider.GetNewGraph();
            _graphViewer.Invalidate();
        }

        public void UpdateGraph()
        {
            _graphViewer.Graph = _provider.GetUpdatedGraph();
            _graphViewer.Invalidate();
        }
    }
}