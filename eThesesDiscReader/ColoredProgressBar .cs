using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace eThesesDiscReader
{
    public class ColoredProgressBar : ProgressBar
    {
        public void SetColor(Color color)
        {
            int state = 1;
            if (color == Color.Red)
                state = 2;
            else if (color == Color.Yellow)
                state = 3;
            ModifyProgressBar.SetState(this, state);
        }

        public void SetValue(long value)
        {
            ModifyProgressBar.SetValue(this, value);
        }
    }

    public static class ModifyProgressBar
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        public static void SetState(this ProgressBar pBar, int state)
        {
            if (pBar.IsHandleCreated)
            {
                if (pBar.InvokeRequired)
                {
                    pBar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
                    });
                }
                else
                    SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
            }
        }

        public static void SetValue(this ProgressBar pBar, long value)
        {
            if (pBar.IsHandleCreated)
            {
                if (pBar.InvokeRequired)
                {
                    pBar.BeginInvoke((MethodInvoker)delegate ()
                    {
                        SendMessage(pBar.Handle, 1026, (IntPtr)value, IntPtr.Zero);
                    });
                }
                else
                    SendMessage(pBar.Handle, 1026, (IntPtr)value, IntPtr.Zero);
            }
        }
    }
}
