using eThesesDiscReader.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class ReaderConfig : NotifyPropertyChangeBean
    {
        private static ReaderConfig _instance;
        private static Settings _settings = Settings.Default;
        private string _instanceName;
        private string _contentPath;
        private string _pythonPath;
        private string _pdfminerPath;
        private bool   _useAdobeReader;
        private bool   _enableOCR;
        private string _ghostscriptPath;
        private string _tesseractPath;
        private string _dbPath;
        private string _storagePath;

        public static ReaderConfig getInstance()
        {
            if (_instance == null)
            {
                _instance = new ReaderConfig();
                _instance.InstanceName = _settings.InstanceName;
                _instance.ContentPath = _settings.ContentPath;
                _instance.PythonPath = _settings.PythonPath;
                _instance.PdfminerPath = _settings.PDFMinerPath;
                _instance.UseAdobeReader = _settings.UseAdobeReader;
                _instance.EnableOCR = _settings.EnableOCR;
                _instance.GhostscriptPath = _settings.GhostscriptPath;
                _instance.TesseractPath = _settings.TesseractPath;
                _instance.DBPath = _settings.DBPath;
                _instance.StoragePath = _settings.StoragePath;
            }
            return _instance;
        }

        public string InstanceName
        {
            get { return _instanceName; }
            set
            {
                if (value != _instanceName)
                {
                    _instanceName = value;
                    _settings.InstanceName = value;
                    OnPropertyChanged("InstanceName");
                }
            }
        }

        public string ContentPath
        {
            get { return _contentPath; }
            set
            {
                if (value != _contentPath)
                {
                    _contentPath = value;
                    _settings.ContentPath = value;
                    OnPropertyChanged("ContentPath");
                }
            }
        }

        public string PythonPath
        {
            get { return _pythonPath; }
            set
            {
                if (value != _pythonPath)
                {
                    _pythonPath = value;
                    _settings.PythonPath = value;
                    OnPropertyChanged("PythonPath");
                }
            }
        }

        public string PdfminerPath
        {
            get { return _pdfminerPath; }
            set
            {
                if (value != _pdfminerPath)
                {
                    _pdfminerPath = value;
                    _settings.PDFMinerPath = value;
                    OnPropertyChanged("PdfminerPath");
                }
            }
        }

        public bool UseAdobeReader
        {
            get { return _useAdobeReader; }
            set
            {
                if (value != _useAdobeReader)
                {
                    _useAdobeReader = value;
                    _settings.UseAdobeReader = value;
                    OnPropertyChanged("UseAdobeReader");
                }
            }
        }

        public bool EnableOCR
        {
            get { return _enableOCR; }
            set
            {
                if (value != _enableOCR)
                {
                    _enableOCR = value;
                    _settings.EnableOCR = value;
                    OnPropertyChanged("EnableOCR");
                }
            }
        }

        public string GhostscriptPath
        {
            get { return _ghostscriptPath; }
            set
            {
                if (value != _ghostscriptPath)
                {
                    _ghostscriptPath = value;
                    _settings.GhostscriptPath = value;
                    OnPropertyChanged("GhostscriptPath");
                }
            }
        }

        public string TesseractPath
        {
            get { return _tesseractPath; }
            set
            {
                if (value != _tesseractPath)
                {
                    _tesseractPath = value;
                    _settings.TesseractPath = value;
                    OnPropertyChanged("TesseractPath");
                }
            }
        }

        public string DBPath
        {
            get { return _dbPath; }
            set
            {
                if (value != _dbPath)
                {
                    _dbPath = value;
                    _settings.DBPath = value;
                    OnPropertyChanged("DBPath");
                }
            }
        }

        public string StoragePath
        {
            get { return _storagePath; }
            set
            {
                if (value != _storagePath)
                {
                    _storagePath = value;
                    _settings.StoragePath = value;
                    OnPropertyChanged("StoragePath");
                }
            }
        }

        public void SaveSettings()
        {
            _settings.Save();
        }
    }
}
