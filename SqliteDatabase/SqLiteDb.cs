using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace NodeMapper.SqliteDatabase
{
    public class SqLiteDb
    {
        private SQLiteConnection _conn;

        public SqLiteDb(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            _conn = new SQLiteConnection("Data Source=" + dbPath + ";Version=3");
            _conn.Open();
        }

        public void LoadDb(string dbPath)
        {
            _conn.Close();
            _conn = new SQLiteConnection("Data Source=" + dbPath + ";Version=3");
            _conn.Open();
        }

        public void ReloadDb()
        {
            _conn.Close();
            _conn.Open();
        }

        public long GetVersion()
        {
            var sql = "PRAGMA user_version";
            object[] result = ExecuteQuery(sql);
            return (long)((object[])result[0])[0];
        }

        public void SetVersion(long version)
        {
            var sql = "PRAGMA user_version = " + version;
            ExecuteCommand(sql);
        }

        public void CreateTables(IDbModule[] modules)
        {
            foreach (IDbModule module in modules)
            {
                var columns = "";
                foreach (var column in module.AllColumns)
                {
                    columns += ", " + column.Name + " " + column.Type;
                    if (column.IsPrimaryKey)
                    {
                        columns += " PRIMARY KEY";
                    }
                }
                columns = columns.Substring(2);

                var sql = "CREATE TABLE IF NOT EXISTS " + module.TableName + "(" + columns + ")";
                var cmd = new SQLiteCommand(sql, _conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void DropTables(IDbModule[] modules)
        {
            foreach (IDbModule module in modules)
            {
                var sql = "DROP TABLE IF EXISTS " + module.TableName;
                var cmd = new SQLiteCommand(sql, _conn);
                cmd.ExecuteNonQuery();
            }
        }

        public object[] Select(string table, string[] columns)
        {
            string getCols = MakeColumnString(columns);

            string commandString = "Select " + getCols + " from " + table;
            object[] result = ExecuteQuery(commandString);
            if (result == null)
            {
                throw new Exception("No data retrieved from database.");
            }
            return result;
        }

        public object[] Select(string table, string[] columns, string otherClauses)
        {
            string getCols = MakeColumnString(columns);

            string commandString = "Select " + getCols + " from " + table + " " + otherClauses;
            object[] result = ExecuteQuery(commandString);
            if (result == null)
            {
                throw new Exception("No data retrieved from database.");
            }
            return result;
        }

        public object[] Select(string table, string[] columns, string whereColumn, object whereValue)
        {
            string getCols = MakeColumnString(columns);

            if (whereValue is string)
            {
                whereValue = "'" + whereValue + "'";
            }
            string commandString = "Select " + getCols + " from " + table + " where " + whereColumn + "=" + whereValue;
            object[] result = ExecuteQuery(commandString);
            if (result == null)
            {
                throw new Exception("No data retrieved from database.");
            }
            return result;
        }

        public object[] Select(string table, string[] columns, string[] whereColumns, object[] whereValues)
        {
            string getCols = MakeColumnString(columns);
            string whereString = MakeWhereString(whereColumns, whereValues);
            string commandString = "Select " + getCols + " from " + table + " where " + whereString;
            object[] result = ExecuteQuery(commandString);
            if (result == null)
            {
                throw new Exception("No data retrieved from database.");
            }
            return result;
        }

        public long Insert(string table, string column, string value)
        {
            string commandString = "INSERT INTO " + table + " ([" + column + "]) VALUES ('" + value + "')";

            long id = ExecuteInsertCommand(commandString);
            return id;
        }

        public long Insert(string table, DbColumn[] columns, object[] values)
        {
            string[] columnNames = columns.Select(column => column.Name).ToArray();
            return Insert(table, columnNames, values);
        }

        public long Insert(string table, string[] columns, object[] values)
        {
            string columnsString = MakeColumnString(columns);
            string valuesString = MakeValueString(values);

            string commandString = "INSERT INTO " + table + " (" + columnsString + ") VALUES (" + valuesString + ")";

            long id = ExecuteInsertCommand(commandString);
            return id;
        }

        public void Update(string table, string column, object value, string otherClauses)
        {
            value = FormatValueAsString(value);
            string commandString = "Update " + table + " set " + column + "=" + value + " " + otherClauses;
            ExecuteCommand(commandString);
        }

        public void Update(string table, string column, object value, string whereColumn, object whereValue)
        {
            Update(table, new[]{column}, new[]{value}, new[] { whereColumn }, new[] { whereValue});
        }

        public void Update(string table, string column, object value, string[] whereColumns, object[] whereValues)
        {
            Update(table, new[] { column }, new[] { value }, whereColumns, whereValues);
        }

        public void Update(string table, string[] columns, object[] values, string whereColumn, object whereValue)
        {
            Update(table, columns, values, new []{whereColumn}, new[]{whereValue});
        }

        public void Update(string table, string[] columns, object[] values, string[] whereColumns, object[] whereValues)
        {
            string setString = "";
            List<SQLiteParameter> parameters = new List<SQLiteParameter>();
            for (int i = 0; i < columns.Length; i++)
            {
                setString += columns[i] + "=@value" + i + ",";
                SQLiteParameter param = new SQLiteParameter()
                {
                    ParameterName = "@value" + i,
                    Value = values[i]
                };
                parameters.Add(param);
            }
            setString = setString.Remove(setString.Length - 1);

            string whereString = MakeWhereString(whereColumns, whereValues);

            string commandString = "UPDATE " + table + " SET " + setString + " WHERE " + whereString;

            SQLiteCommand cmd = new SQLiteCommand(commandString, _conn);
            foreach (var sqLiteParameter in parameters)
            {
                cmd.Parameters.Add(sqLiteParameter);
            }
            cmd.ExecuteNonQuery();
        }

        public void Delete(string table, string whereColumn, object whereValue)
        {
            if (whereValue is string)
            {
                whereValue = "'" + whereValue + "'";
            }

            string commandString = "Delete from " + table + " where " + whereColumn + "=" + whereValue;
            ExecuteCommand(commandString);
        }

        public void Delete(string table, string[] whereColumns, object[] whereValues)
        {
            string whereString = "";
            for (int i = 0; i < whereColumns.Length; i++)
            {
                object value = whereValues[i];
                if (value is string)
                {
                    value = "'" + value + "'";
                }
                whereString += whereColumns[i] + "=" + value + " AND ";
            }
            whereString = whereString.Remove(whereString.Length - 5);

            Delete(table, whereString);
        }

        public void Delete(string table, string whereString)
        {
            string commandString = "Delete from " + table + " where " + whereString;
            ExecuteCommand(commandString);
        }

        public void AlterAddColumn(string table, string columnName, string columnType, bool notNull, string defaultValue)
        {
            var sql = "ALTER TABLE " + table + " ADD " + columnName + " " + columnType;
            if (notNull)
            {
                sql += " NOT NULL";
            }
            if (defaultValue != null)
            {
                sql += " DEFAULT " + defaultValue;
            }
            ExecuteCommand(sql);
        }

        public void ClearTable(string table)
        {
            string commandString = "Delete from " + table;
            ExecuteCommand(commandString);
        }

        private static string MakeColumnString(string[] columns)
        {
            string columnString = "";
            foreach (string column in columns)
            {
                columnString += column + ",";
            }
            return columnString.Remove(columnString.Length - 1);
        }

        private static string MakeValueString(object[] values)
        {
            string valuesString = "";
            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];
                value = FormatValueAsString(value);
                valuesString += value + ",";
            }
            return valuesString.Remove(valuesString.Length - 1);
        }

        private static string MakeSetString(string[] columns, object[] values)
        {
            string setString = "";
            for (int i = 0; i < columns.Length; i++)
            {
                object value = values[i];
                value = FormatValueAsString(value);
                setString += columns[i] + "=" + value + ",";
            }
            setString = setString.Remove(setString.Length - 1);

            return setString;
        }

        private static string MakeWhereString(string[] columns, object[] values)
        {
            string whereString = "";
            for (int i = 0; i < values.Length; i++)
            {
                object value = values[i];
                value = FormatValueAsString(value);
                whereString += columns[i] + "=" + value + " AND ";
            }
            return whereString.Remove(whereString.Length - 5);
        }

        private static object FormatValueAsString(object value)
        {
            if (value is string)
            {
                value = "'" + value + "'";
            }
            if (value is null)
            {
                value = "NULL";
            }
            return value;
        }

        private void ExecuteCommand(string commandString)
        {
            var cmd = new SQLiteCommand(commandString, _conn);
            cmd.ExecuteNonQuery();
        }

        private void ExecuteCommand(string commandString, List<SQLiteParameter> parameters)
        {
            var cmd = new SQLiteCommand(commandString, _conn);
            cmd.Parameters.AddRange(parameters.ToArray());
            cmd.ExecuteNonQuery();
        }

        private long ExecuteInsertCommand(string commandString)
        {
            var cmd = new SQLiteCommand(commandString, _conn);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "select last_insert_rowid()";
            var id = (long)cmd.ExecuteScalar();
            return id;
        }

        private object[] ExecuteQuery(string commandString)
        {
            var cmd = new SQLiteCommand(commandString, _conn);
            var reader = cmd.ExecuteReader();
            object[] result = ReaderToData(reader);
            return result;
        }

        private static object[][] ReaderToData(IDataReader reader)
        {
            var returnList = new List<object[]>();

            while (reader.Read())
            {
                object[] readerArray = new object[reader.FieldCount];

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    readerArray[i] = reader.GetValue(i);
                }
                returnList.Add(readerArray);
            }

            return returnList.ToArray();
        }
    }
}
