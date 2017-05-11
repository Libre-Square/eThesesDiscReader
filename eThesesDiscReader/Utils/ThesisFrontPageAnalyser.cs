using eThesesDiscReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eThesesDiscReader.Utils
{
    public class ThesisFrontPageAnalyser
    {
        private static readonly string[] NEW_LINE_DELIMITERS = { Environment.NewLine, "\n" };
        private readonly static Regex THESIS_LINE = new Regex(@".*thesis.*submit.*in.*ful.*");
        private readonly static Regex REQUIREMENT_LINE = new Regex(@".*requirements.*for.*degree");
        private readonly static Regex UNIVERSITY_LINE = new Regex(@".*chinese university of hong kong.*");

        public static void retrieveThesisRecord(string frontPageTexts, ThesisRecord thesisRecord)
        {
            retrieveEnglishThesisRecord(frontPageTexts, thesisRecord);
        }

        protected static void retrieveEnglishThesisRecord(string frontPageTexts, ThesisRecord thesisRecord)
        {
            if (frontPageTexts != null && !"".Equals(frontPageTexts))
            {
                string[] lines = frontPageTexts.Split(NEW_LINE_DELIMITERS, StringSplitOptions.RemoveEmptyEntries);
                int thesisLineIndex = 0;
                int requirementLineIndex = 0;
                int universityLineIndex = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (THESIS_LINE.Match(lines[i].ToLower()).Success)
                        thesisLineIndex = i;
                    if (REQUIREMENT_LINE.Match(lines[i].ToLower()).Success)
                        requirementLineIndex = i;
                    if (UNIVERSITY_LINE.Match(lines[i].ToLower()).Success)
                        universityLineIndex = i;
                }
                if (thesisLineIndex > 0 && requirementLineIndex > thesisLineIndex && universityLineIndex > requirementLineIndex && universityLineIndex < lines.Length)
                {
                    for (int i = 0; i < thesisLineIndex - 1; i++)
                    {
                        thesisRecord.ThesisTitle += (i == 0 ? "" : " ") + lines[i];
                    }
                    thesisRecord.EnglishName = lines[thesisLineIndex - 1].Trim();
                    thesisRecord.Degree = lines[requirementLineIndex + 1].Trim();
                    thesisRecord.Programme = lines[universityLineIndex - 1].Trim();
                    string gradMonthYear = lines[universityLineIndex + 1].ToLower();
                    thesisRecord.Year = gradMonthYear.Replace("january", "").Replace("february", "").Replace("march", "").Replace("april", "").Replace("may", "").Replace("june", "").Replace("july", "").Replace("august", "").Replace("september", "").Replace("october", "").Replace("november", "").Replace("december", "").Trim();
                }
            }
        }
    }
}
