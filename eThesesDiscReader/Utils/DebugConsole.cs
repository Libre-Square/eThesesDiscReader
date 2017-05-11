using eThesesDiscReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Utils
{
    public class DebugConsole
    {
        public static void WriteLine(string text)
        {
            ViewModel.getInstance().ConsoleText += (text + Environment.NewLine);
        }
    }
}
