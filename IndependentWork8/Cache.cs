using System.Collections.Generic;

namespace IndependentWork8
{
    public class Cache<TKey, TValue> where TKey : notnull
    {
        private readonly int _capacity;
        private readonly Dictionary<TKey, TValue> _cache;
        private readonly Queue<TKey> _keysOrder;

        public Cache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new System.ArgumentException("Ємність кешу має бути більшою за нуль.", nameof(capacity));
            }

            _capacity = capacity;
            _cache = new Dictionary<TKey, TValue>(_capacity);
            _keysOrder = new Queue<TKey>(_capacity);
        }

        public int Count => _cache.Count;

        public void Add(TKey key, TValue value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
                return;
            }

            if (_cache.Count >= _capacity)
            {
                if (_keysOrder.Count > 0)
                {
                    TKey oldestKey = _keysOrder.Dequeue();
                    _cache.Remove(oldestKey);
                }
            }

            _cache[key] = value;
            _keysOrder.Enqueue(key);
        }

        public TValue Get(TKey key)
        {
            if (!_cache.TryGetValue(key, out var value))
            {
                throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");
            }

            return value;
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }
    }
}
