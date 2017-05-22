using eThesesDiscReader.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class ThesisRecord : NotifyPropertyChangeBean
    {
        private string _pdfTexts;
        private string _recordId;
        private DateTime? _createDate;
        private string _chineseName;
        private string _englishName;
        private string _studentId;
        private string _degree;
        private string _year;
        private string _programme;
        private string _thesisTitle;
        private string _thesisChineseTitle;
        private string _accessMode;
        private string _filePath;
        private string _fullTextFilePath;

        private string _thesisChineseAbstract;
        private string _thesisEnglishAbstract;
        private string _advisor1;
        private string _advisor2;
        private string _advisor3;
        private string _advisor4;
        private string _thesisLanguage;

        private string _storageFolderPath;
        private bool _accessModeOpen;
        private bool _accessModeRestricted;
        private bool _accessModeConfidential;
        private DateTime? _receivedDate;

        private bool _enableValidation;

        public string PdfTexts
        {
            get { return _pdfTexts; }
            set
            {
                if (value != _pdfTexts)
                {
                    _pdfTexts = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("PdfTexts");
                }
            }
        }

        public string RecordId
        {
            get { return _recordId; }
            set
            {
                if (value != _recordId)
                {
                    _recordId = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("RecordId");
                }
            }
        }

        public DateTime? CreateDate
        {
            get { return _createDate; }
            set
            {
                if (value != _createDate)
                {
                    _createDate = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("CreateDate");
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
                    validateWithSelectedInventory();
                    OnPropertyChanged("ChineseName");
                }
            }
        }

        public string EnglishName
        {
            get { return _englishName; }
            set
            {
                if (value != _englishName)
                {
                    _englishName = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("EnglishName");
                }
            }
        }

        public string StudentId
        {
            get { return _studentId; }
            set
            {
                if (value != _studentId)
                {
                    _studentId = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("StudentId");
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
                    validateWithSelectedInventory();
                    OnPropertyChanged("Degree");
                }
            }
        }

        public string Year
        {
            get { return _year; }
            set
            {
                if (value != _year)
                {
                    _year = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("Year");
                }
            }
        }

        public string Programme
        {
            get { return _programme; }
            set
            {
                if (value != _programme)
                {
                    _programme = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("Programme");
                }
            }
        }

        public string ThesisTitle
        {
            get { return _thesisTitle; }
            set
            {
                if (value != _thesisTitle)
                {
                    _thesisTitle = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("ThesisTitle");
                }
            }
        }

        public string ThesisChineseTitle
        {
            get { return _thesisChineseTitle; }
            set
            {
                if (value != _thesisChineseTitle)
                {
                    _thesisChineseTitle = value;
                    OnPropertyChanged("ThesisChineseTitle");
                }
            }
        }

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (value != _filePath)
                {
                    _filePath = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("FilePath");
                }
            }
        }

        public string FullTextFilePath
        {
            get { return _fullTextFilePath; }
            set
            {
                if (value != _fullTextFilePath)
                {
                    _fullTextFilePath = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("FullTextFilePath");
                }
            }
        }

        public string ThesisChineseAbstract
        {
            get { return _thesisChineseAbstract; }
            set
            {
                if (value != _thesisChineseAbstract)
                {
                    _thesisChineseAbstract = value;
                    OnPropertyChanged("ThesisChineseAbstract");
                }
            }
        }

        public string ThesisEnglishAbstract
        {
            get { return _thesisEnglishAbstract; }
            set
            {
                if (value != _thesisEnglishAbstract)
                {
                    _thesisEnglishAbstract = value;
                    OnPropertyChanged("ThesisEnglishAbstract");
                }
            }
        }

        public string Advisor1
        {
            get { return _advisor1; }
            set
            {
                if (value != _advisor1)
                {
                    _advisor1 = value;
                    OnPropertyChanged("Advisor1");
                }
            }
        }

        public string Advisor2
        {
            get { return _advisor2; }
            set
            {
                if (value != _advisor2)
                {
                    _advisor2 = value;
                    OnPropertyChanged("Advisor2");
                }
            }
        }

        public string Advisor3
        {
            get { return _advisor3; }
            set
            {
                if (value != _advisor3)
                {
                    _advisor3 = value;
                    OnPropertyChanged("Advisor3");
                }
            }
        }

        public string Advisor4
        {
            get { return _advisor4; }
            set
            {
                if (value != _advisor4)
                {
                    _advisor4 = value;
                    OnPropertyChanged("Advisor4");
                }
            }
        }

        public string ThesisLanguage
        {
            get { return _thesisLanguage; }
            set
            {
                if (value != _thesisLanguage)
                {
                    _thesisLanguage = value;
                    OnPropertyChanged("ThesisLanguage");
                }
            }
        }

        public string StorageFolderPath
        {
            get { return _storageFolderPath; }
            set
            {
                if (value != _storageFolderPath)
                {
                    _storageFolderPath = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("StorageFolderPath");
                }
            }
        }

        public string AccessMode
        {
            get { return _accessMode; }
            set
            {
                if (value != _accessMode)
                {
                    _accessMode = value;
                    validateWithSelectedInventory();
                    string accessKey = InventoryStore.AccessModeMap.FirstOrDefault(x => x.Value == _accessMode).Key;
                    this.AccessModeOpen = InventoryStore.ACCESS_MODE_OPEN_KEY.Equals(accessKey);
                    this.AccessModeRestricted = InventoryStore.ACCESS_MODE_RESTRICTED_KEY.Equals(accessKey);
                    this.AccessModeConfidential = InventoryStore.ACCESS_MODE_CONFIDENTIAL_KEY.Equals(accessKey);
                    OnPropertyChanged("AccessMode");
                    OnPropertyChanged("AccessModeOpen");
                    OnPropertyChanged("AccessModeRestricted");
                    OnPropertyChanged("AccessModeConfidential");
                }
            }
        }

        public bool AccessModeOpen
        {
            get { return _accessModeOpen; }
            set
            {
                if (value != _accessModeOpen)
                {
                    _accessModeOpen = value;
                    this.UpdateAccesMode();
                    OnPropertyChanged("AccessModeOpen");
                }
            }
        }

        public bool AccessModeRestricted
        {
            get { return _accessModeRestricted; }
            set
            {
                if (value != _accessModeRestricted)
                {
                    _accessModeRestricted = value;
                    this.UpdateAccesMode();
                    OnPropertyChanged("AccessModeRestricted");
                }
            }
        }

        public bool AccessModeConfidential
        {
            get { return _accessModeConfidential; }
            set
            {
                if (value != _accessModeConfidential)
                {
                    _accessModeConfidential = value;
                    this.UpdateAccesMode();
                    OnPropertyChanged("AccessModeConfidential");
                }
            }
        }

        protected void UpdateAccesMode()
        {
            if (this.AccessModeOpen)
            {
                this._accessModeRestricted = false;
                this._accessModeConfidential = false;
                this._accessMode = InventoryStore.AccessModeMap[InventoryStore.ACCESS_MODE_OPEN_KEY];
            }
            else if (this.AccessModeRestricted)
            {
                this._accessModeOpen = false;
                this._accessModeConfidential = false;
                this._accessMode = InventoryStore.AccessModeMap[InventoryStore.ACCESS_MODE_RESTRICTED_KEY];
            }
            else if (this.AccessModeConfidential)
            {
                this._accessModeOpen = false;
                this._accessModeRestricted = false;
                this._accessMode = InventoryStore.AccessModeMap[InventoryStore.ACCESS_MODE_CONFIDENTIAL_KEY];
            }
            validateWithSelectedInventory();
            OnPropertyChanged("AccessMode");
        }

        public DateTime? ReceivedDate
        {
            get { return _receivedDate; }
            set
            {
                if (value != _receivedDate)
                {
                    _receivedDate = value;
                    validateWithSelectedInventory();
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }

        public bool EnableValidation
        {
            get { return _enableValidation; }
            set { _enableValidation = value;
                if (_enableValidation) validateWithSelectedInventory();
            }
        }

        public void clearExtractedData()
        {
            _enableValidation = false;
            _pdfTexts = "";
            _recordId = "";
            _createDate = null;
            _chineseName = "";
            _englishName = "";
            _studentId = "";
            _degree = "";
            _year = "";
            _programme = "";
            _thesisTitle = "";
            _thesisChineseTitle = "";

            _thesisChineseAbstract = "";
            _thesisEnglishAbstract = "";
            _advisor1 = "";
            _advisor2 = "";
            _advisor3 = "";
            _advisor4 = "";
            _thesisLanguage = "English";

            _accessMode = "";
            _receivedDate = null;

            _filePath = "";
            _fullTextFilePath = "";
            _storageFolderPath = "";
    }

        protected void validateWithSelectedInventory()
        {
            if (!_enableValidation)
                return;

            // Prevent loop
            _enableValidation = false;

            Color EMPTY_VALUE_COLOR = Color.Yellow;
            Color CONFLICT_VALUE_COLOR = Color.Pink;
            Color MATCH_VALUE_COLOR = Color.LightGreen;

            ViewModel viewModel = ViewModel.getInstance();
            viewModel.ChineseNameInputColor = SystemColors.Window;
            viewModel.EnglishNameInputColor = SystemColors.Window;
            viewModel.StudentIdInputColor = SystemColors.Window;
            viewModel.DegreeInputColor = SystemColors.Window;
            viewModel.YearInputColor = SystemColors.Window;
            viewModel.ProgramInputColor = SystemColors.Window;
            viewModel.TitleInputColor = SystemColors.Window;
            viewModel.AccessInputColor = SystemColors.Window;
            viewModel.ReceivedDateInputColor = SystemColors.Window;

            InventoryRecord invRecord = InventoryStore.getInstance().SelectedInventoryRecord;
            if (invRecord == null || String.IsNullOrEmpty(invRecord.RecordId))
                return;
            
            ThesisRecord thesisRecord = MainThread.getInstance().ThesisRecord;
            // Chinese Name
            if (thesisRecord.ChineseName == null)
                thesisRecord.ChineseName = "";
            if (String.IsNullOrEmpty(thesisRecord.ChineseName) && String.IsNullOrEmpty(invRecord.ChineseName))
                viewModel.ChineseNameInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.ChineseName) && !String.IsNullOrEmpty(invRecord.ChineseName))
            {
                thesisRecord.ChineseName = invRecord.ChineseName;
                viewModel.ChineseNameInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.ChineseName) && String.IsNullOrEmpty(invRecord.ChineseName))
                viewModel.ChineseNameInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.ChineseNameInputColor = thesisRecord.ChineseName.Equals(invRecord.ChineseName) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // English Name
            if (thesisRecord.EnglishName == null)
                thesisRecord.EnglishName = "";
            if (String.IsNullOrEmpty(thesisRecord.EnglishName) && String.IsNullOrEmpty(invRecord.Name))
                viewModel.EnglishNameInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.EnglishName) && !String.IsNullOrEmpty(invRecord.Name))
            {
                thesisRecord.EnglishName = invRecord.Name;
                viewModel.EnglishNameInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.EnglishName) && String.IsNullOrEmpty(invRecord.Name))
                viewModel.EnglishNameInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.EnglishNameInputColor = thesisRecord.EnglishName.Equals(invRecord.Name) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Student ID
            if (thesisRecord.StudentId == null)
                thesisRecord.StudentId = "";
            if (String.IsNullOrEmpty(thesisRecord.StudentId) && String.IsNullOrEmpty(invRecord.StudentNo))
                viewModel.StudentIdInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.StudentId) && !String.IsNullOrEmpty(invRecord.StudentNo))
            {
                thesisRecord.StudentId = invRecord.StudentNo;
                viewModel.StudentIdInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.StudentId) && String.IsNullOrEmpty(invRecord.StudentNo))
                viewModel.StudentIdInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.StudentIdInputColor = thesisRecord.StudentId.Equals(invRecord.StudentNo) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Degree
            if (String.IsNullOrEmpty(thesisRecord.Degree))
                viewModel.DegreeInputColor = CONFLICT_VALUE_COLOR;
            else
                viewModel.DegreeInputColor = EMPTY_VALUE_COLOR;

            // Year
            if (thesisRecord.Year == null)
                thesisRecord.Year = "";
            if (String.IsNullOrEmpty(thesisRecord.Year) && String.IsNullOrEmpty(invRecord.GradYear))
                viewModel.YearInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.Year) && !String.IsNullOrEmpty(invRecord.GradYear))
            {
                thesisRecord.Year = invRecord.GradYear;
                viewModel.YearInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.Year) && String.IsNullOrEmpty(invRecord.GradYear))
                viewModel.YearInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.YearInputColor = thesisRecord.Year.Equals(invRecord.GradYear) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Programme
            if (thesisRecord.Programme == null)
                thesisRecord.Programme = "";
            if (String.IsNullOrEmpty(thesisRecord.Programme) && String.IsNullOrEmpty(invRecord.Program))
                viewModel.ProgramInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.Programme) && !String.IsNullOrEmpty(invRecord.Program))
            {
                thesisRecord.Programme = invRecord.Program;
                viewModel.ProgramInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.Programme) && String.IsNullOrEmpty(invRecord.Program))
                viewModel.ProgramInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.ProgramInputColor = thesisRecord.Programme.Equals(invRecord.Program) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Title
            if (thesisRecord.ThesisTitle == null)
                thesisRecord.ThesisTitle = "";
            if (String.IsNullOrEmpty(thesisRecord.ThesisTitle) && String.IsNullOrEmpty(invRecord.Title))
                viewModel.TitleInputColor = CONFLICT_VALUE_COLOR;
            else if (String.IsNullOrEmpty(thesisRecord.ThesisTitle) && !String.IsNullOrEmpty(invRecord.Title))
            {
                thesisRecord.ThesisTitle = invRecord.Title;
                viewModel.TitleInputColor = EMPTY_VALUE_COLOR;
            }
            else if (!String.IsNullOrEmpty(thesisRecord.ThesisTitle) && String.IsNullOrEmpty(invRecord.Title))
                viewModel.TitleInputColor = EMPTY_VALUE_COLOR;
            else
                viewModel.TitleInputColor = thesisRecord.ThesisTitle.Equals(invRecord.Title) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Access
            if (String.IsNullOrEmpty(thesisRecord.AccessMode) && String.IsNullOrEmpty(invRecord.Access))
            {
                thesisRecord.AccessMode = InventoryStore.ACCESS_MODE_RESTRICTED_KEY;
                viewModel.AccessInputColor = CONFLICT_VALUE_COLOR;
            }
            else if (String.IsNullOrEmpty(thesisRecord.AccessMode) && !String.IsNullOrEmpty(invRecord.Access))
            {
                thesisRecord.AccessMode = invRecord.Access;
                viewModel.AccessInputColor = EMPTY_VALUE_COLOR;
            }
            else
                viewModel.AccessInputColor = thesisRecord.AccessMode.Equals(invRecord.Access) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

            // Received Date
            if (invRecord.Received != null)
                thesisRecord.ReceivedDate = DateTime.ParseExact(invRecord.Received, DiscContentsProcessor.DEFAULT_DATE_FORMAT, CultureInfo.InvariantCulture);
            else
                thesisRecord.ReceivedDate = DateTime.Now;
            viewModel.ReceivedDateInputColor = EMPTY_VALUE_COLOR;

            // Prevent loop
            _enableValidation = true;
        }

        public override string ToString()
        {
            return this.ChineseName + "|" + this.EnglishName + "|" + this.StudentId + "|" + this.Degree + "|" + this.Programme + "|" + this.Year +"|" + this.ThesisTitle + "|" + this.AccessMode + "|" + this.ReceivedDate;
        }
    }
}
