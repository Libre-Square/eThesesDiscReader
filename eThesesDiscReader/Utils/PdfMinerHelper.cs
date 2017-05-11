using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Utils
{
    public class PdfMinerHelper
    {
        public static readonly string SIMPLIFIED_CHINESE_CODEC = "cp950";
        public static readonly string TRADITIONAL_CHINESE_CODEC = "cp936";
        public static readonly string UTF8_CODEC = "utf-8";
        private string pythonPath;
        private string pdfminerPath;
        private string pdf2TxtPath;

        public PdfMinerHelper(string pythonPath, string pdfminerPath)
        {
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string folderPath = Path.GetFullPath(Path.Combine(exePath, ".."));
            Directory.SetCurrentDirectory(folderPath);

            if (pythonPath == null || "".Equals(pythonPath))
                throw new Exception("Invalid Python Path: " + pythonPath);

            if (pdfminerPath == null || "".Equals(pdfminerPath) || !File.Exists(pythonPath))
                throw new Exception("Invalid pdf2txt Path: " + pdfminerPath);

            string pdf2TxtPath = pdfminerPath.EndsWith(@"\") ? pdfminerPath + @"pdf2txt.py" : pdfminerPath;
            if (!File.Exists(pdf2TxtPath))
                throw new Exception("Invalid pdf2txt Path: " + pdf2TxtPath);

            this.pythonPath = pythonPath;
            this.pdfminerPath = pdfminerPath;
            this.pdf2TxtPath = pdf2TxtPath;
        }

        public string retrievePdfTexts(string pdfFileName)
        {
            List<string> supportedCodecList = new List<string>();
            supportedCodecList.Add(UTF8_CODEC);
            supportedCodecList.Add(SIMPLIFIED_CHINESE_CODEC);
            supportedCodecList.Add(TRADITIONAL_CHINESE_CODEC);
            supportedCodecList.Add(null);

            string retrievedText = "";
            Dictionary<int, string> potentialTextMap = new Dictionary<int, string>();
            foreach (string codec in supportedCodecList)
            {
                retrievedText = retrievePdfTexts(pdfFileName, codec);
                if (retrievedText != null && retrievedText.Length > 0)
                {
                    int questionMarksCount = countCharacterInString(retrievedText, "?");
                    if (questionMarksCount <= 0)
                        return retrievedText;
                    if (!potentialTextMap.ContainsKey(questionMarksCount))
                        potentialTextMap.Add(questionMarksCount, retrievedText);
                }
            }
            if (potentialTextMap.Count > 0)
                return potentialTextMap.OrderBy(k => k.Key).First().Value;

            return "";
        }

        protected string retrievePdfTexts(string pdfFileName, string codec)
        {
            string codecOption = "";
            if (codec != null && !"".Equals(codec))
            {
                codecOption = "-c " + codec;
            }
            string pdf2TxtParam = "\"" + pdf2TxtPath + "\" -t text -m 1 " + codecOption + " \"" + pdfFileName + "\"";
            return CmdHelper.run(pythonPath, pdf2TxtParam).Trim();
        }

        protected int countCharacterInString(string sourceText, string character)
        {
            if (sourceText == null || sourceText.Length <= 0 || character == null || character.Length <= 0)
                return 0;
            return sourceText.Length - sourceText.Replace(character, "").Length;
        }
    }

    class RetrievedTextDetail
    {
        private int _questionMarksCount;
        private string _codec;
        private string _retrievedText;

        public int QuestionMarksCount
        {
            get { return _questionMarksCount; }
            set { _questionMarksCount = value; }
        }

        public string Codec
        {
            get { return _codec; }
            set { _codec = value; }
        }

        public string RetrievedText
        {
            get { return _retrievedText; }
            set { _retrievedText = value; }
        }
    }
}
