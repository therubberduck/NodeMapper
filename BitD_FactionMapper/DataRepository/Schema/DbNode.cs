using System.Collections.Generic;
using BitD_FactionMapper.Model;
using NodeMapper.SqliteDatabase;

namespace BitD_FactionMapper.DataRepository.Schema
{
    public class DbNode : DbObjectModule<Node>
    {
        public DbNode(SqliteDbInterface dbInterface, SqLiteDb db) : base(dbInterface, db)
        {
        }

        public override string TableName => "Node";

        public override DbColumn[] AllColumns => new[]
        {
            new DbColumn(Id, DbColumn.Integer, true),
            new DbColumn(NodeId, DbColumn.Text),
            new DbColumn(Title, DbColumn.Text),
            new DbColumn(Body, DbColumn.Text),
        };

        public const string NodeId = "NodeId";
        public const string Title = "Title";
        public const string Body = "Body";

        public void InsertAll(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                Insert(node);
            }
        }

        private long Insert(Node node)
        {
            var nodeId = node.NodeId;
            var labelText = node.Title;
            var userData = node.Body;
        
            return Db.Insert(TableName, new[]
            {
                NodeId, Title, Body
            }, new object[]
            {
                nodeId, labelText, userData
            });
        }

        protected override Node MakeObject(object[] dbObject)
        {
            var reader = new DbResultReader(dbObject);
            reader.ReadLong();
            var nodeId = reader.ReadString();
            var title = reader.ReadString();
            var body = reader.ReadString();

            var node = new Node(nodeId, title, body);
            
            return node;
        }
    }
}
