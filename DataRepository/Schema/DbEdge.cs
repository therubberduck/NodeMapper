using System.Collections.Generic;
using NodeMapper.Model;
using NodeMapper.SqliteDatabase;

namespace NodeMapper.DataRepository.Schema
{
    public class DbEdge : DbObjectModule<Edge>
    {
        public DbEdge(SqliteDbInterface dbInterface, SqLiteDb db) : base(dbInterface, db)
        {
        }

        public override string TableName => "Edge";

        public override DbColumn[] AllColumns => new[]
        {
            new DbColumn(Id, DbColumn.Integer, true),
            new DbColumn(EdgeId, DbColumn.Text),
            new DbColumn(SourceId, DbColumn.Text),
            new DbColumn(TargetId, DbColumn.Text),
            new DbColumn(LabelText, DbColumn.Text),
        };

        public const string EdgeId = "edgeId";
        public const string SourceId = "sourceId";
        public const string TargetId = "targetId";
        public const string LabelText = "LabelText";

        public void InsertAll(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                Insert(edge);
            }
        }
        
        public long Insert(Edge edge)
        {
            var edgeId = edge.EdgeId;
            var sourceId = edge.SourceId;
            var targetId = edge.TargetId;
            string labelText = edge.LabelText;

            return Db.Insert(TableName, new[]
            {
                EdgeId, SourceId, TargetId, LabelText
            }, new object[]
            {
                edgeId, sourceId, targetId, labelText
            });
        }

        protected override Edge MakeObject(object[] dbObject)
        {
            var reader = new DbResultReader(dbObject);
            reader.ReadLong();
            var edgeId = reader.ReadString();
            var sourceId = reader.ReadString();
            var targetId = reader.ReadString();
            var labelText = reader.ReadString();

            var edge = new Edge(edgeId, sourceId, targetId, labelText);

            return edge;
        }
    }
}
