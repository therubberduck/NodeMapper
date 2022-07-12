using System;
using NodeMapper.DataRepository.Schema;
using NodeMapper.SqliteDatabase;

namespace NodeMapper.DataRepository
{
    public class DbInterface : SqliteDbInterface
    {
        public DbEdge Edge;
        public DbNode Node;

        public static DbInterface GetProdInterface()
        {
            return new DbInterface("nodemapper.sql", typeof(UpdateSchema));
        }

        public static DbInterface GetDevInterface()
        {
            return new DbInterface("db_dev.sql", typeof(UpdateSchema));
        }

        private DbInterface(string dbPath, Type updateSchemaClass) : base(dbPath, updateSchemaClass)
        {
        }

        protected override void InitModules(SqLiteDb db)
        {
            Edge = new DbEdge(this, db);
            Node = new DbNode(this, db);
        }

        public override IDbModule[] GetAllModules()
        {
            return new IDbModule[]{Edge, Node};
        }
    }
}
