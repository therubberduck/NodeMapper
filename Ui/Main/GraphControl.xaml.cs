using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class GraphControl
    {
        public delegate void NodeSelectionDelegate(Node node);
        public NodeSelectionDelegate NodeSelection;
        public delegate void EdgeSelectionDelegate(Edge edge);
        public EdgeSelectionDelegate EdgeSelection;
        
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
                NodeSelection?.Invoke(node.Node);
            }
            else if (item is IViewerEdge edge)
            {
                EdgeSelection?.Invoke(edge.Edge);   
            }
        }

        public void Reload()
        {
            _graphViewer.Graph = _provider.GraphViewerGraph;
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