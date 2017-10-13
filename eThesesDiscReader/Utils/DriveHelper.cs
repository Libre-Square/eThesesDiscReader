//using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace eThesesDiscReader.Utils
{
    public class DriveHelper
    {
        public static readonly string STORAGE_DRIVE_PATH = @"\\rsdi-ns01.lib.cuhk.edu.hk\etd";
        public static readonly string STORAGE_DRIVE_USER_ID = @"etd_admin";
        public static readonly string STORAGE_DRIVE_PASSWORD = @"etd9@ss";

        //private static WshNetwork networkShell = new WshNetwork();

        public static void connectStorageDrive()
        {
            disconnectStorageDrive();

            Process p = null;
            try
            {
                p = Process.Start("cmd.exe", "/C net use " + STORAGE_DRIVE_PATH + STORAGE_DRIVE_PASSWORD + " /USER:" + STORAGE_DRIVE_USER_ID);
                while (!p.WaitForExit(1000))
                {
                    //wait
                }
                if (p.ExitCode != 0)
                    MessageBox.Show("Failed to connect network drive!", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (p != null)
                    p.Close();
            }
        }

        public static void disconnectStorageDrive()
        {
            Process p = null;
            try
            {
                p = Process.Start("cmd.exe", "/C net use " + STORAGE_DRIVE_PATH + " /DELETE");
                while (!p.WaitForExit(1000))
                {
                    //wait
                }
            }
            finally
            {
                if (p != null)
                    p.Close();
            }
        }

        public static DriveInfo getDriveByType(DriveType driveType)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == driveType && drive.IsReady)
                    return drive;
            }
            return null;
        }

    }
}
