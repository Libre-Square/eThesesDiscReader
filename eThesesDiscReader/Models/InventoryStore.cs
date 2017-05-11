using eThesesDiscReader.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Controls
{
    public class InventoryStore
    {
        private static InventoryStore _instance = new InventoryStore();
        private DataTable _inventoryDataTable;
        private InventoryRecord _selectedInventoryRecord = new InventoryRecord();
        public static Dictionary<string, string> AccessModeMap = new Dictionary<string, string>();
        public static readonly string ACCESS_MODE_OPEN_KEY = "O";
        public static readonly string ACCESS_MODE_RESTRICTED_KEY = "R";
        public static readonly string ACCESS_MODE_CONFIDENTIAL_KEY = "C";

        static InventoryStore()
        {
            AccessModeMap.Add("O", "Open Access");
            AccessModeMap.Add("R", "Restricted Access");
            AccessModeMap.Add("C", "Confidential Access");
        }
        
        public static InventoryStore getInstance()
        {
            return _instance;
        }

        public DataTable InventoryDataTable
        {
            get { return _inventoryDataTable; }
            set { _inventoryDataTable = value; }
        }

        public InventoryRecord SelectedInventoryRecord
        {
            get { return this._selectedInventoryRecord; }
        }

        public void updateSelectedInventoryRecord(InventoryRecord selectedInventoryRecord)
        {
            InventoryRecord databindingRecord = InventoryStore.getInstance().SelectedInventoryRecord;
            if (selectedInventoryRecord != null)
            {
                databindingRecord.RecordId = selectedInventoryRecord.RecordId;
                databindingRecord.Name = selectedInventoryRecord.Name;
                databindingRecord.ChineseName = selectedInventoryRecord.ChineseName;
                databindingRecord.StudentNo = selectedInventoryRecord.StudentNo;
                databindingRecord.Program = selectedInventoryRecord.Program;
                databindingRecord.Title = selectedInventoryRecord.Title;
                databindingRecord.GradYear = selectedInventoryRecord.GradYear;
                databindingRecord.Degree = selectedInventoryRecord.Degree;
                if (selectedInventoryRecord.Access != null && !"".Equals(selectedInventoryRecord.Access.Trim()))
                    databindingRecord.Access = AccessModeMap[selectedInventoryRecord.Access];
                databindingRecord.BoxNo = selectedInventoryRecord.BoxNo;
                databindingRecord.Assign = selectedInventoryRecord.Assign;
                databindingRecord.Received = selectedInventoryRecord.Received;
                databindingRecord.ApprovalDate = selectedInventoryRecord.ApprovalDate;
                databindingRecord.Embargo = selectedInventoryRecord.Embargo;
                databindingRecord.Release = selectedInventoryRecord.Release;
            }
            else
            {
                databindingRecord.RecordId = "";
                databindingRecord.Name = "";
                databindingRecord.ChineseName = "";
                databindingRecord.StudentNo = "";
                databindingRecord.Program = "";
                databindingRecord.Title = "";
                databindingRecord.GradYear = "";
                databindingRecord.Degree = "";
                databindingRecord.Access = "";
                databindingRecord.BoxNo = 0;
                databindingRecord.Assign = "";
                databindingRecord.Received = null;
                databindingRecord.ApprovalDate = null;
                databindingRecord.Embargo = "";
                databindingRecord.Release = "";
            }
        }
    }
}
