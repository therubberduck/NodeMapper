using System.Collections.Generic;
using BitD_FactionMapper.Model;
using NodeMapper.SqliteDatabase;

namespace BitD_FactionMapper.DataRepository.Schema
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
            new DbColumn(Relation, DbColumn.Integer),
        };

        private const string EdgeId = "edgeId";
        private const string SourceId = "sourceId";
        private const string TargetId = "targetId";
        private const string LabelText = "LabelText";
        private const string Relation = "Relation";

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
            var labelText = edge.LabelText;
            var relation = (int)edge.Relation;

            return Db.Insert(TableName, new[]
            {
                EdgeId, SourceId, TargetId, LabelText, Relation
            }, new object[]
            {
                edgeId, sourceId, targetId, labelText, relation
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
            var relation = reader.ReadInt();

            var edge = new Edge(edgeId, sourceId, targetId, labelText, (Edge.Relationship)relation);

            return edge;
        }
    }
}
