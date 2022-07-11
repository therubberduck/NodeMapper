using System;
using System.Windows;
using System.Windows.Threading;
using NodeMapper.Model;

namespace NodeMapper.Ui.Main
{
    public partial class MainWindow
    {
        private readonly GraphManager _graphManager = GraphManager.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;

        public MainWindow()
        {
            _graphManager.InitEdges();
            InitializeComponent();

            graphControl.NodeSelection += graphControl_OnNodeSelection;
            graphControl.EdgeSelection += graphControl_OnEdgeSelection;
            _nodeViewModel.UpdateGraph += () => graphControl.Update();

            buttonPanel.ShowProgressOverlay += () =>
            {
                frmWorking.Visibility = Visibility.Visible;
                AllowUIToUpdate();
            };
            buttonPanel.HideProgressOverlay += () => frmWorking.Visibility = Visibility.Collapsed;
            buttonPanel.EdgeEditorPanel = edgeEditorPanel;
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            graphControl.Reload();
        }

        private void graphControl_OnNodeSelection(string nodeId)
        {
            if (_nodeViewModel.SelectedNode.NodeId == nodeId) return;

            var newlySelectedNode = _graphManager.GetNode(nodeId);
            _nodeViewModel.SelectedNode = newlySelectedNode;
        }

        private void graphControl_OnEdgeSelection(string edgeId)
        {
            if (_nodeViewModel.SelectedEdge?.EdgeId == edgeId) return;
            
            var edgeSelected = _graphManager.GetEdge(edgeId);
            if (_nodeViewModel.SelectedNode.NodeId != edgeSelected.SourceId &&
                _nodeViewModel.SelectedNode.NodeId != edgeSelected.TargetId)
            {
                _nodeViewModel.SelectedNode = edgeSelected.SourceNode;
            }

            _nodeViewModel.SelectedEdge = edgeSelected;
        }

        private void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(
                delegate(object parameter)
                {
                    frame.Continue = false;
                    return null;
                }), null);

            Dispatcher.PushFrame(frame);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                new Action(delegate { }));
        }
    }
}