using System.Linq;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using NodeMapper.DataRepository;
using NodeMapper.Ui.Main;

namespace NodeMapper.Model
{
    public class GraphProvider
    {
        private static GraphProvider _instance;
        public static GraphProvider Instance => _instance ?? (_instance = new GraphProvider());

        private DbRepository _repo = new DbRepository();

        public Graph Graph
        {
            get;
        }
        
        public GraphProvider()
        {
            Graph = _repo.LoadGraph();
            var settings = new SugiyamaLayoutSettings();
            settings.EdgeRoutingSettings = new EdgeRoutingSettings
                { EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines };
            Graph.LayoutAlgorithmSettings = settings;
            Graph.Attr.LayerDirection = LayerDirection.TB;

            if (!Graph.Nodes.Any())
            {
                CreateBasicGraph();
            }
        }

        private void CreateBasicGraph()
        {
            var nodeVillage = Graph.AddNode("Village");
            var nodeHill = Graph.AddNode("Hill");
            var nodeForest = Graph.AddNode("Forest");

            nodeVillage.UserData = "This is an isolated village.";
            nodeHill.UserData = "A ruined table lies at the top this hill.";
            nodeForest.UserData = "Rumors of a werewolf are connected to this forest.";

            Graph.AddEdge(nodeVillage.Id, "4h", nodeHill.Id);
            Graph.AddEdge(nodeVillage.Id, "4h", nodeForest.Id);
            Graph.AddEdge(nodeForest.Id, "2h", nodeHill.Id);
            Graph.AddEdge(nodeHill.Id, "2h", nodeForest.Id);
        }

        public void SaveGraph()
        {
            _repo.SaveGraph(Graph);
        }
        
        public Node SelectNodeAt(Point point)
        {
            return Graph.Nodes.FirstOrDefault(graphNode => graphNode.BoundingBox.Contains(point));
        }

        public Node CreateNeNodeWithEdgeFrom(Node node)
        {
            var newEdge = Graph.AddEdge(node.Id, "New Node");
            Graph.GeometryGraph.UpdateBoundingBox();
            return newEdge.TargetNode;
        }

        public void RedoBorders()
        {
            // foreach (var node in Graph.Nodes)
            // {
            //     node.Attr.ClearStyles();
            // }
        }
    }
}