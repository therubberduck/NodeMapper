using System.Windows;
using System.Windows.Controls;
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

            txtName.textBox.TextChanged += UpDateName_OnTextChanged;
            txtDescription.textBox.TextChanged += UpDateDescription_OnTextChanged;

            _nodeViewModel.SelectedNode = _graphViewModel.SelectedNode;
            UpdateNodePanelFromViewModel();
        }

        private void UpDateName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.LabelText = txtName.Text;
            graphControl.Update();
        }

        private void UpDateDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.UserData = txtDescription.Text;
            graphControl.Update();
        }

        private void btnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            var edgeToRemove = _nodeViewModel.SelectedEdge;
            if (edgeToRemove != null)
            {
                _graphViewModel.RemoveEdge(edgeToRemove);
                UpdateNodePanelFromViewModel();
                graphControl.Update();
            }
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
            if (nodeSelected && _nodeViewModel.SelectedNode != _graphViewModel.SelectedNode)
            {
                _nodeViewModel.SelectedNode = _graphViewModel.SelectedNode;
                UpdateNodePanelFromViewModel();
                graphControl.Update();
            }
        }

        private void LstEdges_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var edge = (lstEdges.SelectedItem as NodeViewModel.EdgeItem)?.Edge;
            
            if(edge == null || edge == _nodeViewModel.SelectedEdge) return;
            _nodeViewModel.SelectedEdge = edge;
            graphControl.Update();
        }

        private void UpdateNodePanelFromViewModel()
        {
            txtName.Text = _nodeViewModel.NodeName;
            txtDescription.Text = _nodeViewModel.NodeDescription;
            lstEdges.Items.Clear();
            foreach (var edgeItem in _nodeViewModel.EdgeItems)
            {
                lstEdges.Items.Add(edgeItem);                
            }
            foreach (NodeViewModel.EdgeItem item in lstEdges.Items)
            {
                if (item.Edge == _nodeViewModel.SelectedEdge)
                {
                    lstEdges.SelectedItem = item;
                    break;
                }
            }
        }
    }
}