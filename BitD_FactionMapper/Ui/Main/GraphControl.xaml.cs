using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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
        public delegate void UpdateEdgeDelegate(int edgeId);
        public delegate void UpdateNodeDelegate(int nodeId);
        
        private readonly GraphViewer _graphViewer = new GraphViewer();
        private readonly MsaglGraphProvider _provider = MsaglGraphProvider.Instance;

        public GraphControl() {
            InitializeComponent();
            
            _graphViewer.BindToPanel(dockPanel);
            _graphViewer.LayoutEditingEnabled = false;
            
            (_graphViewer as IViewer).MouseUp += OnMouseUp;
            _graphViewer.GraphCanvas.MouseWheel += GraphCanvasOnMouseWheel;
        }

        private void GraphCanvasOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }

        private void OnMouseUp(object sender, MsaglMouseEventArgs e)
        {
            var zoomFactor = _graphViewer.ZoomFactor;
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
            var currentScale = _graphViewer.CurrentScale;
            
            _graphViewer.Graph = _provider.GetNewGraph();
            _graphViewer.Invalidate();

            // Recenter on selected node after redraw
            var selectedNode = _graphViewer.Graph.Nodes.First(n => n.Attr.Id == Properties.Settings.Default.SelectedNode.ToString());
            _graphViewer.NodeToCenterWithScale(selectedNode, currentScale);
        }

        public void UpdateGraph()
        {
            _graphViewer.Graph = _provider.GetUpdatedGraph();
            _graphViewer.Invalidate();
        }

        public void UpdateEdge(int edgeId)
        {
            _graphViewer.Graph = _provider.GetUpdatedGraphEdge(edgeId);
            _graphViewer.Invalidate();
        }

        public void UpdateNode(int nodeId)
        {
            _graphViewer.Graph = _provider.GetUpdatedGraphNode(nodeId);
            _graphViewer.Invalidate();
        }

        public void RandomizeGraph()
        {
            _graphViewer.Graph = _provider.Randomize();
            _graphViewer.Invalidate();
        }
    }
}