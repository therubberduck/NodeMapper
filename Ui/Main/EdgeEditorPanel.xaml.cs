using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public partial class EdgeEditorPanel : Grid
    {
        public delegate void Result(bool successful, Edge selectedEdge);

        private Graph _graph;
        private Edge _selectedEdge;
        private Result _currentCallback;
        
        public EdgeEditorPanel()
        {
            InitializeComponent();
            
            btnRemoveEdge.Click += BtnRemoveEdge_Click;
            btnCancel.Click += btnCancel_Click;
        }

        public void ShowCreate(Graph graph, Result callback)
        {
            _graph = graph;
            _currentCallback = callback;
            
            var nodes = graph.Nodes.Select(node => new NodeItem(node));
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
            var newEdge = _graph.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId);
            
            _currentCallback.Invoke(true, newEdge);
        }

        public void ShowEdit(Graph graph, Edge edge, Result callback)
        {
            _graph = graph;
            _selectedEdge = edge;
            _currentCallback = callback;
            
            var nodes = graph.Nodes.Select(node => new NodeItem(node));
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
            }

            if (edge.Label != null)
            {
                txtEdgeName.Text = edge.LabelText;
            }
                
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
                if (item.Node == node)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void EditEdge(object sender, RoutedEventArgs e)
        {
            if (_selectedEdge.Label == null)
            {
                _selectedEdge.Label = new Microsoft.Msagl.Drawing.Label();
            }

            if (_selectedEdge.LabelText != txtEdgeName.Text || 
                cmbEdgeEditFrom.SelectedItem != null || 
                cmbEdgeEditTo.SelectedItem != null ||
                _selectedEdge.SourceNode.LabelText != cmbEdgeEditFrom.Text ||
                _selectedEdge.TargetNode.LabelText != cmbEdgeEditTo.Text)
            {
                _graph.RemoveEdge(_selectedEdge);
                AddEdge();
            }
        }

        private void BtnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _graph.RemoveEdge(_selectedEdge);
            _currentCallback.Invoke(true, null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _currentCallback.Invoke(false, _selectedEdge);
        }

        public void Hide()
        {
            Visibility = Visibility.Collapsed;
            
            txtEdgeName.Text = "";
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            
            _currentCallback = null;
            _selectedEdge = null;
            _graph = null;
        }
    }
}