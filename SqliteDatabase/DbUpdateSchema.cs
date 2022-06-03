namespace NodeMapper.SqliteDatabase
{
    public abstract class DbUpdateSchema
    {
        private SqLiteDb _db;
        private SqliteDbInterface _moduleInterface;

        public void Init(SqLiteDb db, SqliteDbInterface moduleInterface)
        {
            _db = db;
            _moduleInterface = moduleInterface;
        }

        public abstract long DatabaseVersion();

        public long GetCurrentDatabaseVersion()
        {
            return _db.GetVersion();
        }

        public void CheckForDbSchemaUpdates()
        {
            RunDbSchemaUpdates(_db, _moduleInterface);
            
            _db.SetVersion(DatabaseVersion());
        }

        protected abstract void RunDbSchemaUpdates(SqLiteDb db, SqliteDbInterface moduleInterface);
    }
}
