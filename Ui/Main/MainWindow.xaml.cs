using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    public partial class MainWindow
    {
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;

        public MainWindow()
        {
            InitializeComponent();

            graphControl.NodeSelection += graphControl_OnNodeSelection;
            graphControl.EdgeSelection += graphControl_OnEdgeSelection;
            _nodeViewModel.ReloadGraph += () => graphControl.Reload();
            _nodeViewModel.UpdateGraph += () => graphControl.Update();

            buttonPanel.OnShowProgressOverlay += () =>
            {
                frmWorking.Visibility = Visibility.Visible;
                AllowUIToUpdate();
            };
            buttonPanel.OnHideProgressOverlay += () => frmWorking.Visibility = Visibility.Collapsed;
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

        private void graphControl_OnEdgeSelection(Edge edgeSelected)
        {
            if (_nodeViewModel.SelectedEdge != edgeSelected)
            {
                if (_nodeViewModel.SelectedNode != edgeSelected.SourceNode &&
                    _nodeViewModel.SelectedNode != edgeSelected.TargetNode)
                {
                    _nodeViewModel.SelectedNode = edgeSelected.SourceNode;
                }

                _nodeViewModel.SelectedEdge = edgeSelected;
            }
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