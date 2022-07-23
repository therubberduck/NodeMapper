using System.Collections.Generic;
using BitD_FactionMapper.Model;
using Edge = BitD_FactionMapper.Model.Edge;

namespace BitD_FactionMapper.DataRepository
{
    public class DbRepository
    {
        private DbInterface _db;
        private string _filePath;

        public DbRepository()
        {
            _filePath = Properties.Settings.Default.SaveFile;
            if (_filePath != "")
            {
                _db = DbInterface.GetSaveInterface(_filePath);
            }
        }

        public bool RepoInitialized()
        {
            return _filePath != "";
        }

        public NodeEdgesResult Load(string fileName)
        {
            _db = DbInterface.GetSaveInterface(fileName);
            return Load();
        }

        public NodeEdgesResult Load()
        {
            var nodes = _db.Node.GetAll();
            var edges = _db.Edge.GetAll();
            return new NodeEdgesResult(nodes, edges);
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
            _db = DbInterface.GetSaveInterface(fileName);
            SaveGraph(nodes, edges);
        }
    }
}
