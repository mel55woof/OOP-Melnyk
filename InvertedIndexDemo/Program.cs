using System;
using System.Collections.Generic;
using System.Text;

namespace InvertedIndexDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №11: Інвертований індекс і продуктивність ===\n");

            // Запуск тестів продуктивності для різних обсягів
            PerformanceTest.RunAllTests();

            // Демонстрація на базовому наборі
            var index = new InvertedIndex();
            var baseDocs = PerformanceTest.GenerateDocuments(5);
            baseDocs[0] = new Document { Id = 1, Title = "Introduction to C#", Content = "C# is a modern object-oriented programming language developed by Microsoft." };
            baseDocs[1] = new Document { Id = 2, Title = "Java Basics", Content = "Java is a popular programming language known for its portability and performance." };
            baseDocs[2] = new Document { Id = 3, Title = "Python Overview", Content = "Python is a high-level programming language with simple syntax." };
            baseDocs[3] = new Document { Id = 4, Title = "C# Advanced Features", Content = "C# supports LINQ, async/await, and many modern programming paradigms." };
            baseDocs[4] = new Document { Id = 5, Title = "Comparing Languages", Content = "C#, Java, and Python are among the most popular programming languages today." };

            foreach (var doc in baseDocs) index.AddDocument(doc);

            Console.WriteLine($"Loaded {index.DocumentCount} documents");
            Console.WriteLine($"Index contains {index.UniqueWords} unique words\n");

            // Інтерактивний пошук
            while (true)
            {
                Console.Write("Enter search query (or 'exit'): ");
                var query = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(query) || query.ToLower() == "exit") break;

                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine("\n--- Search Results (AND) ---");
                var results = index.SearchMultipleWords(words);
                
                foreach (var doc in results)
                {
                    Console.WriteLine(doc);
                    Console.WriteLine($"   {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
                }
                Console.WriteLine($"\nFound {results.Count} documents\n");
            }
        }
    }
}
