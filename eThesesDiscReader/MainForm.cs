using eThesesDiscReader.Controls;
using eThesesDiscReader.Models;
using eThesesDiscReader.Utils;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace eThesesDiscReader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.thesisRecordBindingSource.Add(MainThread.getInstance().ThesisRecord);
            this.viewModelBindingSource.Add(ViewModel.getInstance());
            this.readerConfigBindingSource.Add(ReaderConfig.getInstance());
            this.inventoryRecordBindingSource.Add(InventoryStore.getInstance().SelectedInventoryRecord);

            this.fileSelectListBox.DataSource = DiscContents.getInstance().MaskedFileInfoList;
            this.fileSelectListBox.DisplayMember = "MaskedFileName";
            this.allFileListBox.DataSource = DiscContents.getInstance().DisplayFileInfoList;
            this.allFileListBox.DisplayMember = "DisplayFileName";
            this.inventoryMatchesComboBox.DataSource = MainThread.getInstance().MatchingInventoryList;
            this.inventoryMatchesComboBox.DisplayMember = "DisplayString";

            ViewModel.getInstance().ProgressBar = this.progressBar;
            PdfReaderHelper.getInstance().ContainerPanel = this.pdfReaderPanel;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = Text + " - v" + version.Major + "." + version.Minor + "." + version.Build;

            this.chineseAbstractTextBox.KeepLineBreaks = true;
            this.englishAbstractTextBox.KeepLineBreaks = true;

            this.menuPanel.DataBindings.Add(new Binding("Visible", this.viewModelBindingSource, "ShowMenu", true, DataSourceUpdateMode.OnPropertyChanged));
            this.pdfTextsTextArea.DataBindings.Add(new Binding("Visible", this.viewModelBindingSource, "ShowPdfTexts", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void menuOKButton_Click(object sender, EventArgs e)
        {
            ReaderConfig.getInstance().SaveSettings();
            ViewModel.getInstance().ShowMenu = false;
        }

        private void fileSelectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.getInstance().SelectedThesisFile = (MaskedFileInfo)this.fileSelectListBox.SelectedItem;
            DiscContentsProcessor.getInstance().selectionChanged();
        }

        private void fileSelectListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (this.fileSelectListBox.Items.Count != DiscContents.getInstance().MaskedFileInfoList.Count)
                return;

            e.DrawBackground();
            if (ViewModel.getInstance().ExpectedThesisFile != null
                && ViewModel.getInstance().ExpectedThesisFile.Equals(((MaskedFileInfo)this.fileSelectListBox.Items[e.Index]))
                && !ViewModel.getInstance().ExpectedThesisFile.Equals(((MaskedFileInfo)this.fileSelectListBox.SelectedItem))
                )
                e.Graphics.FillRectangle(new SolidBrush(Color.Gold), e.Bounds);
            else
                e.DrawFocusRectangle();

            Brush textBrush = Brushes.Black;
            if (!this.fileSelectListBox.Enabled)
                textBrush = Brushes.Silver;
            e.Graphics.DrawString(((MaskedFileInfo)this.fileSelectListBox.Items[e.Index]).MaskedFileName, e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
        }

        private void inventoryMatchesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InventoryStore.getInstance().updateSelectedInventoryRecord((InventoryRecord)this.inventoryMatchesComboBox.SelectedItem);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            DiscContentsProcessor.getInstance().selectionChanged();
        }

        private void openAccessRadioButton_Click(object sender, EventArgs e)
        {
            RadioButton senderRadioButton = (RadioButton)sender;
            if (!senderRadioButton.Checked)
                senderRadioButton.Checked = true;
        }

        private void restrictedAccessRadioButton_Click(object sender, EventArgs e)
        {
            RadioButton senderRadioButton = (RadioButton)sender;
            if (!senderRadioButton.Checked)
                senderRadioButton.Checked = true;
        }

        private void confidentialAccessRadioButton_Click(object sender, EventArgs e)
        {
            RadioButton senderRadioButton = (RadioButton)sender;
            if (!senderRadioButton.Checked)
                senderRadioButton.Checked = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            ViewModel.getInstance().ProgressBarColor = Color.Yellow;
            DiscContentsProcessor.getInstance().save();
        }
    }
}
