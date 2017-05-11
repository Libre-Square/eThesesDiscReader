using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class DisplayFileInfo : NotifyPropertyChangeBean
    {
        private string _displayFileName;
        private FileInfo _fileInfo;

        public string DisplayFileName
        {
            get { return _displayFileName; }
            set
            {
                _displayFileName = value;
                OnPropertyChanged("DisplayFileName");
            }
        }

        public FileInfo FileInfo
        {
            get { return _fileInfo; }
            set
            {
                _fileInfo = value;
                OnPropertyChanged("FileInfo");
            }
        }
    }
}
