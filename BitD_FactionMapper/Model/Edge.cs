namespace BitD_FactionMapper.Model
{
    public class Edge
    {
        public enum Relationship { Ally = 6, Friend = 5, Neutral = 4, Enemy = 3, War = 2}
        
        private readonly GraphManager _graphManager = GraphManager.Instance;

        public delegate void LabelTextChangedDelegate(string newText);
        public LabelTextChangedDelegate LabelTextChanged;

        public delegate void SourceIdChangedDelegate(string newId);
        public SourceIdChangedDelegate SourceIdChanged;

        public delegate void TargetIdChangedDelegate(string newId);
        public TargetIdChangedDelegate TargetIdChanged;

        public delegate void RelationshipChangedDelegate(Relationship relation);
        public RelationshipChangedDelegate RelationshipChanged;

        private string _labelText;
        private string _sourceId;
        private Relationship _relation;
        private string _targetId;

        public string EdgeId { get; }

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                LabelTextChanged?.Invoke(value);
            }
        }

        public string SourceId
        {
            get => _sourceId;
            set
            {
                _sourceId = value;
                SourceIdChanged?.Invoke(value);
            }
        }

        public string TargetId
        {
            get => _targetId;
            set
            {
                _targetId = value;
                TargetIdChanged?.Invoke(value);
            }
        }

        public Node SourceNode => _graphManager.GetNode(_sourceId);
        public Node TargetNode => _graphManager.GetNode(_targetId);

        public Relationship Relation
        {
            get => _relation;
            set
            {
                _relation = value;
                RelationshipChanged?.Invoke(value);
            }
        }
        
        public Edge(string edgeId, string sourceId, string targetId, string labelText, Relationship relation)
        {
            EdgeId = edgeId;
            _sourceId = sourceId;
            _targetId = targetId;
            _labelText = labelText;
            _relation = relation;
        }
    }
}