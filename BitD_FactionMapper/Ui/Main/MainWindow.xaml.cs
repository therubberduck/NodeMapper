using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class MainWindow
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;
        private readonly NodeFileManager _nodeFileManager = NodeFileManager.Instance;

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
            edgeEditorPanel.UpdateEdge += graphControl.UpdateEdge;
            nodeEditorPanel.RedrawGraph += graphControl.RedrawGraph;
            nodeEditorPanel.UpdateGraph += graphControl.UpdateGraph;
            _nodeFileManager.RedrawGraph += graphControl.RedrawGraph;
            _nodeFileManager.UpdateGraph += graphControl.UpdateGraph;

            _nodeDataManager.EdgeSelected += buttonPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += edgeEditorPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += nodeEditorPanel.OnEdgeSelected;
            _nodeDataManager.EdgeSelected += nodeEditorPanel.UpdateNodeEdges;
            _nodeDataManager.EdgeSelected += graphControl.UpdateGraph;
            
            _nodeDataManager.NodeSelected += nodeEditorPanel.OnNodeSelected;

            _nodeFileManager.ShowProgressOverlay += ShowProgressOverlay;
            _nodeFileManager.HideProgressOverlay += HideProgressOverlay; 
            buttonPanel.EdgeEditorPanel = edgeEditorPanel;

            
            _nodeFileManager.FirstLoad();
            graphControl.RedrawGraph();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            graphControl.RedrawGraph();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show(
                "Do you wish to save your graph before leaving?",
                "Exit FactionMapper",
                MessageBoxButton.YesNo
            );
            
            if(result is MessageBoxResult.Yes)
            {
                _nodeFileManager.Save();       
            }
        }
        
        private void ShowProgressOverlay()
        {
            frmWorking.Visibility = Visibility.Visible;
            AllowUIToUpdate();
        }

        private void HideProgressOverlay()
        {
            frmWorking.Visibility = Visibility.Collapsed;
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