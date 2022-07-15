using System.Windows;
using System.Windows.Controls;
using BitD_FactionMapper.Model;

namespace BitD_FactionMapper.Ui.Main
{
    public partial class EdgeEditorPanel
    {
        private const string Ally = "Ally";
        private const string Friend = "Friend";
        private const string Neutral = "Neutral";
        private const string Enemy = "Enemy";
        private const string War = "War";
        
        private readonly GraphManager _graphManager = GraphManager.Instance;
        private readonly NodeViewModel _nodeViewModel = NodeViewModel.Instance;

        public EdgeEditorPanel()
        {
            InitializeComponent();

            _nodeViewModel.OnEdgeSelected += OnEdgeSelected;
            _nodeViewModel.OnEdgeDeselected += OnEdgeDeselected;
            
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
            var nodes = _nodeViewModel.AllNodeItems;
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);

                if (nodeItem.NodeId == _nodeViewModel.SelectedNode.NodeId)
                {
                    cmbEdgeEditFrom.SelectedItem = nodeItem;
                }
            }

            btnAddEdge.Content = "Add Edge";
            btnAddEdge.Click += AddEdge;
            btnRemoveEdge.Visibility = Visibility.Hidden;

            Visibility = Visibility.Visible;
        }

        private void AddEdge(object sender, RoutedEventArgs e)
        {
            if (cmbRelation.SelectedItem == null ||
                cmbEdgeEditFrom.SelectedItem == null ||
                cmbEdgeEditTo.SelectedItem == null) return;
            AddEdge();
        }

        private void AddEdge()
        {
            var nodeFromId = (cmbEdgeEditFrom.SelectedItem as NodeItem)?.NodeId;
            var nodeToId = (cmbEdgeEditTo.SelectedItem as NodeItem)?.NodeId;
            var relation = MapRelationship(cmbRelation.SelectedItem as string);
            var newEdge = _graphManager.AddEdge(nodeFromId, txtEdgeName.Text, nodeToId, relation);

            _nodeViewModel.SelectedEdge = newEdge;
            _nodeViewModel.UpdateNodeDetails();
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

        private void OnEdgeSelected(Edge edge)
        {
            var nodes = _nodeViewModel.AllNodeItems;
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
            foreach (var nodeItem in nodes)
            {
                cmbEdgeEditFrom.Items.Add(nodeItem);
                cmbEdgeEditTo.Items.Add(nodeItem);
            }

            txtEdgeName.Text = edge.LabelText;

            SelectNodeInComboBox(cmbEdgeEditFrom, edge.SourceNode);
            SelectNodeInComboBox(cmbEdgeEditTo, edge.TargetNode);
            SelectRelationshipInComboBox(edge.Relation);

            btnAddEdge.Content = "Edit Edge";
            btnAddEdge.Click += EditEdge;
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
                    cmbRelation.SelectedItem =  Friend;
                    break;
                case Edge.Relationship.Neutral:
                    cmbRelation.SelectedItem =  Neutral;
                    break;
                case Edge.Relationship.Enemy:
                    cmbRelation.SelectedItem =  Enemy;
                    break;
                case Edge.Relationship.War:
                    cmbRelation.SelectedItem =  War;
                    break;
            }
        }

        private void EditEdge(object sender, RoutedEventArgs e)
        {
            var selectedEdge = _nodeViewModel.SelectedEdge;

            if (selectedEdge == null) return;

            if (selectedEdge.LabelText != txtEdgeName.Text)
            {
                selectedEdge.LabelText = txtEdgeName.Text;
            }

            if (cmbEdgeEditFrom.SelectedItem != null)
            {
                var edgeFrom = cmbEdgeEditFrom.SelectedItem as NodeItem;
                if (selectedEdge.SourceId != edgeFrom?.NodeId)
                {
                    selectedEdge.SourceId = edgeFrom?.NodeId;
                }
            }

            if (cmbEdgeEditTo.SelectedItem != null)
            {
                var edgeTarget = cmbEdgeEditTo.SelectedItem as NodeItem;
                if (selectedEdge.TargetId != edgeTarget?.NodeId)
                {
                    selectedEdge.TargetId = edgeTarget?.NodeId;
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
        }

        private void BtnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            _graphManager.RemoveEdge(_nodeViewModel.SelectedEdge.EdgeId);
            _nodeViewModel.SelectedEdge = null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _nodeViewModel.SelectedEdge = null;
        }

        private void OnEdgeDeselected()
        {
            Visibility = Visibility.Collapsed;

            txtEdgeName.Text = "";
            cmbEdgeEditFrom.Items.Clear();
            cmbEdgeEditTo.Items.Clear();
        }
    }
}