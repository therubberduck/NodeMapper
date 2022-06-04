using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public partial class NodeDetailPanel : UserControl
    {
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;
        
        public NodeDetailPanel()
        {
            InitializeComponent();
            
            _nodeViewModel.OnNodeSelected += OnNodeSelected;
            _nodeViewModel.OnEdgeSelected += OnEdgeSelected;
            _nodeViewModel.OnEdgeDeselected += OnEdgeDeselected;
            
            txtName.textBox.TextChanged += UpDateName_OnTextChanged;
            txtDescription.textBox.TextChanged += UpDateDescription_OnTextChanged;

            _nodeViewModel.Init();
        }

        private void OnNodeSelected(Node node)
        {
            txtName.Text = _nodeViewModel.NodeName;
            txtDescription.Text = _nodeViewModel.NodeDescription;
            lstEdges.Items.Clear();
            foreach (var edgeItem in _nodeViewModel.EdgeItems)
            {
                lstEdges.Items.Add(edgeItem);                
            }
            foreach (EdgeItem item in lstEdges.Items)
            {
                if (item.Edge == _nodeViewModel.SelectedEdge)
                {
                    lstEdges.SelectedItem = item;
                    break;
                }
            }
        }

        private void OnEdgeSelected(Edge edge)
        {
            if ((lstEdges.SelectedItem as EdgeItem)?.Edge == edge) return;
            
            foreach (EdgeItem item in lstEdges.Items)
            {
                if (item.Edge == _nodeViewModel.SelectedEdge)
                {
                    lstEdges.SelectedItem = item;
                    break;
                }
            }
        }
        

        private void UpDateName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.LabelText = txtName.Text;
            _nodeViewModel.SelectedNode.Id = txtName.Text;
            _nodeViewModel.UpdateGraph();
        }

        private void UpDateDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.UserData = txtDescription.Text;
            _nodeViewModel.UpdateGraph();
        }

        private void LstEdges_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var edge = (lstEdges.SelectedItem as EdgeItem)?.Edge;
            
            if(edge == null || edge == _nodeViewModel.SelectedEdge) return;
            _newItemSelected = true;
            _nodeViewModel.SelectedEdge = edge;
        }

        private bool _newItemSelected = false;
        private void LstEdges_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_newItemSelected)
            {
                _nodeViewModel.SelectedEdge = null;
            }
            _newItemSelected = false;
        }
        
        

        private void OnEdgeDeselected()
        {
            lstEdges.SelectedIndex = -1;
        }
    }
}