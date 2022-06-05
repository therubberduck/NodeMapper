using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class ButtonPanel : UserControl
    {
        
        public delegate void ShowProgressOverlayDelegate();
        public ShowProgressOverlayDelegate OnShowProgressOverlay;
        
        public delegate void HideProgressOverlayDelegate();
        public HideProgressOverlayDelegate OnHideProgressOverlay;
        
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
            _nodeViewModel.SelectedEdge = newNode.Edges.First();
            _nodeViewModel.ReloadGraph();
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (_graphProvider.Graph.Nodes.Count() == 1)
            {
                MessageBox.Show("You cannot delete the last node.");
                return;
            }
            
            _graphProvider.Graph.RemoveNode(_nodeViewModel.SelectedNode);
            _nodeViewModel.SelectedNode = _graphProvider.Graph.Nodes.First();
            _nodeViewModel.ReloadGraph();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            OnShowProgressOverlay();
            
            _graphProvider.SaveGraph();

            OnHideProgressOverlay();
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeEditorPanel.ShowCreate();
        }
    }
}