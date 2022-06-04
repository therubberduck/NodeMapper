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
        
        private readonly GraphViewer _graphViewer = new GraphViewer();
        private readonly GraphProvider _provider = GraphProvider.Instance;
        private Graph _graph => _provider.Graph;

        public GraphControl() {
            InitializeComponent();
            
            _graphViewer.BindToPanel(dockPanel);

            _graphViewer.Graph = _graph;
            (_graphViewer as IViewer).MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MsaglMouseEventArgs e)
        {
            var point = _graphViewer.ScreenToSource(e);
            var node = _provider.SelectNodeAt(point);
            OnNodeSelection(node);
        }

        public void Update()
        {
            _graphViewer.Graph = _provider.Graph;
        }
    }
}