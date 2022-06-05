using System.Linq;
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
        
        private readonly GraphProvider _graphProvider = GraphProvider.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;
        
        public ButtonPanel()
        {
            InitializeComponent();

            _nodeViewModel.OnEdgeSelected += edge => btnAddEdge.Visibility = Visibility.Collapsed;
            _nodeViewModel.OnEdgeDeselected += () => btnAddEdge.Visibility = Visibility.Visible;
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            var newNode = _graphProvider.CreateNeNodeWithEdgeFrom(_nodeViewModel.SelectedNode);
            _nodeViewModel.SelectedNode = newNode;
            _nodeViewModel.SelectedEdge = _graphProvider.FirstEdgeOf(newNode);
            _nodeViewModel.ReloadGraph();
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (_graphProvider.NodeCount == 1)
            {
                MessageBox.Show("You cannot delete the last node.");
                return;
            }

            var neighbor = _graphProvider.GetNeighborNode(_nodeViewModel.SelectedNode);
            _graphProvider.RemoveNode(_nodeViewModel.SelectedNode);
            _nodeViewModel.SelectedNode = neighbor;
            _nodeViewModel.ReloadGraph();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            
            _graphProvider.SaveGraph();

            HideProgressOverlay();
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeEditorPanel.ShowCreate();
        }
    }
}