using System;
using NodeMapper.DataRepository.Schema;
using NodeMapper.SqliteDatabase;

namespace NodeMapper.DataRepository
{
    public class DbInterface : SqliteDbInterface
    {
        public DbEdge Edge;
        public DbNode Node;

        public static DbInterface GetTestInterface()
        {
            return new DbInterface("test.sql", typeof(UpdateSchema));
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
