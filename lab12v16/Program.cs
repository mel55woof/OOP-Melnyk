using System;
using System.Text;

namespace lab12v16
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Демонстрація роботи лаб. 12 (Варіант 16: Generics & IRepository) ===\n");

            IRepository<BlogPost> repository = new InMemoryRepository<BlogPost>();

            // Додавання
            repository.Add(new BlogPost(1, "Перший пост", "Вміст першого блог-посту...", 101));
            repository.Add(new BlogPost(2, "CSharp Generics", "Вивчення узагальнених типів у C#...", 102));

            Console.WriteLine($"Всього постів у репозиторії:");
            foreach (var post in repository.GetAll())
            {
                Console.WriteLine($"- [{post.Id}] {post.Title} (Автор ID: {post.AuthorId})");
            }

            // Оновлення
            var postToUpdate = repository.GetById(1);
            if (postToUpdate != null)
            {
                postToUpdate.Title = "Оновлений перший пост";
                repository.Update(postToUpdate);
                Console.WriteLine($"\n[Оновлено] Пост #1 тепер має назву: '{repository.GetById(1).Title}'");
            }

            // Видалення
            repository.Delete(2);
            Console.WriteLine($"\n[Видалено] Пост #2 видалено. Загальна кількість: {repository.GetAll().Count()}");

            Console.WriteLine("\n=== Роботу програми завершено ===");
        }
    }
}
