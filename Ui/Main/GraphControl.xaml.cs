using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using NodeMapper.Model;
using Edge = Microsoft.Msagl.Drawing.Edge;

namespace NodeMapper.Ui.Main
{
    public partial class GraphControl
    {
        public delegate void NodeSelectionDelegate(Node node);
        public NodeSelectionDelegate NodeSelection;
        public delegate void EdgeSelectionDelegate(string edgeId);
        public EdgeSelectionDelegate EdgeSelection;
        
        private readonly GraphViewer _graphViewer = new GraphViewer();
        private readonly MsaglGraphProvider _provider = MsaglGraphProvider.Instance;

        public GraphControl() {
            InitializeComponent();
            
            _graphViewer.BindToPanel(dockPanel);
            _graphViewer.LayoutEditingEnabled = false;

            _provider.ReloadGraph += Reload;
            _provider.InvalidateGraph += Invalidate;
            
            (_graphViewer as IViewer).MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MsaglMouseEventArgs e)
        {
            var item = _graphViewer.ObjectUnderMouseCursor;
            if (item is VNode node)
            {
                NodeSelection?.Invoke(node.Node);
            }
            else if (item is IViewerEdge edge)
            {
                EdgeSelection?.Invoke(edge.Edge.Attr.Id);   
            }
            else if (item.DrawingObject is Label label && label.Owner is Edge labelEdge)
            {
                EdgeSelection?.Invoke(labelEdge.Attr.Id);
            }
        }

        public void Reload()
        {
            _graphViewer.Graph = _provider.GetGraph();
            Invalidate();
        }

        public void Update()
        {
            Invalidate();
        }

        private void Invalidate()
        {
            _graphViewer.Invalidate();
        }
    }
}