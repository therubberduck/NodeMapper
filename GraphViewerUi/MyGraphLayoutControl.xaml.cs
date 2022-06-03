using System.Windows;
using System.Windows.Input;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace NodeMapper.GraphViewerUi
{
    public partial class MyGraphLayoutControl {
        public readonly GraphViewer GraphViewer = new GraphViewer();
        
        public MyGraphLayoutControl() {
            InitializeComponent();
            Loaded += (s, e) => SetGraph();
            
            GraphViewer.BindToPanel(dockPanel);
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
            GraphViewer.Graph = Graph;
        }
        
        private void SetGraph() {
            GraphViewer.Graph = Graph;
        }
    }
}