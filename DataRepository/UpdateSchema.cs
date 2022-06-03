using NodeMapper.SqliteDatabase;

namespace NodeMapper.DataRepository
{
    public class UpdateSchema : DbUpdateSchema
    {
        private const long CurrentVersion = 1;

        public override long DatabaseVersion()
        {
            return CurrentVersion;
        }

        protected override void RunDbSchemaUpdates(SqLiteDb db, SqliteDbInterface moduleInterface)
        {
            var localInterface = (DbInterface)moduleInterface;

            var version = db.GetVersion();

            if (version < CurrentVersion)
            {
                db.DropTables(localInterface.GetAllModules());
                db.CreateTables(localInterface.GetAllModules());
            }
        }
    }
}
