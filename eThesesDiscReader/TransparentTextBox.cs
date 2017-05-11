using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eThesesDiscReader
{
    public partial class TransparentTextBox : TextBox
    {
        public TransparentTextBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Default;
            this.TextChanged += new EventHandler(this.this_TextChanged);
        }

        private void this_TextChanged(object sender, EventArgs e)
        {
            if (this.TextLength > 0)
                this.ScrollBars = ScrollBars.Both;
            else
                this.ScrollBars = ScrollBars.None;
        }
    }
}
