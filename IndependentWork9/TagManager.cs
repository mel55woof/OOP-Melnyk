using System;
using System.Collections.Generic;

namespace IndependentWork9
{
    public class TagManager
    {
        private readonly Dictionary<string, HashSet<string>> _articleTags = new(StringComparer.OrdinalIgnoreCase);

        public void AddTag(string articleId, string tag)
        {
            if (!_articleTags.TryGetValue(articleId, out var tags))
            {
                tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                _articleTags[articleId] = tags;
            }
            tags.Add(tag);
        }

        public void RemoveTag(string articleId, string tag)
        {
            if (_articleTags.TryGetValue(articleId, out var tags))
            {
                tags.Remove(tag);
            }
        }

        public HashSet<string> GetAllUniqueTags()
        {
            var allTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var tags in _articleTags.Values)
            {
                allTags.UnionWith(tags);
            }
            return allTags;
        }

        public HashSet<string> FindCommonTags(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1) ||
                !_articleTags.TryGetValue(articleId2, out var tags2))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var common = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            common.IntersectWith(tags2);
            return common;
        }

        public HashSet<string> FindUniqueToFirstArticle(string articleId1, string articleId2)
        {
            if (!_articleTags.TryGetValue(articleId1, out var tags1))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var diff = new HashSet<string>(tags1, StringComparer.OrdinalIgnoreCase);
            if (_articleTags.TryGetValue(articleId2, out var tags2))
            {
                diff.ExceptWith(tags2);
            }
            return diff;
        }
    }
}
