using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class NodeDetailPanel
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;

        public GraphControl.RedrawGraphDelegate RedrawGraph;
        public GraphControl.UpdateGraphDelegate UpdateGraph;

        public NodeDetailPanel()
        {
            InitializeComponent();

            txtName.TextUpdated += UpDateName_OnTextUpdated;
            txtDescription.TextUpdated += UpDateDescription_OnTextUpdated;
            
            
            cmbType.Items.Add(FactionType.Fringe);
            cmbType.Items.Add(FactionType.Institution);
            cmbType.Items.Add(FactionType.Labor);
            cmbType.Items.Add(FactionType.Underworld);
            cmbType.Items.Add(FactionType.Other);
        }

        private void CmbType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedNode = _nodeDataManager.SelectedNode;
            var newFactionType = (FactionType)cmbType.SelectedItem;
            
            if (selectedNode.FactionType != newFactionType)
            {
                selectedNode.FactionType = newFactionType;
                RedrawGraph();
            }
        }

        public void OnNodeSelected()
        {
            var node = _nodeDataManager.SelectedNode;

            txtName.Text = node.Title;
            txtDescription.Text = node.Body;
            cmbType.SelectedItem = node.FactionType;
            UpdateNodeEdges();
        }

        internal void OnEdgeSelected()
        {
            UpdateNodeEdges();
            
            var edge = _nodeDataManager.SelectedEdge;
            if ((lstEdges.SelectedItem as EdgeItem)?.Edge == edge) return;

            if (edge == null)
            {
                lstEdges.SelectedIndex = -1;
                lstEdges.SelectedItem = null;
                return;
            }

            foreach (EdgeItem item in lstEdges.Items)
            {
                if (item.Edge == _nodeDataManager.SelectedEdge)
                {
                    lstEdges.SelectedItem = item;
                    break;
                }
            }
        }

        public void UpdateNodeEdges()
        {
            var node = _nodeDataManager.SelectedNode;
            lstEdges.Items.Clear();
            foreach (var edgeItem in UiItemMapper.Map(node.Edges))
            {
                lstEdges.Items.Add(edgeItem);
            }

            if (_nodeDataManager.SelectedEdge == null)
            {
                lstEdges.SelectedIndex = -1;
            }
            else
            {
                foreach (EdgeItem item in lstEdges.Items)
                {
                    if (item.Edge.EdgeId == _nodeDataManager.SelectedEdge.EdgeId)
                    {
                        lstEdges.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void UpDateName_OnTextUpdated(string s)
        {
            var selectedNode = _nodeDataManager.SelectedNode;
            var newText = txtName.Text;
            if (selectedNode.Title != newText)
            {
                selectedNode.Title = newText;
                RedrawGraph();
            }
        }

        private void UpDateDescription_OnTextUpdated(string newText)
        {
            if (_nodeDataManager.SelectedNode.Body != txtDescription.Text)
            {
                _nodeDataManager.SelectedNode.Body = txtDescription.Text;
                RedrawGraph();
            }
        }

        private void LstEdges_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var edge = (lstEdges.SelectedItem as EdgeItem)?.Edge;

            if (edge == null || edge == _nodeDataManager.SelectedEdge) return;

            _newItemSelected = true;
            _nodeDataManager.SelectedEdge = edge;
        }

        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool _newItemSelected = false;

        private void LstEdges_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_newItemSelected)
            {
                _nodeDataManager.SelectedEdge = null;
            }

            _newItemSelected = false;
        }
    }
}