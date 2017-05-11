using eThesesDiscReader.Controls;
using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Data
{
    public class DBInventoryRecords : DBBase
    {
        public static DataTable listInventoryItems()
        {
            return query("SELECT * FROM INVENTORY", new DBInventoryRecordResultHandler());
        }
    }

    public class DBInventoryRecordResultHandler : IDBResultHandler
    {
        public static readonly string RECEIVED_DATE_FORMAT = @"M/d/yyyy";
        public static readonly string APPROVAL_DATE_FORMAT = @"d MMMM yyyy";
        private CultureInfo provider = CultureInfo.InvariantCulture;

        public DataTable handle(SQLiteDataReader reader)
        {
            DataTable resultDataTable = new DataTable();
            resultDataTable.Columns.Add("RECORD_ID", typeof(string));
            resultDataTable.Columns.Add("NAME", typeof(string));
            resultDataTable.Columns.Add("C_NAME", typeof(string));
            resultDataTable.Columns.Add("STUDENT_NO", typeof(string));
            resultDataTable.Columns.Add("PROGRAM", typeof(string));
            resultDataTable.Columns.Add("TITLE", typeof(string));
            resultDataTable.Columns.Add("GRAD_YEAR", typeof(string));
            resultDataTable.Columns.Add("DEGREE", typeof(string));
            resultDataTable.Columns.Add("ACCESS", typeof(string));
            resultDataTable.Columns.Add("BOX_NO", typeof(int));
            resultDataTable.Columns.Add("ASSIGN", typeof(string));
            resultDataTable.Columns.Add("RECEIVED", typeof(DateTime));
            resultDataTable.Columns.Add("GCEXCO_APPROVAL_DATE", typeof(DateTime));
            resultDataTable.Columns.Add("EMBARGO", typeof(string));
            resultDataTable.Columns.Add("RELEASE", typeof(string));
            while (reader != null && reader.Read())
            {
                string recordId = (string)reader["RECORD_ID"];
                string name = (string)reader["NAME"];
                string chineseName = (string)reader["C_NAME"];
                string studentNo = (string)reader["STUDENT_NO"];
                string program = (string)reader["PROGRAM"];
                string title = (string)reader["TITLE"];
                string gradYear = (string)reader["GRAD_YEAR"];
                string degree = (string)reader["DEGREE"];
                string access = (string)reader["ACCESS"];
                string assign = (string)reader["ASSIGN"];
                string embargo = (string)reader["EMBARGO"];
                string release = (string)reader["RELEASE"];
                int boxNo = 0;
                DateTime? receivedDate = null;
                DateTime? approvalDate = null;
                try
                {
                    if (!String.IsNullOrEmpty((string)reader["BOX_NO"]))
                        boxNo = Int32.Parse((string)reader["BOX_NO"]);
                    if (!String.IsNullOrEmpty((string)reader["RECEIVED"]))
                        receivedDate = DateTime.ParseExact((String)reader["RECEIVED"], RECEIVED_DATE_FORMAT, provider);
                    if (!String.IsNullOrEmpty((string)reader["GCEXCO_APPROVAL_DATE"]))
                        approvalDate = DateTime.ParseExact((String)reader["GCEXCO_APPROVAL_DATE"], APPROVAL_DATE_FORMAT, provider);
                }
                catch (FormatException e)
                {
                    DebugConsole.WriteLine((string)reader["BOX_NO"]);
                    DebugConsole.WriteLine((string)reader["RECEIVED"]);
                    DebugConsole.WriteLine((string)reader["GCEXCO_APPROVAL_DATE"]);
                    DebugConsole.WriteLine(e.Message);
                    DebugConsole.WriteLine(e.StackTrace);
                }
                resultDataTable.Rows.Add(recordId, name, chineseName, studentNo, program, title, gradYear, degree, access, boxNo, assign, receivedDate, approvalDate, embargo, release);
            }
            return resultDataTable;
        }
    }
}

/*
CREATE TABLE "INVENTORY" (
	`RECORD_ID`	TEXT,
	`NAME`	TEXT,
	`C_NAME`	TEXT,
	`STUDENT_NO`	TEXT,
	`PROGRAM`	TEXT,
	`TITLE`	TEXT,
	`GRAD_YEAR`	TEXT,
	`DEGREE`	TEXT,
	`ACCESS`	TEXT,
	`BOX_NO`	TEXT,
	`ASSIGN`	TEXT,
	`RECEIVED`	TEXT,
	`GCEXCO_APPROVAL_DATE`	TEXT,
	`EMBARGO`	TEXT,
	`RELEASE`	TEXT
)
*/
