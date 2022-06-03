namespace NodeMapper.SqliteDatabase
{
    public interface IDbModule
    {
        string TableName { get; }
        DbColumn[] AllColumns { get; }
    }
}
