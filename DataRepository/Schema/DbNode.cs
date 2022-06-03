using System.Collections.Generic;
using GraphMapper.Model;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Drawing;
using NodeMapper.SqliteDatabase;
using DrawingNode = Microsoft.Msagl.Drawing.Node;

namespace NodeMapper.DataRepository.Schema
{
    public class DbNode : DbObjectModule<NodeStore>
    {
        public DbNode(SqliteDbInterface dbInterface, SqLiteDb db) : base(dbInterface, db)
        {
        }

        public override string TableName => "Node";

        public override DbColumn[] AllColumns => new[]
        {
            new DbColumn(Id, DbColumn.Integer, true),
            new DbColumn(NodeId, DbColumn.Text),
            new DbColumn(PosX, DbColumn.Real),
            new DbColumn(PosY, DbColumn.Real),
            new DbColumn(Shape, DbColumn.Integer),
            new DbColumn(FillColor, DbColumn.Text),
            new DbColumn(LabelText, DbColumn.Text),
            new DbColumn(FontColor, DbColumn.Text),
            new DbColumn(FontSize, DbColumn.Real),
        };

        public const string NodeId = "NodeId";
        public const string PosX = "X";
        public const string PosY = "Y";
        public const string Shape = "Shape";
        public const string FillColor = "Color";
        public const string LabelText = "LabelText";
        public const string FontColor = "FontColor";
        public const string FontSize = "FontSize";

        public void InsertAll(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                Insert(node);
            }
        }
        
        public long Insert(Node node)
        {
            var nodeId = node.Id;
            var posX = node.Pos.X;
            var posY = node.Pos.Y;
            var shape = (int) node.Attr.Shape;
            var fillColor = DbColorConverter.DbStringFromColor(node.Attr.FillColor);
            var labelText = node.Label.Text;
            var fontColor = DbColorConverter.DbStringFromColor(node.Label.FontColor);
            var fontSize = node.Label.FontSize;

            return Db.Insert(TableName, new[]
            {
                NodeId, PosX, PosY, Shape, FillColor, LabelText, FontColor, FontSize
            }, new object[]
            {
                nodeId, posX, posY, shape, fillColor, labelText, fontColor, fontSize
            });
        }

        protected override NodeStore MakeObject(object[] dbObject)
        {
            var reader = new DbResultReader(dbObject);
            reader.ReadLong();
            var nodeId = reader.ReadString();
            var posX = reader.ReadFloat();
            var posY = reader.ReadFloat();
            var shapeInt = reader.ReadInt();
            var fillColorString = reader.ReadString();
            var labelText = reader.ReadString();
            var fontColorString = reader.ReadString();
            var fontSize = reader.ReadFloat();

            var point = new Point(posX, posY);
            var shape = (Shape)shapeInt;
            var fillColor = DbColorConverter.ColorFromDbString(fillColorString);
            var fontColor = DbColorConverter.ColorFromDbString(fontColorString);

            var node = new DrawingNode(nodeId);
            node.Attr.Shape = shape;
            node.Attr.FillColor = fillColor;
            node.Label.Text = labelText;
            node.Label.FontColor = fontColor;
            node.Label.FontSize = fontSize;
            
            return new NodeStore(point, node);
        }
    }
}
