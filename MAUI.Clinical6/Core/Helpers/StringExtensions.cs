using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Contains Html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ContainsHtml(this string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            Regex containsHtmlRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");
            bool containsHtml = containsHtmlRegex.IsMatch(str);
            return containsHtml;
        }

        public static string StripHtml(this string str)
        {
            var tagPattern = new Regex(@"<\s*\/?\w+(\s*\w+\s*=\s*""[^""]+""\s*)*\s*\/?>");
            var stripped = tagPattern.Replace(str, string.Empty);
            return System.Net.WebUtility.HtmlDecode(stripped);
        }

        public static bool HasStringFormat(this string s)
        {
            return Regex.IsMatch(s, "{\\d+}");
        }

        /// <summary>
        /// Returns a Tuple for if first non latin character found and it's index
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Tuple<bool, int> CheckForNonLatinCharacters(this string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                int code = str[i];
                if (!(code >= 32 && code <= 126))
                    return new Tuple<bool, int>(true, i);
            }

            return new Tuple<bool, int>(false, -1);
        }

        static string regExp = @"\p{IsHangulJamo}|" + 
                               @"\p{IsCJKRadicalsSupplement}|" +
                               @"\p{IsCJKSymbolsandPunctuation}|" +
                               @"\p{IsEnclosedCJKLettersandMonths}|" +
                               @"\p{IsCJKCompatibility}|" +
                               @"\p{IsCJKUnifiedIdeographsExtensionA}|" +
                               @"\p{IsCJKUnifiedIdeographs}|" +
                               @"\p{IsHangulSyllables}|" +
                               @"\p{IsCJKCompatibilityForms}";

        private static readonly Regex cjkCharRegex = new Regex(regExp);

        public static bool IsCJK(this string str)
        {
            return str.Any(c => cjkCharRegex.IsMatch(c.ToString()));
        }

        /// <summary>
        /// Returns a string formatted based on the answers result type
        /// </summary>
        /// <param name="answerResultType"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static string FormatAnswerResultType(this string answerResultType, string answer)
        {
            switch (answerResultType)
            {
                case ResultType.MultiValue:
                    return answer.Replace(",", Environment.NewLine);
                case ResultType.DateValue:
                    return DateHelper.TryParseDateTime(answer, "d MMM yyyy");
                case ResultType.DateTimeValue:
                    return DateHelper.TryParseDateTime(answer, "d MMM yyyy | h:mm tt");
                case ResultType.TimeValue:
                    return DateHelper.TryParseDateTimeWithoutLocalTime(answer, "h:mm tt");
                default:
                    return answer;
            }
        }
    }
}