using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
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
            new DbColumn(EdgeColor, DbColumn.Text),
            new DbColumn(ShapeAtSource, DbColumn.Integer),
            new DbColumn(ShapeAtTarget, DbColumn.Integer),
            new DbColumn(LabelText, DbColumn.Text),
            new DbColumn(FontColor, DbColumn.Text),
            new DbColumn(FontSize, DbColumn.Real),
        };

        public const string EdgeId = "edgeId";
        public const string SourceId = "sourceId";
        public const string TargetId = "targetId";
        public const string EdgeColor = "Color";
        public const string ShapeAtSource = "ArrowheadAtSource";
        public const string ShapeAtTarget = "ArrowheadAtTarget";
        public const string LabelText = "LabelText";
        public const string FontColor = "FontColor";
        public const string FontSize = "FontSize";

        public void InsertAll(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
            {
                Insert(edge);
            }
        }
        
        public long Insert(Edge edge)
        {
            var edgeId = edge.Attr.Id;
            var sourceId = edge.Source;
            var targetId = edge.Target;
            var edgeColor = DbColorConverter.DbStringFromColor(edge.Attr.Color);
            var shapeAtSource = (int) edge.Attr.ArrowheadAtSource;
            var shapeAtTarget = (int) edge.Attr.ArrowheadAtTarget;
            string labelText, fontColor;
            double fontSize;
            if (edge.Label != null)
            {
                labelText = edge.Label.Text;     
                fontColor = DbColorConverter.DbStringFromColor(edge.Label.FontColor);
                fontSize = edge.Label.FontSize;           
            }
            else
            {
                labelText = "";
                fontColor = DbColorConverter.DbStringFromColor(Color.Black);
                fontSize = 8;
            }

            return Db.Insert(TableName, new[]
            {
                EdgeId, SourceId, TargetId, EdgeColor, ShapeAtSource, ShapeAtTarget, LabelText, FontColor, FontSize
            }, new object[]
            {
                edgeId, sourceId, targetId, edgeColor, shapeAtSource, shapeAtTarget, labelText, fontColor, fontSize
            });
        }

        protected override Edge MakeObject(object[] dbObject)
        {
            var reader = new DbResultReader(dbObject);
            reader.ReadLong();
            var edgeId = reader.ReadString();
            var sourceId = reader.ReadString();
            var targetId = reader.ReadString();
            var edgeColorString = reader.ReadString();
            var edgeColor = DbColorConverter.ColorFromDbString(edgeColorString);
            var shapeAtSource = reader.ReadInt();
            var shapeAtTarget = reader.ReadInt();
            var labelText = reader.ReadString();
            var fontColorString = reader.ReadString();
            var fontColor = DbColorConverter.ColorFromDbString(fontColorString);
            var fontSize = reader.ReadFloat();

            var edge = new Edge(sourceId, labelText, targetId);
            edge.Attr.Id = edgeId;
            edge.Attr.Color = edgeColor;
            edge.Attr.ArrowheadAtSource = (ArrowStyle)shapeAtSource;
            edge.Attr.ArrowheadAtTarget = (ArrowStyle)shapeAtTarget;
            if (edge.Label != null)
            {
                edge.Label.FontColor = fontColor;
                edge.Label.FontSize = fontSize;
            }

            return edge;
        }
    }
}
