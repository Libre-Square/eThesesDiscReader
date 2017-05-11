using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Data
{
    public interface IDBResultHandler
    {
        DataTable handle(SQLiteDataReader reader);
    }

    public interface IDBInsertHandler
    {
        void handle(SQLiteCommand command, DataTable insertDataTable);
    }
}
