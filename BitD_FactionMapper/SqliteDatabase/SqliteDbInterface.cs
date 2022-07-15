using System;

namespace NodeMapper.SqliteDatabase
{
    public abstract class SqliteDbInterface
    {
        public SqLiteDb Db { get; }

        protected SqliteDbInterface(string dbPath, Type updateSchemaClass)
        {
            Db = new SqLiteDb(dbPath);

            InitModules(Db);

            DbUpdateSchema updateSchema = (DbUpdateSchema)Activator.CreateInstance(updateSchemaClass);
            updateSchema.Init(Db, this);
            updateSchema.CheckForDbSchemaUpdates();
        }

        protected abstract void InitModules(SqLiteDb db);

        public abstract IDbModule[] GetAllModules();
    }
}
