using eThesesDiscReader.Data;
using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace eThesesDiscReader.Controls
{
    public class MainThread
    {
        private static MainThread _instance = new MainThread();
        private bool started = false;
        private BindingList<InventoryRecord> _matchingInventoryList = new BindingList<InventoryRecord>();
        private ThesisRecord _thesisRecord = new ThesisRecord();
        private DiscReader _reader = new DiscReader();

        public static MainThread getInstance()
        {
            return _instance;
        }

        public void start()
        {
            this.run();
        }

        public void stop()
        {
            this.started = false;
        }

        public void run()
        {
            this.started = true;

            ViewModel.getInstance().MenuButtonEnabled = true;
            ViewModel.getInstance().SaveButtonEnabled = false;

            if (String.IsNullOrWhiteSpace(ReaderConfig.getInstance().InstanceName))
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                for (int i = 0; i < 2; i++)
                {
                    var random = new Random();
                    ReaderConfig.getInstance().InstanceName += chars[random.Next(chars.Length)];
                    Thread.Sleep(random.Next(500));
                }
                ReaderConfig.getInstance().SaveSettings();
            }

            if (String.IsNullOrWhiteSpace(ReaderConfig.getInstance().InstanceName))
            {
                while (!PdfReaderHelper.getInstance().ContainerPanel.IsHandleCreated)
                    Thread.Sleep(100);
                MessageBox.Show(PdfReaderHelper.getInstance().ContainerPanel, "Please enter a unique Instance Name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                while (String.IsNullOrWhiteSpace(ReaderConfig.getInstance().InstanceName))
                    Thread.Sleep(100);
            }

            while (started)
            {
                string contentPath = ReaderConfig.getInstance().ContentPath;
                ViewModel.getInstance().DatabaseStatusColor = initStorage() ? SystemColors.Control : Color.Red;
                try
                {
                    if (contentPath == null || "".Equals(contentPath) || !Directory.Exists(contentPath))
                    {
                        foreach (DriveInfo drive in DriveInfo.GetDrives())
                        {
                            if (drive.DriveType == DriveType.CDRom && drive.IsReady)
                                contentPath = drive.Name;
                        }
                        if (contentPath != null && !"".Equals(contentPath) && Directory.Exists(contentPath))
                        {
                            ReaderConfig.getInstance().ContentPath = contentPath;
                            ReaderConfig.getInstance().SaveSettings();
                        }
                    }
                    if (contentPath != null && !"".Equals(contentPath) && Directory.Exists(contentPath))
                    {
                        if (readDiscContent(contentPath))
                        {
                            PdfReaderHelper.getInstance().unload();
                            ViewModel viewModel = ViewModel.getInstance();
                            viewModel.ResetView();
                            this.ThesisRecord.clearExtractedData();
                            InventoryStore.getInstance().updateSelectedInventoryRecord(null);
                            MainThread.getInstance().MatchingInventoryList.Clear();

                            FileUtils.CleanUpStaging();
                            FileUtils.CleanUpTemp();

                            DiscContentsProcessor.getInstance().process();
                        }
                    }
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(PdfReaderHelper.getInstance().ContainerPanel, ioe.Message + Environment.NewLine + ioe.StackTrace, ioe.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Thread.Sleep(1000);
            }
        }

        protected bool initStorage()
        {
            String dbPath = ReaderConfig.getInstance().DBPath.Trim();
            String storagePath = ReaderConfig.getInstance().StoragePath.Trim();
            if (dbPath == null || dbPath.Equals("") || !File.Exists(dbPath))
                return false;
            if (storagePath == null || storagePath.Equals("") || !Directory.Exists(storagePath))
                return false;

            InventoryStore inventoryStore = InventoryStore.getInstance();
            if (inventoryStore.InventoryDataTable == null || inventoryStore.InventoryDataTable.Rows.Count <= 0)
            {
                try
                {
                    inventoryStore.InventoryDataTable = DBInventoryRecords.listInventoryItems();
                    if (inventoryStore.InventoryDataTable.Rows.Count <= 0)
                        return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
            
            return true;
        }

        protected bool readDiscContent(string contentPath)
        {
            if (!_reader.read(contentPath))
                return false;

            if (DiscContents.getInstance().FileInfoList != null)
            {
                foreach (FileInfo fileInfo in DiscContents.getInstance().FileInfoList)
                {
                    if (fileInfo != null && fileInfo.FullName != null && fileInfo.FullName.ToLower().EndsWith(".pdf"))
                        return true;
                }
            }
            return false;
        }

        public ThesisRecord ThesisRecord
        {
            get { return _thesisRecord; }
            set { _thesisRecord = value; }
        }

        public BindingList<InventoryRecord> MatchingInventoryList
        {
            get { return _matchingInventoryList; }
            set { _matchingInventoryList = value; }
        }
    }
}
