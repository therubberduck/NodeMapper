using System.Windows.Controls;
using System.Windows.Input;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class NodeDetailPanel
    {
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;
        private readonly MsaglGraphProvider _graphProvider = MsaglGraphProvider.Instance;

        public NodeDetailPanel()
        {
            InitializeComponent();

            _nodeViewModel.UpdateNodeDetails += () => OnNodeSelected(_nodeViewModel.SelectedNode);
            _nodeViewModel.OnNodeSelected += OnNodeSelected;
            _nodeViewModel.OnEdgeSelected += OnEdgeSelected;
            _nodeViewModel.OnEdgeDeselected += OnEdgeDeselected;

            txtName.TextUpdated += UpDateName_OnTextUpdated;
            txtDescription.TextUpdated += UpDateDescription_OnTextUpdated;

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
                if (item.Edge.EdgeId == _nodeViewModel.SelectedEdge?.EdgeId)
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


        private void UpDateName_OnTextUpdated(string s)
        {
            var selectedNode = _nodeViewModel.SelectedNode;
            var newText = txtName.Text;
            if (selectedNode.Title != newText)
            {
                selectedNode.Title = newText;
                _graphProvider.ReloadGraph();
            }
        }

        private void UpDateDescription_OnTextUpdated(string newText)
        {
            if (_nodeViewModel.SelectedNode.Body != txtDescription.Text)
            {
                _nodeViewModel.SelectedNode.Body = txtDescription.Text;
                _graphProvider.ReloadGraph();
            }
        }

        private void LstEdges_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var edge = (lstEdges.SelectedItem as EdgeItem)?.Edge;

            if (edge == null || edge == _nodeViewModel.SelectedEdge) return;
            _newItemSelected = true;
            _nodeViewModel.SelectedEdge = edge;
        }

        // ReSharper disable once RedundantDefaultMemberInitializer
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