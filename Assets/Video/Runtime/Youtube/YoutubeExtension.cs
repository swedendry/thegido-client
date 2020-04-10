using System;
using System.Text.RegularExpressions;

namespace Video.Youtube
{
    public static class YoutubeExtension
    {
        public static string Url2Id(this string url)
        {
            return Regex.Match(url, @"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)").Groups[1].Value;
        }

        public static string SubstringUntil(this string str, string sub, StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? str : str.Substring(0, index);
        }

        public static string SubstringAfter(this string str, string sub, StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? string.Empty : str.Substring(index + sub.Length, str.Length - index - sub.Length);
        }
    }
}
