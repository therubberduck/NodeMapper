namespace BitD_FactionMapper.Model
{
    public class Edge
    {
        public enum Relationship { Ally = 6, Friend = 5, Neutral = 4, Enemy = 3, War = 2}
        
        private readonly NodeDataManager _nodeDataManager = NodeDataManager.Instance;

        public string EdgeId { get; }

        public string LabelText { get; set; }

        public string SourceId { get; set; }

        public string TargetId { get; set; }

        public Node SourceNode => _nodeDataManager.GetNode(SourceId);
        public Node TargetNode => _nodeDataManager.GetNode(TargetId);

        public Relationship Relation { get; set; }

        public Edge(string edgeId, string sourceId, string targetId, string labelText, Relationship relation)
        {
            EdgeId = edgeId;
            SourceId = sourceId;
            TargetId = targetId;
            LabelText = labelText;
            Relation = relation;
        }
    }
}