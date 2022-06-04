using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Msagl.Drawing;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class EdgeEditorPanel
    {
        private readonly GraphProvider _graphProvider = GraphProvider.Instance;
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
            var nodes = _graphProvider.Graph.Nodes.Select(node => new NodeItem(node));
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
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
            var nodeFromId = (cmbEdgeEditFrom.SelectedItem as NodeItem)?.Node.Id;
            var nodeToId = (cmbEdgeEditTo.SelectedItem as NodeItem)?.Node.Id;
            var newEdge = _graphProvider.Graph.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId);

            _nodeViewModel.SelectedEdge = newEdge;
            _nodeViewModel.UpdateGraph();
        }

        private void OnEdgeSelected(Edge edge)
        {
            var nodes = _graphProvider.Graph.Nodes.Select(node => new NodeItem(node));
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
            }

            if (_nodeViewModel.SelectedEdge.Label != null)
            {
                txtEdgeName.Text = _nodeViewModel.SelectedEdge.LabelText;
            }
                
            SelectNodeInComboBox(cmbEdgeEditFrom, _nodeViewModel.SelectedEdge.SourceNode);
            SelectNodeInComboBox(cmbEdgeEditTo, _nodeViewModel.SelectedEdge.TargetNode);
            
            btnAddEdge.Content = "Edit Edge";
            btnAddEdge.Click += EditEdge;
            btnRemoveEdge.Visibility = Visibility.Visible;
            
            Visibility = Visibility.Visible;
        }

        private void SelectNodeInComboBox(ComboBox comboBox, Node node)
        {
            foreach (NodeItem item in comboBox.Items)
            {
                if (item.Node == node)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void EditEdge(object sender, RoutedEventArgs e)
        {
            var selectedEdge = _nodeViewModel.SelectedEdge;
            if (selectedEdge.Label == null)
            {
                selectedEdge.Label = new Microsoft.Msagl.Drawing.Label();
            }

            if (selectedEdge.LabelText != txtEdgeName.Text || 
                cmbEdgeEditFrom.SelectedItem != null || 
                cmbEdgeEditTo.SelectedItem != null ||
                selectedEdge.SourceNode.LabelText != cmbEdgeEditFrom.Text ||
                selectedEdge.TargetNode.LabelText != cmbEdgeEditTo.Text)
            {
                _graphProvider.Graph.RemoveEdge(selectedEdge);
                AddEdge();
            }
        }

        private void BtnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _graphProvider.Graph.RemoveEdge(_nodeViewModel.SelectedEdge);
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