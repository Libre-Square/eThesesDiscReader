using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Utils
{
    public class DriveHelper
    {
        public static readonly string STORAGE_DRIVE_LETTER = "Y";
        public static readonly string STORAGE_DRIVE_PATH = @"\\rsdi-ns01.lib.cuhk.edu.hk\etd";
        public static readonly string STORAGE_DRIVE_USER_ID = @"etd_admin";
        public static readonly string STORAGE_DRIVE_PASSWORD = @"etd9@ss";

        private static WshNetwork networkShell = new WshNetwork();

        public static void connectStorageDrive()
        {
            disconnectStorageDrive();
            networkShell.MapNetworkDrive(STORAGE_DRIVE_LETTER + ":", STORAGE_DRIVE_PATH, false, STORAGE_DRIVE_USER_ID, STORAGE_DRIVE_PASSWORD);
        }

        public static void disconnectStorageDrive()
        {
            DriveInfo driveInfo = new DriveInfo(STORAGE_DRIVE_LETTER);
            if (driveInfo.IsReady)
                networkShell.RemoveNetworkDrive(STORAGE_DRIVE_LETTER + ":", true, true);
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
