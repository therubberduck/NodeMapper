using System.Windows;
using Microsoft.Msagl.Drawing;
using NodeMapper.Ui.Main;

namespace NodeMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GraphViewModel _graphViewModel = new GraphViewModel();
        private readonly NodeViewModel _nodeViewModel = new NodeViewModel();
        
        public MainWindow()
        {
            InitializeComponent();

            graphControl.Graph = _graphViewModel.Graph;
            (graphControl.GraphViewer as IViewer).MouseUp += GraphControl_OnMouseUp;
            
            UpdateNodePanelFromViewModel();
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            _graphViewModel.CreateNewEdge();
            graphControl.Update();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _graphViewModel.SaveGraph();
            MessageBox.Show("Db Saved");
        }

        private void GraphControl_OnMouseUp(object sender, MsaglMouseEventArgs  e)
        {

            graphControl.GraphViewer.ScreenToSource(e);
            
            var nodeSelected = _graphViewModel.SelectNode(graphControl.GraphViewer.ScreenToSource(e));
            if (nodeSelected)
            {
                _nodeViewModel.SelectedNode = _graphViewModel.SelectedNode;
                UpdateNodePanelFromViewModel();
            }
        }

        private void UpdateNodePanelFromViewModel()
        {
            txtDescription.Text = _nodeViewModel.NodeDescription;
            lstEdges.Items.Clear();
            foreach (var edgeItem in _nodeViewModel.EdgeItems)
            {
                lstEdges.Items.Add(edgeItem);                
            }
            
        }
    }
}