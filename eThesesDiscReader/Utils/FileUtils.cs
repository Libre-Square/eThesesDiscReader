using eThesesDiscReader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;
using eThesesDiscReader.Controls;
using eThesesDiscReader.Data;
using System.IO.Packaging;

namespace eThesesDiscReader.Utils
{
    public class FileUtils
    {
        public readonly static string DEFAULT_STAGING_DIRECTORY = @"D:\eThesisReader\staging\";
        public readonly static string DEFAULT_TEMP_DIRECTORY = @"D:\eThesisReader\temp\";
        public readonly static string TEMP_PDF_FILE_NAME = @"temp.pdf";
        public readonly static string DEFAULT_ZIP_FILE = @"content.zip";

        private static int zipFileCopyPercentage = 0;
        private static int fullTextFileCopyPercentage = 0;
        private static bool zipFileCopyComplete = false;
        private static bool zipFileCopySuccess = false;
        private static bool fullTextFileCopyComplete = false;
        private static bool fullTextFileCopySuccess = false;

        public static void CopyContentToStaging()
        {
            CleanUpStaging();
            long totalFileSize = DiscContents.getInstance().TotalFileSize;
            long copiedFileSize = 0;
            string sourceDirectory = ReaderConfig.getInstance().ContentPath;
            if (!sourceDirectory.EndsWith(@"\"))
                sourceDirectory += @"\";

            foreach (FileInfo fileInfo in DiscContents.getInstance().FileInfoList)
            {
                string fileName = fileInfo.Name;
                string targetFileDirectory = DEFAULT_STAGING_DIRECTORY + fileInfo.FullName.Substring(sourceDirectory.Length);
                targetFileDirectory = targetFileDirectory.Substring(0, targetFileDirectory.Length - fileName.Length);
                if (!targetFileDirectory.EndsWith(@"\"))
                    targetFileDirectory += @"\";
                if (!Directory.Exists(targetFileDirectory))
                    Directory.CreateDirectory(targetFileDirectory);

                File.Copy(fileInfo.FullName, targetFileDirectory + fileName);
                copiedFileSize += fileInfo.Length;
                ViewModel.getInstance().ProgressPercentage = copiedFileSize * 100 / totalFileSize;
            }
        }

        public static void BgCreateZipForStaging()
        {
            Thread zipThread = new Thread(new ThreadStart(CreateZipForStaging));
            zipThread.SetApartmentState(ApartmentState.STA);
            zipThread.Start();
        }

        public static void CreateZipForStaging()
        {
            ViewModel.getInstance().ProgressBarColor = Color.Yellow;
            //For .NET 4.5 Up Only
            //ZipFile.CreateFromDirectory(DEFAULT_STAGING_DIRECTORY, DEFAULT_TEMP_DIRECTORY + DEFAULT_ZIP_FILE);

            //For .NET 4.0
            ZipHelper.CreateZipFromDirectory(DEFAULT_STAGING_DIRECTORY, DEFAULT_TEMP_DIRECTORY + DEFAULT_ZIP_FILE);

            File.Move(DEFAULT_TEMP_DIRECTORY + DEFAULT_ZIP_FILE, DEFAULT_STAGING_DIRECTORY + DEFAULT_ZIP_FILE);
            ViewModel.getInstance().ProgressPercentage += 50;
        }

        public static void CopyZipToStorage()
        {
            zipFileCopyPercentage = 0;
            fullTextFileCopyPercentage = 0;
            zipFileCopyComplete = false;
            zipFileCopySuccess = false;
            fullTextFileCopyComplete = false;
            fullTextFileCopySuccess = false;

            ThesisRecord record = MainThread.getInstance().ThesisRecord;
            //File.Copy(DEFAULT_STAGING_DIRECTORY + DEFAULT_ZIP_FILE, targetFilePath, false);

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += ZipFileCopyProgress_Update;
                webClient.DownloadFileCompleted += ZipFileCopy_Completed;
                webClient.DownloadFileAsync(new Uri(DEFAULT_STAGING_DIRECTORY + DEFAULT_ZIP_FILE), record.StorageFolderPath + record.FilePath);
            }
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += FullTextFileCopyProgress_Update;
                webClient.DownloadFileCompleted += FullTextFileCopy_Completed;
                webClient.DownloadFileAsync(new Uri(DEFAULT_TEMP_DIRECTORY + TEMP_PDF_FILE_NAME), record.StorageFolderPath + record.FullTextFilePath);
            }
        }

        private static void ZipFileCopyProgress_Update(object sender, DownloadProgressChangedEventArgs e)
        {
            zipFileCopyPercentage = e.ProgressPercentage;
            FileCopyProgress_Update();
        }

        private static void ZipFileCopy_Completed(object sender, AsyncCompletedEventArgs e)
        {
            zipFileCopyComplete = true;
            DiscContentsProcessor processor = DiscContentsProcessor.getInstance();
            ThesisRecord record = MainThread.getInstance().ThesisRecord;
            zipFileCopySuccess = (e.Error == null && File.Exists(record.StorageFolderPath + record.FilePath));
            FileCopy_Completed();
        }

        private static void FullTextFileCopyProgress_Update(object sender, DownloadProgressChangedEventArgs e)
        {
            fullTextFileCopyPercentage = e.ProgressPercentage;
            FileCopyProgress_Update();
        }

        private static void FullTextFileCopy_Completed(object sender, AsyncCompletedEventArgs e)
        {
            fullTextFileCopyComplete = true;
            DiscContentsProcessor processor = DiscContentsProcessor.getInstance();
            ThesisRecord record = MainThread.getInstance().ThesisRecord;
            fullTextFileCopySuccess = (e.Error == null && File.Exists(record.StorageFolderPath + record.FullTextFilePath));
            FileCopy_Completed();
        }

        private static void FileCopyProgress_Update()
        {
            ViewModel.getInstance().ProgressPercentage = (zipFileCopyPercentage + fullTextFileCopyPercentage) / 2;
        }

        private static void FileCopy_Completed()
        {
            if (zipFileCopyComplete && fullTextFileCopyComplete)
            {
                DiscContentsProcessor processor = DiscContentsProcessor.getInstance();
                ThesisRecord record = MainThread.getInstance().ThesisRecord;
                if (zipFileCopySuccess && fullTextFileCopySuccess)
                    processor.fileCopySuccess();
                else
                    processor.fileCopyFailed();
            }
        }

        public static void CleanUpStaging()
        {
            CleanUpDirectory(DEFAULT_STAGING_DIRECTORY);
        }

        public static string CopyPdfToTemp(string filePath)
        {
            int RETRY_COUNT = 5;
            int RETRY_INTERVAL = 500;
            string tempFilePath = DEFAULT_TEMP_DIRECTORY + TEMP_PDF_FILE_NAME;
            if (File.Exists(tempFilePath))
            {
                for (int retry = 0; retry < RETRY_COUNT; retry++)
                {
                    try
                    {
                        if (retry > 0)
                            Thread.Sleep(RETRY_INTERVAL);
                        File.SetAttributes(tempFilePath, FileAttributes.Normal);
                        File.Delete(tempFilePath);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            File.Copy(filePath, tempFilePath, true);
            return tempFilePath;
        }

        public static void CleanUpTemp()
        {
            CleanUpDirectory(DEFAULT_TEMP_DIRECTORY);
        }

        public static void CleanUpDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                List<string> filePathList = ListFilesInDirectory(path);
                foreach (string filePath in filePathList)
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        public static List<string> ListFilesInDirectory(string directory)
        {
            List<string> filePathList = new List<string>();
            filePathList.AddRange(Directory.GetFiles(directory));
            ListSubtreeFiles(directory, filePathList);
            return filePathList;
        }

        private static void ListSubtreeFiles(string path, List<string> filePathList)
        {
            foreach (string dir in Directory.GetDirectories(path))
            {
                foreach (string file in Directory.GetFiles(dir))
                    filePathList.Add(file);
                ListSubtreeFiles(dir, filePathList);
            }
        }
    }
}
