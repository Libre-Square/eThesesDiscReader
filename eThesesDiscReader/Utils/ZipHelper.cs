using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace eThesesDiscReader.Utils
{
    public class ZipHelper
    {

        public static void CreateZipFromDirectory(string directoryPath, string zipFilePath)
        {
            if (String.IsNullOrEmpty(directoryPath) || String.IsNullOrEmpty(zipFilePath))
                return;

            using (ZipFile zip = new ZipFile(zipFilePath))
            {
                zip.AlternateEncoding = Encoding.UTF8;
                zip.AddDirectory(directoryPath);
                zip.Save();
            }
        }
    }
}