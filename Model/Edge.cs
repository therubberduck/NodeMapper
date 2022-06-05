using Microsoft.Msagl.Drawing;

namespace NodeMapper.Model
{
    public class Edge
    {
        private readonly GraphProvider _graphProvider = GraphProvider.Instance;

        public delegate void LabelTextChangedDelegate(string newText);

        public LabelTextChangedDelegate LabelTextChanged;

        public delegate void SourceIdChangedDelegate(string newId);

        public SourceIdChangedDelegate SourceIdChanged;

        public delegate void TargetIdChangedDelegate(string newId);

        public TargetIdChangedDelegate TargetIdChanged;

        private string _labelText;
        private string _sourceId;
        private string _targetId;

        public string EdgeId { get; }

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                LabelTextChanged(value);
            }
        }

        public string SourceId
        {
            get => _sourceId;
            set
            {
                _sourceId = value;
                SourceIdChanged(value);
            }
        }

        public string TargetId
        {
            get => _targetId;
            set
            {
                _targetId = value;
                TargetIdChanged(value);
            }
        }

        public Node SourceNode => _graphProvider.GetNode(_sourceId);
        public Node TargetNode => _graphProvider.GetNode(_targetId);
        
        public Edge(string edgeId, string sourceId, string targetId, string labelText)
        {
            EdgeId = edgeId;
            _sourceId = sourceId;
            _targetId = targetId;
            _labelText = labelText;
        }
    }
}