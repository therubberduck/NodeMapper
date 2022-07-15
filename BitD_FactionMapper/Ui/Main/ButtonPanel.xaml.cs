using System.Windows;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class ButtonPanel
    {
        
        public GraphControl.RedrawGraphDelegate RedrawGraph;
        public GraphControl.UpdateGraphDelegate UpdateGraph;
        
        public delegate void ShowProgressOverlayDelegate();
        public ShowProgressOverlayDelegate ShowProgressOverlay;
        
        public delegate void HideProgressOverlayDelegate();
        public HideProgressOverlayDelegate HideProgressOverlay;
        
        public EdgeEditorPanel EdgeEditorPanel { private get; set; }
        
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;
        
        public ButtonPanel()
        {
            InitializeComponent();
        }

        public void OnEdgeSelected()
        {
            if (_nodeDataManager.SelectedEdge == null)
            {
                btnAddEdge.Visibility = Visibility.Visible;
            }
            else
            {
                btnAddEdge.Visibility = Visibility.Collapsed;
            }
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            var newNode = _nodeDataManager.CreateNewNodeWithEdgeFrom(_nodeDataManager.SelectedNode.NodeId);
            _nodeDataManager.SelectedNode = newNode;
            _nodeDataManager.SelectedEdge = _nodeDataManager.FirstEdgeOf(newNode);

            UpdateGraph();
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (_nodeDataManager.NodeCount == 1)
            {
                MessageBox.Show("You cannot delete the last node.");
                return;
            }

            var neighbor = _nodeDataManager.GetNeighborNode(_nodeDataManager.SelectedNode);
            _nodeDataManager.RemoveNode(_nodeDataManager.SelectedNode);
            _nodeDataManager.SelectedNode = neighbor;

            UpdateGraph();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ShowProgressOverlay();
            
            _nodeDataManager.SaveGraph();

            HideProgressOverlay();
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeEditorPanel.ShowCreate();
        }
    }
}