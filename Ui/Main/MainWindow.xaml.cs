using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.Ui.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly GraphViewModel _graphViewModel = new GraphViewModel();
        private readonly NodeViewModel _nodeViewModel = new NodeViewModel();
        
        public MainWindow()
        {
            InitializeComponent();

            graphControl.Graph = _graphViewModel.Graph;
            (graphControl.GraphViewer as IViewer).MouseUp += GraphControl_OnMouseUp;

            txtName.textBox.TextChanged += UpDateName_OnTextChanged;
            txtDescription.textBox.TextChanged += UpDateDescription_OnTextChanged;

            _nodeViewModel.SelectedNode = _graphViewModel.SelectedNode;
            UpdateNodePanelFromViewModel();
        }

        private void UpDateName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.LabelText = txtName.Text;
            graphControl.Update();
        }

        private void UpDateDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _nodeViewModel.SelectedNode.UserData = txtDescription.Text;
            graphControl.Update();
        }

        private void btnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            var edgeToRemove = _nodeViewModel.SelectedEdge;
            if (edgeToRemove != null)
            {
                _graphViewModel.RemoveEdge(edgeToRemove);
                UpdateNodePanelFromViewModel();
                graphControl.Update();
            }
        }

        private void btnCreateNode_Click(object sender, RoutedEventArgs e)
        {
            _graphViewModel.CreateNewEdge();
            graphControl.Update();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            frmWorking.Visibility = Visibility.Visible;
            AllowUIToUpdate();
            
            _graphViewModel.SaveGraph();
            
            frmWorking.Visibility = Visibility.Collapsed;
        }
        

        private void GraphControl_OnMouseUp(object sender, MsaglMouseEventArgs  e)
        {
            graphControl.GraphViewer.ScreenToSource(e);
            
            var nodeSelected = _graphViewModel.SelectNode(graphControl.GraphViewer.ScreenToSource(e));
            if (nodeSelected && _nodeViewModel.SelectedNode != _graphViewModel.SelectedNode)
            {
                _nodeViewModel.SelectedNode = _graphViewModel.SelectedNode;
                UpdateNodePanelFromViewModel();
                graphControl.Update();
            }
        }

        private void LstEdges_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var edge = (lstEdges.SelectedItem as EdgeItem)?.Edge;
            
            if(edge == null || edge == _nodeViewModel.SelectedEdge) return;
            _newItemSelected = true;
            _nodeViewModel.SelectedEdge = edge;
            
            edgeEditorPanel.ShowEdit(_graphViewModel.Graph, edge, EdgeEditorPanelCallback);
            btnAddEdge.Visibility = Visibility.Collapsed;
            
            graphControl.Update();
        }

        private bool _newItemSelected = false;
        private void LstEdges_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_newItemSelected)
            {
                _nodeViewModel.SelectedEdge = null;
                lstEdges.SelectedIndex = -1;
                edgeEditorPanel.Hide();
                btnAddEdge.Visibility = Visibility.Visible;
            }
            _newItemSelected = false;
        }

        private void UpdateNodePanelFromViewModel()
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
                if (item.Edge == _nodeViewModel.SelectedEdge)
                {
                    lstEdges.SelectedItem = item;
                    break;
                }
            }
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            edgeEditorPanel.ShowCreate(_graphViewModel.Graph, EdgeEditorPanelCallback);
            btnAddEdge.Visibility = Visibility.Collapsed;
        }

        private void EdgeEditorPanelCallback(bool successful, Edge newSelectedEdge)
        {
            edgeEditorPanel.Hide();
            btnAddEdge.Visibility = Visibility.Visible;

            if (successful)
            {
                _nodeViewModel.SelectedEdge = newSelectedEdge;
                UpdateNodePanelFromViewModel();
                graphControl.Update();

                if (newSelectedEdge != null)
                {
                    edgeEditorPanel.ShowEdit(_graphViewModel.Graph, newSelectedEdge, EdgeEditorPanelCallback);
                    btnAddEdge.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
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