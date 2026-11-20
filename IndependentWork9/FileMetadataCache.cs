using System;
using System.Collections.Generic;

namespace IndependentWork9
{
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string Hash { get; set; } = string.Empty;
    }

    public class FileMetadataCache
    {
        private readonly Dictionary<string, FileMetadata> _cache = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _cache.Count;

        public void Add(string path, FileMetadata metadata)
        {
            _cache[path] = metadata;
        }

        public FileMetadata? Get(string path)
        {
            _cache.TryGetValue(path, out var metadata);
            return metadata;
        }

        public bool Remove(string path)
        {
            return _cache.Remove(path);
        }

        public bool Contains(string path)
        {
            return _cache.ContainsKey(path);
        }
    }
}
