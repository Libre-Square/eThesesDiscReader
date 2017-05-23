using eThesesDiscReader.Data;
using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Controls
{
    public class DiscContentsProcessor : NotifyPropertyChangeBean
    {
        private readonly static Regex EXPECTED_FILE_PATTERN_REGEX = new Regex(@".*pdf.*\\.*full.*.pdf$");
        private readonly static Regex STUDENT_ID_REGEX = new Regex(@"[0-9]{10}");
        private readonly static Regex STUDENT_NAME_REGEX = new Regex(@"^[0-9]{10}[_‐﹘-][^_‐﹘-]+[_‐﹘-]");
        public static readonly string DEFAULT_DATE_FORMAT = @"dd/MM/yyyy";

        private static DiscContentsProcessor _instance = new DiscContentsProcessor();

        private bool firstLoadReady = false;

        public static DiscContentsProcessor getInstance()
        {
            return _instance;
        }

        public void process()
        {
            try
            {
                ViewModel viewModel = ViewModel.getInstance();
                viewModel.ProgressBar.SetMaximum(200);
                viewModel.ProgressBarColor = Color.Yellow;
                viewModel.AllowInput(false);
                viewModel.MenuButtonEnabled = false;
                FileUtils.CleanUpTemp();
                copyToStaging();
                FileUtils.BgCreateZipForStaging();
                loadExpectedThesisFile();
                viewModel.AllowInput(true);
                viewModel.MenuButtonEnabled = true;
                this.firstLoadReady = true;
            }
            catch (Exception e)
            {
                alert(e.StackTrace, e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ViewModel.getInstance().MenuButtonEnabled = true;
            }
        }

        public void selectionChanged()
        {
            if (firstLoadReady)
                loadSelectedThesisFile();
        }

        public void save()
        {
            ViewModel.getInstance().ProgressBarColor = Color.Green;
            ViewModel.getInstance().ProgressBar.Maximum = 100;
            ThesisRecord record = MainThread.getInstance().ThesisRecord;
            if (!validateInput(record))
                return;

            ViewModel.getInstance().AllowInput(false);
            ViewModel.getInstance().MenuButtonEnabled = false;

            record.RecordId = DBEtdRecords.generateRecordId();
            record.FilePath = record.RecordId + ".zip";
            record.FullTextFilePath = record.RecordId + ".pdf";

            string storageFolderPath = ReaderConfig.getInstance().StoragePath;
            if (!storageFolderPath.EndsWith(@"\"))
                storageFolderPath += @"\";
            record.StorageFolderPath = storageFolderPath;

            FileUtils.CopyZipToStorage();
        }

        protected bool validateInput(ThesisRecord record)
        {
            if (String.IsNullOrWhiteSpace(record.StudentId))
            {
                alert("Student ID cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.EnglishName))
            {
                alert("Name (English) cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.Degree))
            {
                alert("Degree cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.Year))
            {
                alert("Year cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.StudentId))
            {
                alert("Student ID cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.Programme))
            {
                alert("Programme cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.ThesisTitle))
            {
                alert("Thesis Title cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (String.IsNullOrWhiteSpace(record.AccessMode))
            {
                alert("Agree to allow cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (record.ReceivedDate == null)
            {
                alert("Received Date cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public void fileCopySuccess()
        {
            ThesisRecord record = MainThread.getInstance().ThesisRecord;
            record.CreateDate = DateTime.Now;

            List<ThesisRecord> recordList = new List<ThesisRecord>();
            recordList.Add(record);
            int rowsInserted = DBEtdRecords.insertEtdRecords(recordList);
            if (rowsInserted == 1)
            {
                ViewModel.getInstance().ProgressBarColor = Color.Green;
                alert("Thesis record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViewModel.getInstance().MenuButtonEnabled = true;
            }
            else
            {
                string archiveFilePath = record.StorageFolderPath + record.FilePath;
                string fullTextFilePath = record.StorageFolderPath + record.FullTextFilePath;
                if (File.Exists(archiveFilePath))
                    File.Delete(archiveFilePath);
                if (File.Exists(fullTextFilePath))
                    File.Delete(fullTextFilePath);
                ViewModel.getInstance().ProgressBarColor = Color.Red;
                alert("Failed to insert record into database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ViewModel.getInstance().AllowInput(true);
                ViewModel.getInstance().MenuButtonEnabled = true;
            }
        }

        public void fileCopyFailed()
        {
            ViewModel.getInstance().ProgressBarColor = Color.Red;
            alert("Failed to copy file contents to storage!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ViewModel.getInstance().AllowInput(true);
            ViewModel.getInstance().MenuButtonEnabled = true;
        }

        public void copyToStaging()
        {
            FileUtils.CopyContentToStaging();
        }

        protected void loadExpectedThesisFile()
        {
            MaskedFileInfo expectedFileInfo = expectedFileLookup(DiscContents.getInstance());
            ViewModel viewModel = ViewModel.getInstance();
            if (expectedFileInfo != null)
            {
                viewModel.ExpectedThesisFile = expectedFileInfo;
                viewModel.SelectedThesisFile = expectedFileInfo;
            }
            loadSelectedThesisFile();
        }

        public void loadSelectedThesisFile()
        {
            ViewModel.getInstance().ProgressBarColor = Color.Yellow;
            MaskedFileInfo selectedFileInfo = ViewModel.getInstance().SelectedThesisFile;
            ViewModel viewModel = ViewModel.getInstance();
            if (selectedFileInfo != null)
            {
                viewModel.SaveButtonEnabled = false;
                ThesisRecord thesisRecord = MainThread.getInstance().ThesisRecord;
                thesisRecord.clearExtractedData();
                viewModel.SelectedThesisFile = selectedFileInfo;
                viewModel.EnableContentFileListbox = false;
                thesisRecord.PdfTexts = "";

                PdfReaderHelper.getInstance().unload();
                string tempPdfFileName = FileUtils.CopyPdfToTemp(selectedFileInfo.FileInfo.FullName);
                loadPDF(tempPdfFileName);
                viewModel.EnableContentFileListbox = true;

                determineThesisRecord(thesisRecord);

                MainThread.getInstance().MatchingInventoryList.Clear();
                List<InventoryRecord> matchingInventoryRecords = getMatchingInventoryRecordList(thesisRecord);
                if (matchingInventoryRecords.Count > 0)
                {
                    foreach (InventoryRecord matchingRecord in matchingInventoryRecords)
                        MainThread.getInstance().MatchingInventoryList.Add(matchingRecord);
                    InventoryStore.getInstance().updateSelectedInventoryRecord(matchingInventoryRecords.First());
                }
                else
                {
                    InventoryStore.getInstance().updateSelectedInventoryRecord(null);
                }
                //markupRecordDifference();
                thesisRecord.EnableValidation = true;
                viewModel.SaveButtonEnabled = true;
            }
            ViewModel.getInstance().ProgressPercentage += 50;
        }

        //public void markupRecordDifference()
        //{
        //    Color EMPTY_VALUE_COLOR = Color.Yellow;
        //    Color CONFLICT_VALUE_COLOR = Color.Pink;
        //    Color MATCH_VALUE_COLOR = Color.LightGreen;

        //    ViewModel viewModel = ViewModel.getInstance();
        //    ThesisRecord thesisRecord = MainThread.getInstance().ThesisRecord;
        //    InventoryRecord invRecord = InventoryStore.getInstance().SelectedInventoryRecord;

        //    viewModel.ChineseNameInputColor = SystemColors.Window;
        //    viewModel.EnglishNameInputColor = SystemColors.Window;
        //    viewModel.StudentIdInputColor = SystemColors.Window;
        //    viewModel.DegreeInputColor = SystemColors.Window;
        //    viewModel.YearInputColor = SystemColors.Window;
        //    viewModel.ProgramInputColor = SystemColors.Window;
        //    viewModel.TitleInputColor = SystemColors.Window;
        //    viewModel.AccessInputColor = SystemColors.Window;
        //    viewModel.ReceivedDateInputColor = SystemColors.Window;

        //    // Chinese Name
        //    if (thesisRecord.ChineseName == null)
        //        thesisRecord.ChineseName = "";
        //    if (String.IsNullOrEmpty(thesisRecord.ChineseName) && String.IsNullOrEmpty(invRecord.ChineseName))
        //        viewModel.ChineseNameInputColor = CONFLICT_VALUE_COLOR;
        //    else if (String.IsNullOrEmpty(thesisRecord.ChineseName) && !String.IsNullOrEmpty(invRecord.ChineseName))
        //    {
        //        thesisRecord.ChineseName = invRecord.ChineseName;
        //        viewModel.ChineseNameInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.ChineseNameInputColor = thesisRecord.ChineseName.Equals(invRecord.ChineseName) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // English Name
        //    if (thesisRecord.EnglishName == null)
        //        thesisRecord.EnglishName = "";
        //    if (String.IsNullOrEmpty(thesisRecord.EnglishName) && String.IsNullOrEmpty(invRecord.Name))
        //        viewModel.EnglishNameInputColor = CONFLICT_VALUE_COLOR;
        //    else if(String.IsNullOrEmpty(thesisRecord.EnglishName) && !String.IsNullOrEmpty(invRecord.Name))
        //    {
        //        thesisRecord.EnglishName = invRecord.Name;
        //        viewModel.EnglishNameInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.EnglishNameInputColor = thesisRecord.EnglishName.Equals(invRecord.Name) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Student ID
        //    if (thesisRecord.StudentId == null)
        //        thesisRecord.StudentId = "";
        //    if (String.IsNullOrEmpty(thesisRecord.StudentId) && String.IsNullOrEmpty(invRecord.StudentNo))
        //        viewModel.StudentIdInputColor = CONFLICT_VALUE_COLOR;
        //    else if (String.IsNullOrEmpty(thesisRecord.StudentId) && !String.IsNullOrEmpty(invRecord.StudentNo))
        //    {
        //        thesisRecord.StudentId = invRecord.StudentNo;
        //        viewModel.StudentIdInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.StudentIdInputColor = thesisRecord.StudentId.Equals(invRecord.StudentNo) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Degree
        //    if (String.IsNullOrEmpty(thesisRecord.Degree))
        //        viewModel.DegreeInputColor = CONFLICT_VALUE_COLOR;
        //    else
        //        viewModel.DegreeInputColor = EMPTY_VALUE_COLOR;

        //    // Year
        //    if (thesisRecord.Year == null)
        //        thesisRecord.Year = "";
        //    if (String.IsNullOrEmpty(thesisRecord.Year) && String.IsNullOrEmpty(invRecord.GradYear))
        //        viewModel.YearInputColor = CONFLICT_VALUE_COLOR;
        //    else if (String.IsNullOrEmpty(thesisRecord.Year) && !String.IsNullOrEmpty(invRecord.GradYear))
        //    {
        //        thesisRecord.Year = invRecord.GradYear;
        //        viewModel.YearInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.YearInputColor = thesisRecord.Year.Equals(invRecord.GradYear) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Programme
        //    if (thesisRecord.Programme == null)
        //        thesisRecord.Programme = "";
        //    if (String.IsNullOrEmpty(thesisRecord.Programme) && String.IsNullOrEmpty(invRecord.Program))
        //        viewModel.ProgramInputColor = CONFLICT_VALUE_COLOR;
        //    else if (String.IsNullOrEmpty(thesisRecord.Programme) && !String.IsNullOrEmpty(invRecord.Program))
        //    {
        //        thesisRecord.Programme = invRecord.Program;
        //        viewModel.ProgramInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.ProgramInputColor = thesisRecord.Programme.Equals(invRecord.Program) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Title
        //    if (thesisRecord.ThesisTitle == null)
        //        thesisRecord.ThesisTitle = "";
        //    if (String.IsNullOrEmpty(thesisRecord.ThesisTitle) && String.IsNullOrEmpty(invRecord.Title))
        //        viewModel.TitleInputColor = CONFLICT_VALUE_COLOR;
        //    else if (String.IsNullOrEmpty(thesisRecord.ThesisTitle) && !String.IsNullOrEmpty(invRecord.Title))
        //    {
        //        thesisRecord.ThesisTitle = invRecord.Title;
        //        viewModel.TitleInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.TitleInputColor = thesisRecord.ThesisTitle.Equals(invRecord.Title) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Access
        //    if (String.IsNullOrEmpty(thesisRecord.AccessMode) && String.IsNullOrEmpty(invRecord.Access))
        //    {
        //        thesisRecord.AccessMode = InventoryStore.ACCESS_MODE_RESTRICTED_KEY;
        //        viewModel.AccessInputColor = CONFLICT_VALUE_COLOR;
        //    }
        //    else if (String.IsNullOrEmpty(thesisRecord.AccessMode) && !String.IsNullOrEmpty(invRecord.Access))
        //    {
        //        thesisRecord.AccessMode = invRecord.Access;
        //        viewModel.AccessInputColor = EMPTY_VALUE_COLOR;
        //    }
        //    else
        //        viewModel.AccessInputColor = thesisRecord.AccessMode.Equals(invRecord.Access) ? MATCH_VALUE_COLOR : CONFLICT_VALUE_COLOR;

        //    // Received Date
        //    if (invRecord.Received != null)
        //        thesisRecord.ReceivedDate = DateTime.ParseExact(invRecord.Received, DEFAULT_DATE_FORMAT, CultureInfo.InvariantCulture);
        //    else
        //        thesisRecord.ReceivedDate = DateTime.Now;
        //    viewModel.ReceivedDateInputColor = EMPTY_VALUE_COLOR;
        //}

        public List<InventoryRecord> getMatchingInventoryRecordList(ThesisRecord thesisRecord)
        {
            List<InventoryRecord> matchingInventoryRecordList = new List<InventoryRecord>();

            if (thesisRecord == null || (String.IsNullOrEmpty(thesisRecord.ChineseName) && String.IsNullOrEmpty(thesisRecord.EnglishName) && String.IsNullOrEmpty(thesisRecord.StudentId)
                && String.IsNullOrEmpty(thesisRecord.Degree) && String.IsNullOrEmpty(thesisRecord.Year) && String.IsNullOrEmpty(thesisRecord.Programme)
                && String.IsNullOrEmpty(thesisRecord.ThesisTitle) && String.IsNullOrEmpty(thesisRecord.AccessMode) && thesisRecord.ReceivedDate == null))
                return matchingInventoryRecordList;

            DataTable inventoryDataTable = InventoryStore.getInstance().InventoryDataTable;
            List<DataRow> matchingDataRows = new List<DataRow>();

            string filterString = "";

            if (!String.IsNullOrEmpty(thesisRecord.StudentId))
                filterString += "(STUDENT_NO LIKE '%" + thesisRecord.StudentId + "') OR ";

            filterString += "(RECORD_ID <> ''";
            if (!String.IsNullOrEmpty(thesisRecord.ChineseName))
                filterString += " AND C_NAME LIKE '%" + thesisRecord.ChineseName + "%'";
            if (!String.IsNullOrEmpty(thesisRecord.EnglishName))
                filterString += " AND NAME LIKE '%" + thesisRecord.EnglishName + "%'";
            if (!String.IsNullOrEmpty(thesisRecord.StudentId))
                filterString += " AND STUDENT_NO LIKE '%" + thesisRecord.StudentId + "%'";
            /*if (!String.IsNullOrEmpty(thesisRecord.Degree))
                filterString += " AND DEGREE LIKE '%" + thesisRecord.Degree + "%'";*/
            if (!String.IsNullOrEmpty(thesisRecord.Year))
                filterString += " AND GRAD_YEAR LIKE '%" + thesisRecord.Year + "%'";
            /*if (!String.IsNullOrEmpty(thesisRecord.Programme))
                filterString += " AND PROGRAM LIKE '%" + thesisRecord.Programme + "%'";*/
            /*if (!String.IsNullOrEmpty(thesisRecord.ThesisTitle))
                filterString += " AND TITLE LIKE '%" + thesisRecord.ThesisTitle + "%'";*/
            if (!String.IsNullOrEmpty(thesisRecord.AccessMode))
                filterString += " AND ACCESS LIKE '%" + thesisRecord.AccessMode + "%'";
            if (thesisRecord.ReceivedDate != null)
                filterString += " AND RECEIVED LIKE '%" + thesisRecord.ReceivedDate.ToString() + "%'";
            if (filterString.Length > 0)
            {
                filterString += ")";
                matchingDataRows.AddRange(inventoryDataTable.Select(filterString, "STUDENT_NO ASC"));
            }

            foreach (DataRow row in matchingDataRows)
            {
                InventoryRecord invRecord = new InventoryRecord();
                invRecord.RecordId = (string)row["RECORD_ID"];
                invRecord.Name = (string)row["NAME"];
                invRecord.ChineseName = (string)row["C_NAME"];
                invRecord.StudentNo = (string)row["STUDENT_NO"];
                invRecord.Program = (string)row["PROGRAM"];
                invRecord.Title = (string)row["TITLE"];
                invRecord.GradYear = (string)row["GRAD_YEAR"];
                invRecord.Degree = (string)row["DEGREE"];
                invRecord.Access = (string)row["ACCESS"];
                invRecord.BoxNo = (int)row["BOX_NO"];
                invRecord.Assign = (string)row["ASSIGN"];
                try
                {
                    if (row["RECEIVED"] != null)
                    {
                        DateTime receivedDateTime = (DateTime)row["RECEIVED"];
                        invRecord.Received = receivedDateTime.ToString(DEFAULT_DATE_FORMAT);
                    }
                    if (row["GCEXCO_APPROVAL_DATE"] != null)
                    {
                        DateTime approvalDateTime = (DateTime)row["GCEXCO_APPROVAL_DATE"];
                        invRecord.ApprovalDate = approvalDateTime.ToString(DEFAULT_DATE_FORMAT);
                    }
                }
                catch (Exception)
                {
                    /*Do nothing*/
                }
                
                invRecord.Embargo = (string)row["EMBARGO"];
                invRecord.Release = (string)row["RELEASE"];
                matchingInventoryRecordList.Add(invRecord);
            }
            
            return matchingInventoryRecordList;
        }

        public void determineThesisRecord(ThesisRecord thesisRecord)
        {
            ViewModel viewModel = ViewModel.getInstance();
            if (viewModel.SelectedThesisFile != null)
            {
                string fileName = viewModel.SelectedThesisFile.FileInfo.Name;

                Match studentIdMatch = STUDENT_ID_REGEX.Match(fileName);
                if (studentIdMatch.Success)
                    thesisRecord.StudentId = studentIdMatch.Value;

                Match studentNameMatch = STUDENT_NAME_REGEX.Match(fileName);
                if (studentNameMatch.Success)
                {
                    string matchPattern = studentNameMatch.Value;
                    thesisRecord.EnglishName = matchPattern.Substring(11, matchPattern.Length - 12);
                }
                if (thesisRecord.PdfTexts != null && !"".Equals(thesisRecord.PdfTexts))
                    ThesisFrontPageAnalyser.retrieveThesisRecord(thesisRecord.PdfTexts, thesisRecord);
            }
        }

        public void loadPDF(string pdfFileName)
        {
            PdfReaderHelper.getInstance().load(pdfFileName);

            string parseResult = parsePDFTexts(pdfFileName);
            parseResult = Regex.Replace(parseResult, @"([^\S\r\n]*(\r\n|\r|\n)[^\S\r\n]*(\r\n|\r|\n))+", "\n");

            bool isOCRResult = false;
            if ((parseResult == null || "".Equals(parseResult)) && ReaderConfig.getInstance().EnableOCR)
            {
                parseResult = ocrPDFTexts(pdfFileName);
                isOCRResult = true;
            }

            if (parseResult == null || "".Equals(parseResult))
            {
                alert("Failed to extract any text from file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ViewModel.getInstance().OcrBackColor = isOCRResult ? Color.LightBlue : Color.White;
            MainThread.getInstance().ThesisRecord.PdfTexts = parseResult;
        }

        public string parsePDFTexts(string pdfFileName)
        {
            PdfMinerHelper pdfMinderHelper = new PdfMinerHelper(ReaderConfig.getInstance().PythonPath, ReaderConfig.getInstance().PdfminerPath);
            return pdfMinderHelper.retrievePdfTexts(pdfFileName);
        }

        public string ocrPDFTexts(string pdfFileName)
        {
            string ghostScriptPath = ReaderConfig.getInstance().GhostscriptPath;
            if (ghostScriptPath == null || "".Equals(ghostScriptPath) || !File.Exists(ghostScriptPath))
            {
                alert("Invalid Ghostscript Path: " + ghostScriptPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string tesseractPath = ReaderConfig.getInstance().TesseractPath;
            if (tesseractPath == null || "".Equals(tesseractPath) || !File.Exists(tesseractPath))
            {
                alert("Invalid Tesseeract Path: " + tesseractPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string ghostScriptParam = "-q -dNOPAUSE -dFirstPage=1 -dLastPage=1 -sDEVICE=tifflzw -r300 -sOutputFile=temp.tif " + "\"" + pdfFileName + "\"" + " -c quit";
            CmdHelper.run(ghostScriptPath, ghostScriptParam);
            if (!File.Exists(@"temp.tif"))
            {
                alert("Error converting pdf to tiff", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string tesseractParam = "temp.tif tsrout";

            CmdHelper.run(tesseractPath, tesseractParam);
            if (!File.Exists(@"tsrout.txt"))
            {
                alert("Error reading ocr output", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            string parseResult = File.ReadAllText(@"tsrout.txt");
            File.Delete(@"temp.tif");
            File.Delete(@"tsrout.txt");
            return parseResult;
        }

        protected MaskedFileInfo expectedFileLookup(DiscContents contents)
        {
            List<MaskedFileInfo> potentialFileList = new List<MaskedFileInfo>();
            MaskedFileInfo largestPdfFile = null;

            foreach (MaskedFileInfo maskedFileInfo in contents.MaskedFileInfoList)
            {
                if (maskedFileInfo == null)
                    continue;
                FileInfo fileInfo = maskedFileInfo.FileInfo;
                string filePathLower = fileInfo.FullName.ToLower();

                if (EXPECTED_FILE_PATTERN_REGEX.IsMatch(filePathLower))
                    potentialFileList.Add(maskedFileInfo);

                if (filePathLower.EndsWith("pdf") && fileInfo.Length > (largestPdfFile == null ? 0 : largestPdfFile.FileInfo.Length))
                    largestPdfFile = maskedFileInfo;
            }
            if (potentialFileList.Count > 0)
            {
                MaskedFileInfo largerPdfFile = null;
                foreach (MaskedFileInfo potentialFileInfo in potentialFileList)
                {
                    if (potentialFileInfo.FileInfo.Length > (largerPdfFile == null ? 0 : largerPdfFile.FileInfo.Length))
                        largerPdfFile = potentialFileInfo;
                }
                return largerPdfFile;
            }

            if (largestPdfFile != null)
                return largestPdfFile;

            return null;
        }

        protected string cleanUpExtractedText(string extractedText)
        {
            string resultText = "";
            using (StringReader reader = new StringReader(extractedText))
            {
                string line = null;
                int previousEmptyLineCount = 0;
                string lastTextSegment = "";
                bool startNewLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if ("".Equals(line))
                        previousEmptyLineCount++;
                    else
                    {
                        if (previousEmptyLineCount >= 2)
                        {
                            resultText += "\n" + lastTextSegment + "\n";
                            startNewLine = true;
                        }
                        else
                        {
                            resultText += (startNewLine ? "" : " ") + lastTextSegment;
                            startNewLine = false;
                        }

                        lastTextSegment = line;
                        previousEmptyLineCount = 0;
                    }
                }
                if (previousEmptyLineCount >= 2)
                    resultText += "\n" + lastTextSegment + "\n";
                else
                    resultText += (resultText.Length == 0 ? "" : " ") + lastTextSegment;
            }
            return resultText.Trim();
        }

        protected void alert(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Panel msgBoxOwner = PdfReaderHelper.getInstance().ContainerPanel;
            if (msgBoxOwner.InvokeRequired)
            {
                msgBoxOwner.BeginInvoke((MethodInvoker)delegate ()
                {
                    MessageBox.Show(msgBoxOwner, text, caption, buttons, icon);
                });
            }
            else
                MessageBox.Show(msgBoxOwner, text, caption, buttons, icon);
        }
    }
}
