using System;
using System.Globalization;

namespace iExporter.wpf.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World") using the Invariant Culture
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns>The string in title case</returns>
        public static string ToTitleCaseInvariant(this string s)
        {
            return ToTitleCase(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World")
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns>The string in title case</returns>
        public static string ToTitleCase(this string s)
        {
            return ToTitleCase(s, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Creates a new string with Title Case (ie "hEllO wORLd" becomes  "Hello World")
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <param name="ci">The culture to use when creating title case</param>
        /// <returns>The string in title case</returns>
        public static string ToTitleCase(this string s, CultureInfo ci)
        {
            if (s == null)
                throw new ArgumentNullException();

            return ci.TextInfo.ToTitleCase(s);
        }
    }
}
