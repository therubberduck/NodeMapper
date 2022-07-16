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

        private const string Ally = "Ally";
        private const string Friend = "Friend";
        private const string Neutral = "Neutral";
        private const string Enemy = "Enemy";
        private const string War = "War";

        private Edge _currentEdge;

        public EdgeEditorPanel()
        {
            InitializeComponent();

            cmbRelation.Items.Add(Ally);
            cmbRelation.Items.Add(Friend);
            cmbRelation.Items.Add(Neutral);
            cmbRelation.Items.Add(Enemy);
            cmbRelation.Items.Add(War);

            btnRemoveEdge.Click += BtnRemoveEdge_Click;
            btnCancel.Click += btnCancel_Click;
        }

        public void ShowCreate()
        {
            var nodes = UiItemMapper.Map(_nodeDataManager.AllNodes);
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);

                if (nodeItem.NodeId == _nodeDataManager.SelectedNode.NodeId)
                {
                    cmbEdgeEditFrom.SelectedItem = nodeItem;
                }
            }

            btnAddEdge.Content = "Add Edge";
            btnAddEdge.Click -= btnAddEdge_Click;
            btnAddEdge.Click -= btnEditEdge_Click;
            btnAddEdge.Click += btnAddEdge_Click;
            btnRemoveEdge.Visibility = Visibility.Hidden;

            Visibility = Visibility.Visible;
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRelation.SelectedItem == null ||
                cmbEdgeEditFrom.SelectedItem == null ||
                cmbEdgeEditTo.SelectedItem == null) return;
            AddEdge();
        }

        private void btnEditEdge_Click(object sender, RoutedEventArgs e)
        {
            var selectedEdge = _nodeDataManager.SelectedEdge;

            if (selectedEdge == null) return;

            if (selectedEdge.LabelText != txtEdgeName.Text)
            {
                selectedEdge.LabelText = txtEdgeName.Text;
            }

            if (cmbEdgeEditFrom.SelectedItem != null)
            {
                var edgeFrom = cmbEdgeEditFrom.SelectedItem as NodeItem;
                if (edgeFrom != null && selectedEdge.SourceId != edgeFrom.NodeId)
                {
                    selectedEdge.SourceId = edgeFrom.NodeId;
                }
            }

            if (cmbEdgeEditTo.SelectedItem != null)
            {
                var edgeTarget = cmbEdgeEditTo.SelectedItem as NodeItem;
                if (edgeTarget != null && selectedEdge.TargetId != edgeTarget.NodeId)
                {
                    selectedEdge.TargetId = edgeTarget.NodeId;
                }
            }

            if (cmbRelation.SelectedItem != null)
            {
                var relationship = MapRelationship(cmbRelation.SelectedItem as string);
                if (selectedEdge.Relation != relationship)
                {
                    selectedEdge.Relation = relationship;
                }
            }

            RedrawGraph();
        }

        private void BtnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _nodeDataManager.RemoveEdge(_nodeDataManager.SelectedEdge.EdgeId);
            _nodeDataManager.SelectedEdge = null;

            RedrawGraph();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _nodeDataManager.SelectedEdge = null;

            RedrawGraph();
        }

        private void AddEdge()
        {
            if (cmbEdgeEditFrom.SelectedItem == null || cmbEdgeEditTo.SelectedItem == null) return;
            
            var nodeFromId = (cmbEdgeEditFrom.SelectedItem as NodeItem).NodeId;
            var nodeToId = (cmbEdgeEditTo.SelectedItem as NodeItem).NodeId;
            var relation = MapRelationship(cmbRelation.SelectedItem as string);
            var newEdge = _nodeDataManager.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId, relation);

            _nodeDataManager.SelectedEdge = newEdge;
            RedrawGraph();
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
            var nodes = UiItemMapper.Map(_nodeDataManager.AllNodes);
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
            }

            txtEdgeName.Text = _currentEdge.LabelText;

            SelectNodeInComboBox(cmbEdgeEditFrom, _currentEdge.SourceNode);
            SelectNodeInComboBox(cmbEdgeEditTo, _currentEdge.TargetNode);
            SelectRelationshipInComboBox(_currentEdge.Relation);

            btnAddEdge.Content = "Edit Edge";
            btnAddEdge.Click -= btnAddEdge_Click;
            btnAddEdge.Click -= btnEditEdge_Click;
            btnAddEdge.Click += btnEditEdge_Click;
            btnRemoveEdge.Visibility = Visibility.Visible;

            Visibility = Visibility.Visible;
        }

        private void SelectNodeInComboBox(ComboBox comboBox, Node node)
        {
            foreach (NodeItem item in comboBox.Items)
            {
                if (item.NodeId == node.NodeId)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
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
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
        }
    }
}