using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class InventoryRecord : NotifyPropertyChangeBean
    {
        private string _recordId;
        private string _name;
        private string _chineseName;
        private string _studentNo;
        private string _program;
        private string _title;
        private string _gradYear;
        private string _degree;
        private string _access;
        private int _boxNo;
        private string _assign;
        private string _received;
        private string _approvalDate;
        private string _embargo;
        private string _release;

        public string RecordId
        {
            get { return _recordId; }
            set
            {
                if (value != _recordId)
                {
                    _recordId = value;
                    OnPropertyChanged("RecordId");
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string ChineseName
        {
            get { return _chineseName; }
            set
            {
                if (value != _chineseName)
                {
                    _chineseName = value;
                    OnPropertyChanged("ChineseName");
                }
            }
        }
        public string StudentNo
        {
            get { return _studentNo; }
            set
            {
                if (value != _studentNo)
                {
                    _studentNo = value;
                    OnPropertyChanged("StudentNo");
                }
            }
        }
        public string Program
        {
            get { return _program; }
            set
            {
                if (value != _program)
                {
                    _program = value;
                    OnPropertyChanged("Program");
                }
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public string GradYear
        {
            get { return _gradYear; }
            set
            {
                if (value != _gradYear)
                {
                    _gradYear = value;
                    OnPropertyChanged("GradYear");
                }
            }
        }
        public string Degree
        {
            get { return _degree; }
            set
            {
                if (value != _degree)
                {
                    _degree = value;
                    OnPropertyChanged("Degree");
                }
            }
        }
        public string Access
        {
            get { return _access; }
            set
            {
                if (value != _access)
                {
                    _access = value;
                    OnPropertyChanged("Access");
                }
            }
        }
        public int BoxNo
        {
            get { return _boxNo; }
            set
            {
                if (value != _boxNo)
                {
                    _boxNo = value;
                    OnPropertyChanged("BoxNo");
                }
            }
        }
        public string Assign
        {
            get { return _assign; }
            set
            {
                if (value != _assign)
                {
                    _assign = value;
                    OnPropertyChanged("Assign");
                }
            }
        }
        public string Received
        {
            get { return _received; }
            set
            {
                if (value != _received)
                {
                    _received = value;
                    OnPropertyChanged("Received");
                }
            }
        }
        public string ApprovalDate
        {
            get { return _approvalDate; }
            set
            {
                if (value != _approvalDate)
                {
                    _approvalDate = value;
                    OnPropertyChanged("ApprovalDate");
                }
            }
        }
        public string Embargo
        {
            get { return _embargo; }
            set
            {
                if (value != _embargo)
                {
                    _embargo = value;
                    OnPropertyChanged("Embargo");
                }
            }
        }
        public string Release
        {
            get { return _release; }
            set
            {
                if (value != _release)
                {
                    _release = value;
                    OnPropertyChanged("Release");
                }
            }
        }

        public string DisplayString
        {
            get { return this.StudentNo + "|" + this.ChineseName + "|" + this.Name + "|" + this.Degree + "|" + this.Program + "|" + this.GradYear; }
        }
    }
}
