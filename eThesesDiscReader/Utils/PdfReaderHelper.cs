using AxAcroPDFLib;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader.Utils
{
    public class PdfReaderHelper
    {
        private static PdfReaderHelper _instance = new PdfReaderHelper();
        private Panel _containerPanel;
        private PdfRenderer pdfRenderer;
        private AxAcroPDF axAcroPDF;

        public static PdfReaderHelper getInstance()
        {
            return _instance;
        }

        public Panel ContainerPanel
        {
            get { return _containerPanel; }
            set { _containerPanel = value; }
        }

        public void init(bool useAcroPDF)
        {
            if (useAcroPDF && this.axAcroPDF == null)
            {
                this.axAcroPDF = new AxAcroPDF();
                initAxAcroPDF();
            }
            else if (!useAcroPDF && this.pdfRenderer == null)
            {
                this.pdfRenderer = new PdfRenderer();
                initPdfRenderer();
            }
        }

        public void load(string filePath)
        {
            for (int retry = 0; !this._containerPanel.IsHandleCreated && retry < 10; retry++)
            {
                Thread.Sleep(200);
            }
            if (!this._containerPanel.IsHandleCreated)
                return;

            this._containerPanel.BeginInvoke((MethodInvoker)delegate ()
            {
                if (this.axAcroPDF != null)
                {
                    this.axAcroPDF.LoadFile(filePath);
                    this.axAcroPDF.src = filePath;
                    this.axAcroPDF.setShowToolbar(false);
                    this.axAcroPDF.setPageMode("none");
                    this.axAcroPDF.setLayoutMode("SinglePage");
                    this.axAcroPDF.setView("FitW");
                }
                else if (this.pdfRenderer != null)
                {
                    PdfDocument doc = PdfDocument.Load(filePath);
                    this.pdfRenderer.Load(doc);
                }
            });
        }

        public void unload()
        {
            for (int retry = 0; !this._containerPanel.IsHandleCreated && retry < 10; retry++)
            {
                Thread.Sleep(200);
            }
            if (!this._containerPanel.IsHandleCreated)
                return;

            this._containerPanel.BeginInvoke((MethodInvoker)delegate ()
            {
                if (this.axAcroPDF != null)
                {
                    string dummyFilePath = FileUtils.DEFAULT_TEMP_DIRECTORY + @"dummy.pdf";
                    this.axAcroPDF.LoadFile(dummyFilePath);
                }
            });
        }

        private void initAxAcroPDF()
        {
            this._containerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF)).BeginInit();
            this.axAcroPDF.CreateControl();
            this.axAcroPDF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAcroPDF.Enabled = true;
            this.axAcroPDF.Location = new System.Drawing.Point(0, 0);
            this.axAcroPDF.Name = "axAcroPDF";
            this.axAcroPDF.OcxState = ((System.Windows.Forms.AxHost.State)(new ComponentResourceManager(typeof(MainForm)).GetObject("axAcroPDF.OcxState")));
            this.axAcroPDF.TabIndex = 0;
            this._containerPanel.Controls.Add(this.axAcroPDF);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF)).EndInit();
            this._containerPanel.ResumeLayout(false);
        }

        private void initPdfRenderer()
        {
            this._containerPanel.SuspendLayout();
            this.pdfRenderer.Cursor = System.Windows.Forms.Cursors.Default;
            this.pdfRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfRenderer.Location = new System.Drawing.Point(0, 0);
            this.pdfRenderer.Name = "pdfRenderer";
            this.pdfRenderer.Page = 1;
            this.pdfRenderer.Size = new System.Drawing.Size(this._containerPanel.Width, this._containerPanel.Height);
            this.pdfRenderer.TabIndex = 0;
            this.pdfRenderer.BackColor = Color.DarkGray;
            this.pdfRenderer.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitWidth;
            this._containerPanel.Controls.Add(this.pdfRenderer);
            this._containerPanel.ResumeLayout(false);
        }
    }
}
