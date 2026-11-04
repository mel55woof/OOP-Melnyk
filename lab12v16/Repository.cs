using System.Collections.Generic;
using System.Linq;

namespace lab12v16
{
    public interface IRepository<T> where T : IEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }

    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new List<T>();

        public T? GetById(int id)
{
    return _items.FirstOrDefault(x => x.Id == id);
}

        public IEnumerable<T> GetAll()
        {
            return _items.ToList();
        }

        public void Add(T item)
        {
            if (item == null) return;
            // Перевірка на унікальність ID
            if (_items.Any(x => x.Id == item.Id))
            {
                throw new System.ArgumentException("Елемент з таким ID вже існує.");
            }
            _items.Add(item);
        }

        public void Update(T item)
        {
            if (item == null) return;
            var existingItem = GetById(item.Id);
            if (existingItem == null)
            {
                throw new System.Collections.Generic.KeyNotFoundException("Елемент для оновлення не знайдено.");
            }
            
            int index = _items.IndexOf(existingItem);
            _items[index] = item;
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
            }
            else
            {
                throw new System.Collections.Generic.KeyNotFoundException("Елемент для видалення не знайдено.");
            }
        }
    }
}
