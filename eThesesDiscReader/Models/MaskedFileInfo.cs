using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Models
{
    public class MaskedFileInfo : NotifyPropertyChangeBean
    {
        private string _maskedFileName;
        private FileInfo _fileInfo;

        public string MaskedFileName
        {
            get { return _maskedFileName; }
            set
            {
                _maskedFileName = value;
                OnPropertyChanged("MaskedFileName");
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
