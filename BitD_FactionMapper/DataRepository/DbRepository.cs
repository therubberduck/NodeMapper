using System.Collections.Generic;
using BitD_FactionMapper.Model;
using Edge = BitD_FactionMapper.Model.Edge;

namespace BitD_FactionMapper.DataRepository
{
    public class DbRepository
    {
        private DbInterface _db;

        public DbRepository()
        {
            #if DEBUG
                _db = DbInterface.GetDevInterface();
            #else
                _db = DbInterface.GetProdInterface();
            #endif
        }

        public DbNodeEdgesResult Load(string fileName)
        {
            var saveDb = DbInterface.GetSaveInterface(fileName);
            var nodes = saveDb.Node.GetAll();
            var edges = saveDb.Edge.GetAll();
            return new DbNodeEdgesResult(nodes, edges);
        }

        public List<Node> LoadNodes()
        {
            return _db.Node.GetAll();
        }

        public IEnumerable<Edge> LoadEdges()
        {
            return _db.Edge.GetAll();
        }

        public void SaveGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges)
        {
            _db.Node.ClearTable();
            _db.Edge.ClearTable();
            _db.Node.InsertAll(nodes);
            _db.Edge.InsertAll(edges);
        }
        
        public void SaveGraph(IEnumerable<Node> nodes, IEnumerable<Edge> edges, string fileName)
        {
            var saveDb = DbInterface.GetSaveInterface(fileName);
            saveDb.Node.ClearTable();
            saveDb.Edge.ClearTable();
            saveDb.Node.InsertAll(nodes);
            saveDb.Edge.InsertAll(edges);
        }

        public struct DbNodeEdgesResult
        {
            public readonly List<Node> Nodes;
            public readonly List<Edge> Edges;

            public DbNodeEdgesResult(List<Node> nodes, List<Edge> edges)
            {
                Nodes = nodes;
                Edges = edges;
            }
        }
    }
}
