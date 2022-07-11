using System.Windows;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class ButtonPanel
    {
        
        public delegate void ShowProgressOverlayDelegate();
        public ShowProgressOverlayDelegate ShowProgressOverlay;
        
        public delegate void HideProgressOverlayDelegate();
        public HideProgressOverlayDelegate HideProgressOverlay;
        
        public EdgeEditorPanel EdgeEditorPanel { private get; set; }
        
        private readonly GraphManager _graphManager = GraphManager.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;
        
        public ButtonPanel()
        {
            InitializeComponent();

            _nodeViewModel.OnEdgeSelected += edge => btnAddEdge.Visibility = Visibility.Collapsed;
            _nodeViewModel.OnEdgeDeselected += () => btnAddEdge.Visibility = Visibility.Visible;
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            var newNode = _graphManager.CreateNewNodeWithEdgeFrom(_nodeViewModel.SelectedNode.NodeId);
            _nodeViewModel.SelectedNode = newNode;
            _nodeViewModel.SelectedEdge = _graphManager.FirstEdgeOf(newNode);
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (_graphManager.NodeCount == 1)
            {
                MessageBox.Show("You cannot delete the last node.");
                return;
            }

            var neighbor = _graphManager.GetNeighborNode(_nodeViewModel.SelectedNode);
            _graphManager.RemoveNode(_nodeViewModel.SelectedNode);
            _nodeViewModel.SelectedNode = neighbor;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            
            _graphManager.SaveGraph();

            HideProgressOverlay();
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeEditorPanel.ShowCreate();
        }
    }
}