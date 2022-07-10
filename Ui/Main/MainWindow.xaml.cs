using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Msagl.Drawing;
using NodeMapper.Model;
using Edge = Microsoft.Msagl.Drawing.Edge;

namespace NodeMapper.Ui.Main
{
    public partial class MainWindow
    {
        private readonly GraphManager _graphManager = GraphManager.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;

        public MainWindow()
        {
            InitializeComponent();

            _graphManager.InitEdges();
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

        private void graphControl_OnNodeSelection(Node nodeSelected)
        {
            if (nodeSelected != null && _nodeViewModel.SelectedNode != nodeSelected)
            {
                _nodeViewModel.SelectedNode = nodeSelected;
            }
        }

        private void graphControl_OnEdgeSelection(string edgeId)
        {
            if (_nodeViewModel.SelectedEdge?.EdgeId == edgeId) return;
            
            var edgeSelected = _graphManager.GetEdge(edgeId);
            if (_nodeViewModel.SelectedNode.Id != edgeSelected.SourceId &&
                _nodeViewModel.SelectedNode.Id != edgeSelected.TargetId)
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