using eThesesDiscReader.Controls;
using eThesesDiscReader.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Data
{
    public class DBEtdRecordInsertHandler : IDBInsertHandler
    {
        public void handle(SQLiteCommand command, DataTable insertDataTable)
        {
            int rowCount = 0;
            foreach (DataRow row in insertDataTable.Rows)
            {
                foreach (DataColumn column in insertDataTable.Columns)
                {
                    string paramName = "@" + column.ColumnName + rowCount;

                    DbType dbType = DbType.String;
                    if (typeof(int).Equals(column.DataType))
                        dbType = DbType.Int32;
                    if (typeof(DateTime).Equals(column.DataType))
                        dbType = DbType.DateTime;

                    SQLiteParameter parameter = new SQLiteParameter(paramName, dbType);
                    parameter.Value = row[column.ColumnName];
                    command.Parameters.Add(parameter);
                }
                rowCount++;
            }
        }
    }

    public class DBEtdRecords : DBBase
    {
        public static int insertEtdRecords(List<ThesisRecord> recordList)
        {
            DataTable insertDataTable = new DataTable();
            insertDataTable.Columns.Add("RECORD_ID", typeof(string));
            insertDataTable.Columns.Add("CREATE_DATE", typeof(DateTime));
            insertDataTable.Columns.Add("CHINESE_NAME", typeof(string));
            insertDataTable.Columns.Add("ENGLISH_NAME", typeof(string));
            insertDataTable.Columns.Add("STUDENT_ID", typeof(string));
            insertDataTable.Columns.Add("DEGREE", typeof(string));
            insertDataTable.Columns.Add("YEAR", typeof(string));
            insertDataTable.Columns.Add("PROGRAMME", typeof(string));
            insertDataTable.Columns.Add("THESIS_TITLE", typeof(string));
            insertDataTable.Columns.Add("THESIS_CHI_TITLE", typeof(string));
            insertDataTable.Columns.Add("THESIS_CHI_ABSTR", typeof(string));
            insertDataTable.Columns.Add("THESIS_ENG_ABSTR", typeof(string));
            insertDataTable.Columns.Add("ADVISOR_1", typeof(string));
            insertDataTable.Columns.Add("ADVISOR_2", typeof(string));
            insertDataTable.Columns.Add("ADVISOR_3", typeof(string));
            insertDataTable.Columns.Add("ADVISOR_4", typeof(string));
            insertDataTable.Columns.Add("THESIS_LANG", typeof(string));
            insertDataTable.Columns.Add("ACCESS_MODE", typeof(string));
            insertDataTable.Columns.Add("RECEIVED_DATE", typeof(DateTime));
            insertDataTable.Columns.Add("FILE_PATH", typeof(string));
            foreach (ThesisRecord record in recordList)
            {
                if (record != null && !String.IsNullOrWhiteSpace(record.RecordId))
                    insertDataTable.Rows.Add(record.RecordId, record.CreateDate, record.ChineseName, record.EnglishName, record.StudentId, record.Degree, record.Year, record.Programme, record.ThesisTitle, record.ThesisChineseTitle, record.ThesisChineseAbstract, record.ThesisEnglishAbstract, record.Advisor1, record.Advisor2, record.Advisor3, record.Advisor4, record.ThesisLanguage, record.AccessMode, record.ReceivedDate, record.FilePath);
            }

            if (insertDataTable == null)
                return 0;

            string insertSQL = "INSERT INTO ETD_RECORDS (RECORD_ID, CREATE_DATE, CHINESE_NAME, ENGLISH_NAME, STUDENT_ID, DEGREE, YEAR, PROGRAMME, THESIS_TITLE, THESIS_CHI_TITLE, THESIS_CHI_ABSTR, THESIS_ENG_ABSTR, ADVISOR_1, ADVISOR_2, ADVISOR_3, ADVISOR_4, THESIS_LANG, ACCESS_MODE, RECEIVED_DATE, FILE_PATH) VALUES ";
            int rowCount = 0;
            foreach (DataRow row in insertDataTable.Rows)
            {
                insertSQL += rowCount == 0 ? "(" : ", (";
                int columnCount = 0;
                foreach (DataColumn column in insertDataTable.Columns)
                {
                    insertSQL += columnCount == 0 ? "@" : ", @";
                    insertSQL += column.ColumnName + rowCount.ToString();
                    columnCount++;
                }
                insertSQL += ")";
                rowCount++;
            }
            return insert(insertSQL, insertDataTable, new DBEtdRecordInsertHandler());
        }

        public static string generateRecordId()
        {
            return ReaderConfig.getInstance().InstanceName + DateTime.Now.ToString("yyMMddHHmmss") + "_" + new Random().Next(9);
        }
    }
}


/*
CREATE TABLE "ETD_RECORDS" (
	`RECORD_ID`	TEXT NOT NULL,
	`CREATE_DATE`	TEXT,
	`CHINESE_NAME`	TEXT,
	`ENGLISH_NAME`	TEXT NOT NULL,
	`STUDENT_ID`	TEXT NOT NULL,
	`DEGREE`	TEXT NOT NULL,
	`YEAR`	BLOB NOT NULL,
	`PROGRAMME`	TEXT NOT NULL,
	`THESIS_TITLE`	TEXT NOT NULL,
	`THESIS_CHI_TITLE`	TEXT,
	`THESIS_CHI_ABSTR`	TEXT,
	`THESIS_ENG_ABSTR`	TEXT,
	`ADVISOR_1`	TEXT,
	`ADVISOR_2`	TEXT,
	`ADVISOR_3`	TEXT,
	`ADVISOR_4`	TEXT,
	`THESIS_LANG`	TEXT,
	`ACCESS_MODE`	TEXT NOT NULL,
	`RECEIVED_DATE`	TEXT NOT NULL,
	`FILE_PATH`	TEXT NOT NULL,
	PRIMARY KEY(RECORD_ID)
);
CREATE UNIQUE INDEX etd_records_student_id_thesis_title_year ON etd_records(STUDENT_ID, THESIS_TITLE, YEAR);
*/
