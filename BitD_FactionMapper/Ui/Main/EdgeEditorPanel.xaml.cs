using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class EdgeEditorPanel
    {
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;

        public GraphControl.RedrawGraphDelegate RedrawGraph;
        public GraphControl.UpdateGraphDelegate UpdateGraph;
        public GraphControl.UpdateEdgeDelegate UpdateEdge;

        private const string Ally = "Ally";
        private const string Friend = "Friend";
        private const string Neutral = "Neutral";
        private const string Enemy = "Enemy";
        private const string War = "War";

        private Edge _currentEdge;

        public EdgeEditorPanel()
        {
            InitializeComponent();
            
            cmbRelation.AddItems(Ally, Friend, Neutral, Enemy, War);
            
            txtEdgeName.TextUpdated += txtEdgeName_TextUpdated;
        }

        private void cmbRelation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEdge = _nodeDataManager.SelectedEdge;
            if (cmbRelation.SelectedItem != null && selectedEdge != null)
            {
                var relationship = MapRelationship(cmbRelation.SelectedItem as string);
                if (selectedEdge.Relation != relationship)
                {
                    selectedEdge.Relation = relationship;
                    UpdateEdge(selectedEdge.EdgeId);
                }
            }
        }

        private void txtEdgeName_TextUpdated(string newtext)
        {
            var selectedEdge = _nodeDataManager.SelectedEdge;
            if (selectedEdge.LabelText != txtEdgeName.Text)
            {
                selectedEdge.LabelText = txtEdgeName.Text;
                UpdateEdge(selectedEdge.EdgeId);
            }
        }

        private void CmbEdgeEditFrom_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEdge = _nodeDataManager.SelectedEdge;
            if (cmbEdgeEditFrom.SelectedItem != null)
            {
                var edgeFrom = cmbEdgeEditFrom.SelectedItem as NodeItem;
                if (edgeFrom != null && selectedEdge.SourceId != edgeFrom.NodeId)
                {
                    selectedEdge.SourceId = edgeFrom.NodeId;

                    RedrawGraph();
                }
            }
        }

        private void CmbEdgeEditTo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedEdge = _nodeDataManager.SelectedEdge;
            if (cmbEdgeEditTo.SelectedItem != null)
            {
                var edgeTarget = cmbEdgeEditTo.SelectedItem as NodeItem;
                if (edgeTarget != null && selectedEdge.TargetId != edgeTarget.NodeId)
                {
                    selectedEdge.TargetId = edgeTarget.NodeId;

                    RedrawGraph();
                }
            }
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRelation.SelectedItem == null ||
                cmbEdgeEditFrom.SelectedItem == null ||
                cmbEdgeEditTo.SelectedItem == null) return;
            AddEdge();
        }

        private void btnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _nodeDataManager.RemoveEdge(_nodeDataManager.SelectedEdge.EdgeId);
            _nodeDataManager.SelectedEdge = null;

            RedrawGraph();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var previousSelectedEdge = _nodeDataManager.SelectedEdge; 
            _nodeDataManager.SelectedEdge = null;
            EdgeDeselected();
            
            if (previousSelectedEdge != null) // If we are making a new edge and cancel
            {
                UpdateEdge(previousSelectedEdge.EdgeId);                
            }
        }

        public void ShowCreate()
        {
            var nodes = UiItemMapper.Map(_nodeDataManager.Nodes);
            cmbEdgeEditFrom.SetItems(nodes);
            cmbEdgeEditTo.SetItems(nodes);
            SelectNodeInComboBox(cmbEdgeEditFrom, _nodeDataManager.SelectedNode);

            btnAddEdge.Visibility = Visibility.Visible;
            btnRemoveEdge.Visibility = Visibility.Collapsed;
            
            Visibility = Visibility.Visible;
        }

        private void AddEdge()
        {
            if (cmbEdgeEditFrom.SelectedItem == null || cmbEdgeEditTo.SelectedItem == null) return;
            
            var nodeFromId = (cmbEdgeEditFrom.SelectedItem as NodeItem).NodeId;
            var nodeToId = (cmbEdgeEditTo.SelectedItem as NodeItem).NodeId;
            var relation = MapRelationship(cmbRelation.SelectedItem as string);
            var newEdge = _nodeDataManager.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId, relation);

            RedrawGraph();
            _nodeDataManager.SelectedEdge = newEdge;
            EdgeSelected();
        }

        private static Edge.Relationship MapRelationship(string text)
        {
            switch (text)
            {
                case Ally:
                    return Edge.Relationship.Ally;
                case Friend:
                    return Edge.Relationship.Friend;
                case Neutral:
                    return Edge.Relationship.Neutral;
                case Enemy:
                    return Edge.Relationship.Enemy;
                case War:
                    return Edge.Relationship.War;
            }

            return Edge.Relationship.Neutral;
        }

        public void OnEdgeSelected()
        {
            if (_nodeDataManager.SelectedEdge == _currentEdge) return;
            _currentEdge = _nodeDataManager.SelectedEdge;

            if (_currentEdge == null)
            {
                EdgeDeselected();
            }
            else
            {
                EdgeSelected();
            }
        }

        private void EdgeSelected()
        {
            var nodes = UiItemMapper.Map(_nodeDataManager.Nodes);
            cmbEdgeEditFrom.SetItems(nodes);
            cmbEdgeEditTo.SetItems(nodes);

            txtEdgeName.Text = _currentEdge.LabelText;

            SelectNodeInComboBox(cmbEdgeEditFrom, _currentEdge.SourceNode);
            SelectNodeInComboBox(cmbEdgeEditTo, _currentEdge.TargetNode);
            SelectRelationshipInComboBox(_currentEdge.Relation);
            
            btnAddEdge.Visibility = Visibility.Collapsed;
            btnRemoveEdge.Visibility = Visibility.Visible;
            
            Visibility = Visibility.Visible;
        }

        private void SelectNodeInComboBox(LabeledComboBox comboBox, Node node)
        {
            comboBox.SelectItem<NodeItem>(cmbItem => cmbItem.NodeId == node.NodeId);
        }

        private void SelectRelationshipInComboBox(Edge.Relationship relation)
        {
            switch (relation)
            {
                case Edge.Relationship.Ally:
                    cmbRelation.SelectedItem = Ally;
                    break;
                case Edge.Relationship.Friend:
                    cmbRelation.SelectedItem = Friend;
                    break;
                case Edge.Relationship.Neutral:
                    cmbRelation.SelectedItem = Neutral;
                    break;
                case Edge.Relationship.Enemy:
                    cmbRelation.SelectedItem = Enemy;
                    break;
                case Edge.Relationship.War:
                    cmbRelation.SelectedItem = War;
                    break;
            }
        }

        private void EdgeDeselected()
        {
            Visibility = Visibility.Collapsed;

            txtEdgeName.Text = "";
        }
    }
}