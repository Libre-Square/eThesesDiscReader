using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using eThesesDiscReader.Controls;
using eThesesDiscReader.Utils;
using eThesesDiscReader.Models;
using System.Deployment.Application;
using Microsoft.Win32;

namespace eThesesDiscReader
{
    static class Program
    {
        private static MainThread m = MainThread.getInstance();
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            runOnce(); // Setup application icon for "Add remove programs"

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 1 && "reset".Equals(args[0]))
            {
                ReaderConfig config = ReaderConfig.getInstance();
                config.ContentPath = args[1];
                config.PythonPath = @"lib\WinPython\python\python.exe";
                config.PdfminerPath = @"lib\WinPython\python\Scripts\pdf2txt.py";
                config.UseAdobeReader = true;
                config.EnableOCR = false;
                config.GhostscriptPath = @"lib\Ghostscript\bin\gswin32.exe";
                config.TesseractPath = @"lib\Tesseract-ocr\tesseract.exe";
                config.DBPath = @"Y:\rsdiconversion\db\ethesis2016.db3";
                config.StoragePath = @"Y:\rsdiconversion\contents";
                config.SaveSettings();
            }

            Form mainForm = new MainForm();
            
            // Initialize PDF viewer component
            PdfReaderHelper.getInstance().init(ReaderConfig.getInstance().UseAdobeReader);

            // Map network drive
            DriveHelper.connectStorageDrive();

            // Start file monitor thread
            Thread readCDThread = new Thread(new ThreadStart(m.start));
            readCDThread.SetApartmentState(ApartmentState.STA);
            readCDThread.Start();

            Application.Run(mainForm);
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            PdfReaderHelper.getInstance().unload();
            DriveHelper.disconnectStorageDrive();
            FileUtils.CleanUpStaging();
            FileUtils.CleanUpTemp();
            m.stop();
        }

        private static void runOnce()
        {
            try
            {
                if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    // Set icon for "Add remove programs"
                    try
                    {
                        string exeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Lo‌​cation);
                        string iconSourcePath = Path.Combine(exeDirectory, "eThesesDiscReader.ico");
                        if (!File.Exists(iconSourcePath))
                        {
                            MessageBox.Show("Application icon not found");
                            return;
                        }

                        RegistryKey myUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                        string[] mySubKeyNames = myUninstallKey.GetSubKeyNames();
                        for (int i = 0; i < mySubKeyNames.Length; i++)
                        {
                            RegistryKey myKey = myUninstallKey.OpenSubKey(mySubKeyNames[i], true);
                            object myValue = myKey.GetValue("DisplayName");
                            if (myValue != null && myValue.ToString() == "eThesesDiscReader")
                            {
                                myKey.SetValue("DisplayIcon", iconSourcePath);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Failed to set up applicartion icon" + e.ToString());
                    }
                }
            }
            catch (Exception)
            {
                // Not deployed as ClickOnce
            }
            
        }
    }
}
