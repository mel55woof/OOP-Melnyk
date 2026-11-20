using System;
using System.Collections.Generic;
using System.Linq;

namespace InvertedIndexDemo
{
    public class InvertedIndex
    {
        private readonly Dictionary<string, HashSet<int>> _index = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<int, Document> _documents = new();

        public int DocumentCount => _documents.Count;
        public int UniqueWords => _index.Keys.Count;

        public void AddDocument(Document doc)
        {
            if (_documents.ContainsKey(doc.Id)) return;

            _documents[doc.Id] = doc;
            var tokens = TextProcessor.Tokenize(doc.Content);
            var filteredTokens = TextProcessor.RemoveStopWords(tokens);

            foreach (var word in filteredTokens)
            {
                if (!_index.TryGetValue(word, out var docIds))
                {
                    docIds = new HashSet<int>();
                    _index[word] = docIds;
                }
                docIds.Add(doc.Id);
            }
        }

        public List<Document> SearchSingleWord(string word)
        {
            var normalized = word.ToLowerInvariant();
            if (_index.TryGetValue(normalized, out var docIds))
            {
                return docIds.Select(id => _documents[id]).ToList();
            }
            return new List<Document>();
        }

        public List<Document> SearchMultipleWords(params string[] words)
        {
            if (words == null || words.Length == 0) return new List<Document>();

            var normalizedWords = words.Select(w => w.ToLowerInvariant()).ToArray();
            HashSet<int>? resultSet = null;

            foreach (var word in normalizedWords)
            {
                if (!_index.TryGetValue(word, out var docIds))
                {
                    return new List<Document>(); // Якщо хоча б одного слова немає, перетин порожній
                }

                if (resultSet == null)
                {
                    resultSet = new HashSet<int>(docIds);
                }
                else
                {
                    resultSet.IntersectWith(docIds);
                }

                if (resultSet.Count == 0) break;
            }

            return resultSet == null ? new List<Document>() : resultSet.Select(id => _documents[id]).ToList();
        }

        public List<Document> SearchAnyWord(params string[] words)
        {
            if (words == null || words.Length == 0) return new List<Document>();

            var normalizedWords = words.Select(w => w.ToLowerInvariant()).ToArray();
            var resultSet = new HashSet<int>();

            foreach (var word in normalizedWords)
            {
                if (_index.TryGetValue(word, out var docIds))
                {
                    resultSet.UnionWith(docIds);
                }
            }

            return resultSet.Select(id => _documents[id]).ToList();
        }
    }
}
