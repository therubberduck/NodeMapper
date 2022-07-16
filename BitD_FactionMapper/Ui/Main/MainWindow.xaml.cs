using System;
using System.Windows;
using System.Windows.Threading;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class MainWindow
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;

        public MainWindow()
        {
            InitializeComponent();
            
            menuBar.RedrawGraph += graphControl.RedrawGraph;
            menuBar.UpdateGraph += graphControl.UpdateGraph;
            buttonPanel.RedrawGraph += graphControl.RedrawGraph;
            buttonPanel.UpdateGraph += graphControl.UpdateGraph;
            buttonPanel.RandomizeGraph += graphControl.RandomizeGraph;
            edgeEditorPanel.RedrawGraph += graphControl.RedrawGraph;
            edgeEditorPanel.UpdateGraph += graphControl.UpdateGraph;
            nodeEditorPanel.RedrawGraph += graphControl.RedrawGraph;
            nodeEditorPanel.UpdateGraph += graphControl.UpdateGraph;

            _nodeDataManager.EdgeSelected += buttonPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += edgeEditorPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += nodeEditorPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += nodeEditorPanel.UpdateNodeEdges;
            _nodeDataManager.EdgeSelected += graphControl.UpdateGraph;
            
            _nodeDataManager.NodeSelected += nodeEditorPanel.OnNodeSelected;
            _nodeDataManager.NodeSelected += graphControl.UpdateGraph;
            
            menuBar.ShowProgressOverlay += () =>
            {
                frmWorking.Visibility = Visibility.Visible;
                AllowUIToUpdate();
            };
            menuBar.HideProgressOverlay += () => frmWorking.Visibility = Visibility.Collapsed;
            buttonPanel.EdgeEditorPanel = edgeEditorPanel;
            
            _nodeDataManager.LoadData();
            graphControl.RedrawGraph();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            graphControl.RedrawGraph();
        }

        private void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(
                delegate
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