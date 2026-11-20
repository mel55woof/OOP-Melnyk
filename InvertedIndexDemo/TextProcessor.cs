using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace InvertedIndexDemo
{
    public static class TextProcessor
    {
        private static readonly Regex WordRegex = new Regex(@"\w+", RegexOptions.Compiled);

        public static IEnumerable<string> Tokenize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                yield break;

            foreach (Match match in WordRegex.Matches(text.ToLowerInvariant()))
            {
                yield return match.Value;
            }
        }

        public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
        {
            var stopWords = new HashSet<string>
            {
                "a", "an", "the", "is", "are", "was", "were",
                "in", "on", "at", "to", "for", "of", "and", "or"
            };
            return words.Where(w => !stopWords.Contains(w));
        }
    }
}
