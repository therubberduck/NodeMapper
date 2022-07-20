using System;
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
            new DbColumn(NodeId, DbColumn.Integer),
            new DbColumn(Title, DbColumn.Text),
            new DbColumn(Body, DbColumn.Text),
            new DbColumn(FactionType, DbColumn.Text),
        };

        public const string NodeId = "NodeId";
        public const string Title = "Title";
        public const string Body = "Body";
        public const string FactionType = "FactionType";

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
            var factionType = node.FactionType;
        
            return Db.Insert(TableName, new[]
            {
                NodeId, Title, Body, FactionType
            }, new object[]
            {
                nodeId, labelText, userData, factionType
            });
        }

        protected override Node MakeObject(object[] dbObject)
        {
            var reader = new DbResultReader(dbObject);
            reader.ReadLong();
            var nodeId = reader.ReadInt();
            var title = reader.ReadString();
            var body = reader.ReadString();
            var factionTypeString = reader.ReadString();
            Enum.TryParse(factionTypeString, out FactionType factionType);

            var node = new Node(nodeId, title, body, factionType);
            
            return node;
        }
    }
}
