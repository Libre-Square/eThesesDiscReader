using eThesesDiscReader.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Models
{
    public class DiscContents : NotifyPropertyChangeBean
    {
        private static DiscContents _instance = new DiscContents();
        private BindingList<FileInfo> _fileInfoList = new BindingList<FileInfo>();
        private BindingList<DisplayFileInfo> _displayFileInfoList = new BindingList<DisplayFileInfo>();
        private BindingList<MaskedFileInfo> _maskedFileInfoList = new BindingList<MaskedFileInfo>();
        private byte[] _fileDigests;
        private long _totalFileSize;

        public static DiscContents getInstance()
        {
            return _instance;
        }

        public BindingList<FileInfo> FileInfoList
        {
            get { return _fileInfoList; }
            set
            {
                _fileInfoList.Clear();
                _displayFileInfoList.Clear();
                _maskedFileInfoList.Clear();
                int sourceContentPathLength = ReaderConfig.getInstance().ContentPath.Length;
                BindingList<MaskedFileInfo> newMaskedFileInfoList = new BindingList<MaskedFileInfo>();
                foreach (FileInfo fileInfo in value)
                {
                    _fileInfoList.Add(fileInfo);

                    DisplayFileInfo displayFileInfo = new DisplayFileInfo();
                    string relativeFilePath = fileInfo.FullName.Substring(sourceContentPathLength);
                    if (relativeFilePath.StartsWith(@"\"))
                        relativeFilePath = relativeFilePath.Substring(1);
                    displayFileInfo.DisplayFileName = relativeFilePath;
                    displayFileInfo.FileInfo = fileInfo;
                    _displayFileInfoList.Add(displayFileInfo);

                    if (fileInfo.FullName.ToLower().EndsWith(@".pdf"))
                    {
                        MaskedFileInfo maskedFileInfo = new MaskedFileInfo();
                        string stagingFilePath = FileUtils.DEFAULT_STAGING_DIRECTORY + relativeFilePath;
                        //maskedFileInfo.MaskedFileName = @"<staging>\" + relativeFilePath;
                        maskedFileInfo.MaskedFileName = relativeFilePath;
                        maskedFileInfo.FileInfo = new FileInfo(stagingFilePath);
                        newMaskedFileInfoList.Add(maskedFileInfo);
                    }
                }
                this.MaskedFileInfoList = newMaskedFileInfoList;
                OnPropertyChanged("FileInfoList");
            }
        }

        public BindingList<DisplayFileInfo> DisplayFileInfoList
        {
            get { return _displayFileInfoList; }
            set
            {
                _displayFileInfoList.Clear();
                foreach (DisplayFileInfo displayFileInfo in value)
                {
                    _displayFileInfoList.Add(displayFileInfo);
                }
                OnPropertyChanged("DisplayFileInfo");
            }
        }

        public BindingList<MaskedFileInfo> MaskedFileInfoList
        {
            get { return _maskedFileInfoList; }
            set
            {
                _maskedFileInfoList.Clear();
                foreach (MaskedFileInfo maskedFileInfo in value)
                {
                    _maskedFileInfoList.Add(maskedFileInfo);
                }
                OnPropertyChanged("MaskedFileInfoList");
            }
        }

        public bool updateFileInfoList(List<FileInfo> newFileInfoList)
        {
            if (discContentChanged(newFileInfoList))
            {
                this.FileInfoList = new BindingList<FileInfo>(newFileInfoList);
                return true;
            }
            return false;
        }

        protected bool discContentChanged(List<FileInfo> newFileInfoList)
        {
            string fileNameConcat = "";
            foreach (FileInfo newFileInfo in newFileInfoList)
                fileNameConcat += newFileInfo.FullName;

            byte[] newContentDigest = DiscReaderUtils.shaDigest(fileNameConcat);
            if (!DiscReaderUtils.compareDigests(_fileDigests, newContentDigest))
            {
                _fileDigests = newContentDigest;
                return true;
            }
            return false;
        }

        public long TotalFileSize
        {
            get { return _totalFileSize; }
            set
            {
                _totalFileSize = value;
                OnPropertyChanged("TotalFileSize");
            }
        }
    }
}
