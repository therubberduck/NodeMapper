using System.Windows;
using System.Windows.Controls;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class EdgeEditorPanel
    {
        private readonly GraphManager _graphManager = GraphManager.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;
        
        public EdgeEditorPanel()
        {
            InitializeComponent();
            
            _nodeViewModel.OnEdgeSelected += OnEdgeSelected;
            _nodeViewModel.OnEdgeDeselected += OnEdgeDeselected;
            
            btnRemoveEdge.Click += BtnRemoveEdge_Click;
            btnCancel.Click += btnCancel_Click;
        }

        public void ShowCreate()
        {
            var nodes = _nodeViewModel.AllNodeItems;
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);

                if (nodeItem.NodeId == _nodeViewModel.SelectedNode.NodeId)
                {
                    cmbEdgeEditFrom.SelectedItem = nodeItem;
                }
            }

            btnAddEdge.Content = "Add Edge";
            btnAddEdge.Click += AddEdge;
            btnRemoveEdge.Visibility = Visibility.Hidden;
            
            Visibility = Visibility.Visible;
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            if (cmbEdgeEditFrom.SelectedItem == null || cmbEdgeEditTo.SelectedItem == null) return;
            AddEdge();
        }
        
        private void AddEdge()
        {
            var nodeFromId = (cmbEdgeEditFrom.SelectedItem as NodeItem)?.NodeId;
            var nodeToId = (cmbEdgeEditTo.SelectedItem as NodeItem)?.NodeId;
            var newEdge = _graphManager.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId);

            _nodeViewModel.SelectedEdge = newEdge;
            _nodeViewModel.UpdateNodeDetails();
        }

        private void OnEdgeSelected(Edge edge)
        {
            var nodes = _nodeViewModel.AllNodeItems;
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
            }

            txtEdgeName.Text = edge.LabelText;
                
            SelectNodeInComboBox(cmbEdgeEditFrom, edge.SourceNode);
            SelectNodeInComboBox(cmbEdgeEditTo, edge.TargetNode);
            
            btnAddEdge.Content = "Edit Edge";
            btnAddEdge.Click += EditEdge;
            btnRemoveEdge.Visibility = Visibility.Visible;
            
            Visibility = Visibility.Visible;
        }

        private void SelectNodeInComboBox(ComboBox comboBox, Node node)
        {
            foreach (NodeItem item in comboBox.Items)
            {
                if (item.NodeId == node.NodeId)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void EditEdge(object sender, RoutedEventArgs e)
        {
            var selectedEdge = _nodeViewModel.SelectedEdge;
            
            if (selectedEdge != null && selectedEdge.LabelText != txtEdgeName.Text || 
                cmbEdgeEditFrom.SelectedItem != null || 
                cmbEdgeEditTo.SelectedItem != null ||
                selectedEdge.SourceNode.Title != cmbEdgeEditFrom.Text ||
                selectedEdge.TargetNode.Title != cmbEdgeEditTo.Text)
            {
                _graphManager.RemoveEdge(selectedEdge.EdgeId);
                AddEdge();
            }
        }

        private void BtnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _graphManager.RemoveEdge(_nodeViewModel.SelectedEdge.EdgeId);
            _nodeViewModel.SelectedEdge = null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _nodeViewModel.SelectedEdge = null;
        }

        private void OnEdgeDeselected()
        {
            Visibility = Visibility.Collapsed;
            
            txtEdgeName.Text = "";
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
        }
    }
}