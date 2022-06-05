using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class GraphControl
    {
        public delegate void GraphControl_OnNodeSelection(Node node);
        public GraphControl_OnNodeSelection OnNodeSelection;
        public delegate void GraphControl_OnEdgeSelection(Edge edge);
        public GraphControl_OnEdgeSelection OnEdgeSelection;
        
        private readonly GraphViewer _graphViewer = new GraphViewer();
        private readonly GraphProvider _provider = GraphProvider.Instance;

        public GraphControl() {
            InitializeComponent();
            
            _graphViewer.BindToPanel(dockPanel);
            _graphViewer.LayoutEditingEnabled = false;
            
            (_graphViewer as IViewer).MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MsaglMouseEventArgs e)
        {
            var item = _graphViewer.ObjectUnderMouseCursor;
            if (item is VNode node)
            {
                OnNodeSelection?.Invoke(node.Node);
            }
            else if (item is IViewerEdge edge)
            {
                OnEdgeSelection?.Invoke(edge.Edge);   
            }
            // var point = _graphViewer.ScreenToSource(e);
            // var node = _provider.SelectNodeAt(point);
            // OnNodeSelection(node);
        }

        public void Reload()
        {
            _graphViewer.Graph = _provider.Graph;
            Invalidate();
        }

        public void Update()
        {
            Invalidate();
        }

        private void Invalidate()
        {
            _provider.RedoBorders();
            _graphViewer.Invalidate();
        }
    }
}