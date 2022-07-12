using System.Collections.Generic;
using NodeMapper.Model;
using Edge = NodeMapper.Model.Edge;

namespace NodeMapper.DataRepository
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
    }
}
