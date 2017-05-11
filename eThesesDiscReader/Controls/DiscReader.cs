using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Controls
{
    public class DiscReader
    {
        List<string> _filePathList = new List<string>();

        public bool read(string path)
        {
            DiscContents discContents = DiscContents.getInstance();
            this._filePathList.Clear();
            this._filePathList.AddRange(FileUtils.ListFilesInDirectory(path));

            List<FileInfo> fileInfoList = new List<FileInfo>();
            long totalFileSize = 0;
            foreach (string filePath in this._filePathList)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                fileInfoList.Add(fileInfo);
                totalFileSize += fileInfo.Length;
            }
            discContents.TotalFileSize = totalFileSize;
            bool isNewDisc = discContents.updateFileInfoList(fileInfoList);
            this._filePathList.Clear();
            return isNewDisc;
        }
    }
}
