using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace BitD_FactionMapper.Model
{
    public class Node
    {
        private readonly GraphManager _graphManager = GraphManager.Instance;
        
        public delegate void TitleChangedDelegate(string title);
        public TitleChangedDelegate TitleChanged;

        public delegate void BodyChangedDelegate(string body);
        public BodyChangedDelegate BodyChanged;

        public delegate void FillColorChangedDelegate(Color fillColor);
        public FillColorChangedDelegate FillColorChanged;
        
        private string _title;
        private string _body;
        private Color _fillColor;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                TitleChanged?.Invoke(value);
            }
        }

        public string Body
        {
            get => _body;
            set
            {
                _body = value;
               BodyChanged?.Invoke(value);
            }
        }

        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
               FillColorChanged?.Invoke(value);
            }
        }

        public IEnumerable<Edge> Edges => _graphManager.GetEdgesForNode(NodeId);

        public string NodeId { get; }
        public Node(string nodeId, string title, string body)
        {
            NodeId = nodeId;
            _title = title;
            _body = body;
            _fillColor = Color.White;
        }
    }
}