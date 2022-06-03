using System.Windows;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace NodeMapper.GraphViewerUi
{
    public partial class MyGraphLayoutControl {
        public GraphViewer GraphViewer;
        
        public MyGraphLayoutControl() {
            InitializeComponent();
            Loaded += (s, e) => SetGraph();
        }
        public Graph Graph {
            get => (Graph)GetValue(GraphProperty);
            set => SetValue(GraphProperty, value);
        }
        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Graph", typeof(Graph), typeof(MyGraphLayoutControl), new PropertyMetadata(default(Graph),
                (d,e)=> ((MyGraphLayoutControl)d)?.SetGraph()));

        public void Update()
        {
            var graph = Graph;
            Graph = null;
            Graph = graph;
        }
        
        private void SetGraph() {
            if (Graph == null) {
                dockPanel.Children.Clear();
                GraphViewer = null;
                return;
            }
            if (GraphViewer == null) {
                GraphViewer = new GraphViewer();
                GraphViewer.BindToPanel(dockPanel);
            }
            GraphViewer.Graph = Graph;
        }
    }
}