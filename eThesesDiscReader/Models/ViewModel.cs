using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class ViewModel : NotifyPropertyChangeBean
    {
        private static readonly ViewModel _viewModel = new ViewModel();
        private bool _showMenu;
        private bool _menuButtonEnabled;
        private bool _refreshButtonEnabled;
        private bool _showPdfTexts;
        private MaskedFileInfo _expectedThesisFile;
        private MaskedFileInfo _selectedThesisFile;
        private bool _enableContentFileListbox;
        private string _consoleText;

        private Color _ocrBackColor;
        private Color _databaseStatusColor;
        private bool _saveButtonEnabled;
        private Color _saveButtonColor;

        private ColoredProgressBar _progressBar;
        private long _progressPercentage;
        private Color _progressBarColor;

        private Color _chineseNameInputColor;
        private Color _englishNameInputColor;
        private Color _studentIdInputColor;
        private Color _degreeInputColor;
        private Color _yearInputColor;
        private Color _programInputColor;
        private Color _titleInputColor;
        private Color _accessInputColor;
        private Color _receivedDateInputColor;

        private Color _chineseTitleInputColor;
        private Color _chineseAbstractInputColor;
        private Color _englishAbstractInputColor;
        private Color _advisor1InputColor;
        private Color _advisor2InputColor;
        private Color _advisor3InputColor;
        private Color _advisor4InputColor;
        private Color _languageInputColor;

        private bool _chineseNameInputEnabled;
        private bool _englishNameInputEnabled;
        private bool _studentIdInputEnabled;
        private bool _degreeInputEnabled;
        private bool _yearInputEnabled;
        private bool _programInputEnabled;
        private bool _titleInputEnabled;
        private bool _accessInputEnabled;
        private bool _receivedDateInputEnabled;
        private bool _inventoryMatchesInputEnabled;

        private bool _chineseTitleInputEnabled;
        private bool _chineseAbstractInputEnabled;
        private bool _englishAbstractInputEnabled;
        private bool _advisor1InputEnabled;
        private bool _advisor2InputEnabled;
        private bool _advisor3InputEnabled;
        private bool _advisor4InputEnabled;
        private bool _languageInputEnabled;

        public static ViewModel getInstance()
        {
            return _viewModel;
        }

        public bool ShowMenu
        {
            get { return _showMenu; }
            set
            {
                if (value != _showMenu)
                {
                    _showMenu = value;
                    OnPropertyChanged("ShowMenu");
                }
            }
        }

        public bool MenuButtonEnabled
        {
            get { return _menuButtonEnabled; }
            set
            {
                if (value != _menuButtonEnabled)
                {
                    _menuButtonEnabled = value;
                    OnPropertyChanged("MenuButtonEnabled");
                }
            }
        }

        public bool RefreshButtonEnabled
        {
            get { return _refreshButtonEnabled; }
            set
            {
                if (value != _refreshButtonEnabled)
                {
                    _refreshButtonEnabled = value;
                    OnPropertyChanged("RefreshButtonEnabled");
                }
            }
        }

        public bool ShowPdfTexts
        {
            get { return _showPdfTexts; }
            set
            {
                if (value != _showPdfTexts)
                {
                    _showPdfTexts = value;
                    OnPropertyChanged("ShowPdfTexts");
                }
            }
        }

        public MaskedFileInfo ExpectedThesisFile
        {
            get { return _expectedThesisFile; }
            set
            {
                if (value != _expectedThesisFile)
                {
                    _expectedThesisFile = value;
                    OnPropertyChanged("ExpectedThesisFile");
                }
            }
        }

        public MaskedFileInfo SelectedThesisFile
        {
            get { return _selectedThesisFile; }
            set
            {
                if (value != _selectedThesisFile)
                {
                    _selectedThesisFile = value;
                    OnPropertyChanged("SelectedThesisFile");
                }
            }
        }

        public bool EnableContentFileListbox
        {
            get { return _enableContentFileListbox; }
            set
            {
                if (value != _enableContentFileListbox)
                {
                    _enableContentFileListbox = value;
                    OnPropertyChanged("EnableContentFileListbox");
                }
            }
        }

        public string ConsoleText
        {
            get { return _consoleText; }
            set
            {
                if (value != _consoleText)
                {
                    _consoleText = value;
                    OnPropertyChanged("ConsoleText");
                }
            }
        }

        public Color OcrBackColor
        {
            get { return _ocrBackColor; }
            set
            {
                if (value != _ocrBackColor)
                {
                    _ocrBackColor = value;
                    OnPropertyChanged("OcrBackColor");
                }
            }
        }

        public Color DatabaseStatusColor
        {
            get { return _databaseStatusColor; }
            set
            {
                if (value != _databaseStatusColor)
                {
                    _databaseStatusColor = value;
                    OnPropertyChanged("DatabaseStatusColor");
                }
            }
        }

        public bool SaveButtonEnabled
        {
            get { return _saveButtonEnabled; }
            set
            {
                if (value != _saveButtonEnabled)
                {
                    _saveButtonEnabled = value;
                    OnPropertyChanged("SaveButtonEnabled");
                }
            }
        }

        public Color SaveButtonColor
        {
            get { return _saveButtonColor; }
            set
            {
                if (value != _saveButtonColor)
                {
                    _saveButtonColor = value;
                    OnPropertyChanged("SaveButtonColor");
                }
            }
        }

        public long ProgressPercentage
        {
            get { return _progressPercentage; }
            set
            {
                if (value != _progressPercentage)
                {
                    _progressPercentage = value;
                    _progressBar.SetValue(_progressPercentage);
                    OnPropertyChanged("ProgressPercentage");
                }
            }
        }

        public ColoredProgressBar ProgressBar
        {
            get { return _progressBar; }
            set
            {
                if (value != _progressBar)
                {
                    _progressBar = value;
                    OnPropertyChanged("ProgressBar");
                }
            }
        }

        public Color ProgressBarColor
        {
            get { return _progressBarColor; }
            set
            {
                if (value != _progressBarColor)
                {
                    _progressBarColor = value;
                    _progressBar.SetColor(_progressBarColor);
                    OnPropertyChanged("ProgressBarColor");
                }
            }
        }

        public Color ChineseNameInputColor
        {
            get { return _chineseNameInputColor; }
            set
            {
                if (value != _chineseNameInputColor)
                {
                    _chineseNameInputColor = value;
                    OnPropertyChanged("ChineseNameInputColor");
                }
            }
        }

        public Color EnglishNameInputColor
        {
            get { return _englishNameInputColor; }
            set
            {
                if (value != _englishNameInputColor)
                {
                    _englishNameInputColor = value;
                    OnPropertyChanged("EnglishNameInputColor");
                }
            }
        }

        public Color StudentIdInputColor
        {
            get { return _studentIdInputColor; }
            set
            {
                if (value != _studentIdInputColor)
                {
                    _studentIdInputColor = value;
                    OnPropertyChanged("StudentIdInputColor");
                }
            }
        }

        public Color DegreeInputColor
        {
            get { return _degreeInputColor; }
            set
            {
                if (value != _degreeInputColor)
                {
                    _degreeInputColor = value;
                    OnPropertyChanged("DegreeInputColor");
                }
            }
        }

        public Color YearInputColor
        {
            get { return _yearInputColor; }
            set
            {
                if (value != _yearInputColor)
                {
                    _yearInputColor = value;
                    OnPropertyChanged("YearInputColor");
                }
            }
        }

        public Color ProgramInputColor
        {
            get { return _programInputColor; }
            set
            {
                if (value != _programInputColor)
                {
                    _programInputColor = value;
                    OnPropertyChanged("ProgramInputColor");
                }
            }
        }

        public Color TitleInputColor
        {
            get { return _titleInputColor; }
            set
            {
                if (value != _titleInputColor)
                {
                    _titleInputColor = value;
                    OnPropertyChanged("TitleInputColor");
                }
            }
        }

        public Color AccessInputColor
        {
            get { return _accessInputColor; }
            set
            {
                if (value != _accessInputColor)
                {
                    _accessInputColor = value;
                    OnPropertyChanged("AccessInputColor");
                }
            }
        }

        public Color ReceivedDateInputColor
        {
            get { return _receivedDateInputColor; }
            set
            {
                if (value != _receivedDateInputColor)
                {
                    _receivedDateInputColor = value;
                    OnPropertyChanged("ReceivedDateInputColor");
                }
            }
        }

        public Color ChineseTitleInputColor
        {
            get { return _chineseTitleInputColor; }
            set
            {
                if (value != _chineseTitleInputColor)
                {
                    _chineseTitleInputColor = value;
                    OnPropertyChanged("ChineseTitleInputColor");
                }
            }
        }

        public Color ChineseAbstractInputColor
        {
            get { return _chineseAbstractInputColor; }
            set
            {
                if (value != _chineseAbstractInputColor)
                {
                    _chineseAbstractInputColor = value;
                    OnPropertyChanged("ChineseAbstractInputColor");
                }
            }
        }

        public Color EnglishAbstractInputColor
        {
            get { return _englishAbstractInputColor; }
            set
            {
                if (value != _englishAbstractInputColor)
                {
                    _englishAbstractInputColor = value;
                    OnPropertyChanged("EnglishAbstractInputColor");
                }
            }
        }

        public Color Advisor1InputColor
        {
            get { return _advisor1InputColor; }
            set
            {
                if (value != _advisor1InputColor)
                {
                    _advisor1InputColor = value;
                    OnPropertyChanged("Advisor1InputColor");
                }
            }
        }

        public Color Advisor2InputColor
        {
            get { return _advisor2InputColor; }
            set
            {
                if (value != _advisor2InputColor)
                {
                    _advisor2InputColor = value;
                    OnPropertyChanged("Advisor2InputColor");
                }
            }
        }

        public Color Advisor3InputColor
        {
            get { return _advisor3InputColor; }
            set
            {
                if (value != _advisor3InputColor)
                {
                    _advisor3InputColor = value;
                    OnPropertyChanged("Advisor3InputColor");
                }
            }
        }

        public Color Advisor4InputColor
        {
            get { return _advisor4InputColor; }
            set
            {
                if (value != _advisor4InputColor)
                {
                    _advisor4InputColor = value;
                    OnPropertyChanged("Advisor4InputColor");
                }
            }
        }

        public Color LanguageInputColor
        {
            get { return _languageInputColor; }
            set
            {
                if (value != _languageInputColor)
                {
                    _languageInputColor = value;
                    OnPropertyChanged("LanguageInputColor");
                }
            }
        }

        public bool ChineseNameInputEnabled
        {
            get { return _chineseNameInputEnabled; }
            set
            {
                if (value != _chineseNameInputEnabled)
                {
                    _chineseNameInputEnabled = value;
                    OnPropertyChanged("ChineseNameInputEnabled");
                }
            }
        }
        public bool EnglishNameInputEnabled
        {
            get { return _englishNameInputEnabled; }
            set
            {
                if (value != _englishNameInputEnabled)
                {
                    _englishNameInputEnabled = value;
                    OnPropertyChanged("EnglishNameInputEnabled");
                }
            }
        }
        public bool StudentIdInputEnabled
        {
            get { return _studentIdInputEnabled; }
            set
            {
                if (value != _studentIdInputEnabled)
                {
                    _studentIdInputEnabled = value;
                    OnPropertyChanged("StudentIdInputEnabled");
                }
            }
        }
        public bool DegreeInputEnabled
        {
            get { return _degreeInputEnabled; }
            set
            {
                if (value != _degreeInputEnabled)
                {
                    _degreeInputEnabled = value;
                    OnPropertyChanged("DegreeInputEnabled");
                }
            }
        }
        public bool YearInputEnabled
        {
            get { return _yearInputEnabled; }
            set
            {
                if (value != _yearInputEnabled)
                {
                    _yearInputEnabled = value;
                    OnPropertyChanged("YearInputEnabled");
                }
            }
        }
        public bool ProgramInputEnabled
        {
            get { return _programInputEnabled; }
            set
            {
                if (value != _programInputEnabled)
                {
                    _programInputEnabled = value;
                    OnPropertyChanged("ProgramInputEnabled");
                }
            }
        }
        public bool TitleInputEnabled
        {
            get { return _titleInputEnabled; }
            set
            {
                if (value != _titleInputEnabled)
                {
                    _titleInputEnabled = value;
                    OnPropertyChanged("TitleInputEnabled");
                }
            }
        }
        public bool AccessInputEnabled
        {
            get { return _accessInputEnabled; }
            set
            {
                if (value != _accessInputEnabled)
                {
                    _accessInputEnabled = value;
                    OnPropertyChanged("AccessInputEnabled");
                }
            }
        }
        public bool ReceivedDateInputEnabled
        {
            get { return _receivedDateInputEnabled; }
            set
            {
                if (value != _receivedDateInputEnabled)
                {
                    _receivedDateInputEnabled = value;
                    OnPropertyChanged("ReceivedDateInputEnabled");
                }
            }
        }
        public bool InventoryMatchesInputEnabled
        {
            get { return _inventoryMatchesInputEnabled; }
            set
            {
                if (value != _inventoryMatchesInputEnabled)
                {
                    _inventoryMatchesInputEnabled = value;
                    OnPropertyChanged("InventoryMatchesInputEnabled");
                }
            }
        }
        public bool ChineseTitleInputEnabled
        {
            get { return _chineseTitleInputEnabled; }
            set
            {
                if (value != _chineseTitleInputEnabled)
                {
                    _chineseTitleInputEnabled = value;
                    OnPropertyChanged("ChineseTitleInputEnabled");
                }
            }
        }
        public bool ChineseAbstractInputEnabled
        {
            get { return _chineseAbstractInputEnabled; }
            set
            {
                if (value != _chineseAbstractInputEnabled)
                {
                    _chineseAbstractInputEnabled = value;
                    OnPropertyChanged("ChineseAbstractInputEnabled");
                }
            }
        }
        public bool EnglishAbstractInputEnabled
        {
            get { return _englishAbstractInputEnabled; }
            set
            {
                if (value != _englishAbstractInputEnabled)
                {
                    _englishAbstractInputEnabled = value;
                    OnPropertyChanged("EnglishAbstractInputEnabled");
                }
            }
        }

        public bool Advisor1InputEnabled
        {
            get { return _advisor1InputEnabled; }
            set
            {
                if (value != _advisor1InputEnabled)
                {
                    _advisor1InputEnabled = value;
                    OnPropertyChanged("Advisor1InputEnabled");
                }
            }
        }

        public bool Advisor2InputEnabled
        {
            get { return _advisor2InputEnabled; }
            set
            {
                if (value != _advisor2InputEnabled)
                {
                    _advisor2InputEnabled = value;
                    OnPropertyChanged("Advisor2InputEnabled");
                }
            }
        }

        public bool Advisor3InputEnabled
        {
            get { return _advisor3InputEnabled; }
            set
            {
                if (value != _advisor3InputEnabled)
                {
                    _advisor3InputEnabled = value;
                    OnPropertyChanged("Advisor3InputEnabled");
                }
            }
        }

        public bool Advisor4InputEnabled
        {
            get { return _advisor4InputEnabled; }
            set
            {
                if (value != _advisor4InputEnabled)
                {
                    _advisor4InputEnabled = value;
                    OnPropertyChanged("Advisor4InputEnabled");
                }
            }
        }
        public bool LanguageInputEnabled
        {
            get { return _languageInputEnabled; }
            set
            {
                if (value != _languageInputEnabled)
                {
                    _languageInputEnabled = value;
                    OnPropertyChanged("LanguageInputEnabled");
                }
            }
        }


        public void ResetView()
        {
            this.ShowMenu = false;
            this.ShowPdfTexts = true;
            this.ExpectedThesisFile = null;
            this.SelectedThesisFile = null;
            this.OcrBackColor = Color.White;
            this.SaveButtonEnabled = false;
            this.SaveButtonColor = Color.Green;
            this.ProgressPercentage = 0;
            this.ProgressBarColor = Color.Green;
            this.ChineseNameInputColor = SystemColors.Window;
            this.EnglishNameInputColor = SystemColors.Window;
            this.StudentIdInputColor = SystemColors.Window;
            this.DegreeInputColor = SystemColors.Window;
            this.YearInputColor = SystemColors.Window;
            this.ProgramInputColor = SystemColors.Window;
            this.TitleInputColor = SystemColors.Window;
            this.AccessInputColor = SystemColors.Window;
            this.ReceivedDateInputColor = SystemColors.Window;
            this.ChineseTitleInputColor = SystemColors.Window;
            this.ChineseAbstractInputColor = System.Drawing.Color.LightGoldenrodYellow;
            this.EnglishAbstractInputColor = System.Drawing.Color.LightGoldenrodYellow;
            this.Advisor1InputColor = SystemColors.Window;
            this.Advisor2InputColor = SystemColors.Window;
            this.Advisor3InputColor = SystemColors.Window;
            this.Advisor4InputColor = SystemColors.Window;
            this.LanguageInputColor = SystemColors.Window;

        }

        public void AllowInput(bool allowInput)
        {
            this.ShowMenu = false;
            this.ChineseNameInputEnabled = allowInput;
            this.EnglishNameInputEnabled = allowInput;
            this.StudentIdInputEnabled = allowInput;
            this.DegreeInputEnabled = allowInput;
            this.YearInputEnabled = allowInput;
            this.ProgramInputEnabled = allowInput;
            this.TitleInputEnabled = allowInput;
            this.AccessInputEnabled = allowInput;
            this.ReceivedDateInputEnabled = allowInput;
            this.EnableContentFileListbox = allowInput;
            this.RefreshButtonEnabled = allowInput;
            this.SaveButtonEnabled = allowInput;
            this.InventoryMatchesInputEnabled = allowInput;
            this.ChineseTitleInputEnabled = allowInput;
            this.ChineseAbstractInputEnabled = allowInput;
            this.EnglishAbstractInputEnabled = allowInput;
            this.Advisor1InputEnabled = allowInput;
            this.Advisor2InputEnabled = allowInput;
            this.Advisor3InputEnabled = allowInput;
            this.Advisor4InputEnabled = allowInput;
            this.LanguageInputEnabled = allowInput;
        }
    }
}
