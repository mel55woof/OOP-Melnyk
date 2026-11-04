using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab13v16
{

    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int Views { get; set; } 

        public BlogPost(int id, string title, string content, int authorId, int views)
        {
            Id = id;
            Title = title;
            Content = content;
            AuthorId = authorId;
            Views = views;
        }

        public override string ToString() => $"[Post #{Id}] '{Title}' (Переглядів: {Views}, Автор ID: {AuthorId})";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №13: Generic-методи та функції вищого порядку (Варіант 16) ===\n");

           
            var numbers = new List<int> { 5, 12, 3, 22, 8, 15 };
            Console.WriteLine($"Початкові числа: {string.Join(", ", numbers)}");

            var mappedNums = numbers.Map(n => n * 10);
            Console.WriteLine($"1. Map (множення на 10): {string.Join(", ", mappedNums)}");

            var filteredNums = numbers.Filter(n => n > 10);
            Console.WriteLine($"2. Filter (тільки > 10): {string.Join(", ", filteredNums)}");

            var maxNum = numbers.MaxBy(n => n);
            Console.WriteLine($"3. MaxBy (найбільше число): {maxNum}");

            var avgNum = numbers.Average(n => n);
            Console.WriteLine($"4. Average (середнє значення): {avgNum}");

            var hasLarge = numbers.Any(n => n > 50);
            Console.WriteLine($"5. Any (чи є число > 50?): {hasLarge}\n");

        
            Console.WriteLine("--- Демонстрація на колекції BlogPost ---");
            var posts = new List<BlogPost>
            {
                new BlogPost(1, "Intro to C#", "Basics of C# language", 101, 150),
                new BlogPost(2, "Advanced Generics", "Deep dive into generics", 102, 320),
                new BlogPost(3, "LINQ Queries", "How to use LINQ effectively", 101, 85),
                new BlogPost(4, "Delegates & Events", "Understanding Func and Action", 103, 410)
            };

            foreach (var post in posts)
            {
                Console.WriteLine(post);
            }

            
            var titles = posts.Map(p => p.Title);
            Console.WriteLine($"\n[Map] Заголовки постів: {string.Join(" | ", titles)}");

          
            var popularPosts = posts.Filter(p => p.Views > 100);
            Console.WriteLine($"\n[Filter] Популярні пости (>100 переглядів):");
            foreach (var p in popularPosts) Console.WriteLine($" - {p.Title} ({p.Views} переглядів)");

          
            var mostPopular = posts.MaxBy(p => p.Views);
            Console.WriteLine($"\n[MaxBy] Пост із найбільшою кількістю переглядів: {mostPopular.Title} ({mostPopular.Views})");

   
            double avgViews = posts.Average(p => p.Views);
            Console.WriteLine($"\n[Average] Середня кількість переглядів: {avgViews}");


            bool hasAuthor103 = posts.Any(p => p.AuthorId == 103);
            Console.WriteLine($"\n[Any] Чи є пости автора ID 103? {hasAuthor103}");

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}

