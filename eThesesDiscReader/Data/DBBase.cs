using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Data
{
    public class DBBase
    {
        protected static DataTable query(string sql, IDBResultHandler dbResultHandler)
        {
            DataTable result = null;
            using (SQLiteConnection conn = getConnection())
            {
                try
                {
                    conn.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        SQLiteDataReader reader = command.ExecuteReader();
                        result = dbResultHandler.handle(reader);
                    }
                }
                catch (SQLiteException sle)
                {
                    DebugConsole.WriteLine(sle.Message + Environment.NewLine + sle.StackTrace + Environment.NewLine + ReaderConfig.getInstance().DBPath);
                }
                finally
                {
                    if (conn != null)
                    {
                        try
                        {
                            conn.Close();
                        }
                        catch (SQLiteException sle)
                        {
                            DebugConsole.WriteLine(sle.Message + Environment.NewLine + sle.StackTrace);
                        }
                    }
                }
            }
            return result;
        }

        protected static int insert(string sql, DataTable insertDataTable, IDBInsertHandler dbInsertHandler)
        {
            using (SQLiteConnection conn = getConnection())
            {
                int updatedRowCount = 0;
                try
                {
                    conn.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        dbInsertHandler.handle(command, insertDataTable);
                        updatedRowCount = command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException sle)
                {
                    DebugConsole.WriteLine(sle.Message + Environment.NewLine + sle.StackTrace + Environment.NewLine + ReaderConfig.getInstance().DBPath);
                }
                finally
                {
                    if (conn != null)
                    {
                        try
                        {
                            conn.Close();
                        }
                        catch (SQLiteException sle)
                        {
                            DebugConsole.WriteLine(sle.Message + Environment.NewLine + sle.StackTrace);
                        }
                    }
                }
                return updatedRowCount;
            }
        }

        protected static SQLiteConnection getConnection()
        {
            string dbFilePath = ReaderConfig.getInstance().DBPath;
            if (dbFilePath != null && File.Exists(dbFilePath))
                return new SQLiteConnection(@"Data Source=" + dbFilePath + @";Version=3;");
            return null;
        }

    }
}
