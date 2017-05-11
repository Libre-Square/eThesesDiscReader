using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace eThesesDiscReader
{
    public partial class PlainRichTextBox : RichTextBox
    {
        private string originalText = string.Empty;
        private bool _keepLineBreaks = false;

        public PlainRichTextBox()
        {
            InitializeComponent();
            this.TextChanged += new EventHandler(this.this_TextChanged);
        }

        public bool KeepLineBreaks
        {
            get { return _keepLineBreaks; }
            set
            {
                if (value != _keepLineBreaks)
                {
                    _keepLineBreaks = value;
                }
            }
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            originalText = this.Text;
        }

        private void this_TextChanged(object sender, EventArgs e)
        {
            int cursorPos = this.SelectionStart;
            this.Font = new System.Drawing.Font("Arial Unicode MS", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            string normalizedText = this.Text;
            string normalizedTextPrefix = "";

            if (cursorPos >= 0 && normalizedText != null && normalizedText.Length > 0)
            {
                normalizedTextPrefix = normalizedText.Substring(0, cursorPos);
                normalizedText = normalizedText.Substring(cursorPos);
            }

            if (!KeepLineBreaks)
            {
                normalizedTextPrefix = Regex.Replace(normalizedTextPrefix, @"\r\n?|\n", " ");
                normalizedText = Regex.Replace(normalizedText, @"\r\n?|\n", " ");
            }

            normalizedTextPrefix = Regex.Replace(normalizedTextPrefix, @"[ ]+", " ");
            normalizedText = Regex.Replace(normalizedText, @"[ ]+", " ");

            normalizedTextPrefix = Regex.Replace(normalizedTextPrefix, @"^[ ]+", "");
            normalizedText = Regex.Replace(normalizedText, @"^[ ]+", "");

            normalizedTextPrefix = Regex.Replace(normalizedTextPrefix, @"[ ]+$", " ");
            normalizedText = Regex.Replace(normalizedText, @"[ ]+$", "");

            normalizedText = normalizedTextPrefix + normalizedText;

            if (!normalizedText.Equals(this.Text))
            {
                this.Text = normalizedText;
                cursorPos = normalizedTextPrefix.Length;
            }
            if (!originalText.Equals(this.Text))
                this.SelectionStart = cursorPos;

        }
    }
}
