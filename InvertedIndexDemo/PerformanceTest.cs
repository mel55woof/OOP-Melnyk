using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InvertedIndexDemo
{
    public class PerformanceTest
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== Тестування продуктивності (Лінійний пошук vs Inverted Index) ===");
            int[] sizes = { 10, 100, 1000, 10000 };

            foreach (var size in sizes)
            {
                var docs = GenerateDocuments(size);
                string testWord = "programming";

                var sw = Stopwatch.StartNew();
                var linearResults = docs.Where(d => TextProcessor.Tokenize(d.Content).Contains(testWord)).ToList();
                sw.Stop();
                long linearTicks = sw.ElapsedTicks;

                var index = new InvertedIndex();
                foreach (var doc in docs) index.AddDocument(doc);

                sw.Restart();
                var indexResults = index.SearchSingleWord(testWord);
                sw.Stop();
                long indexTicks = sw.ElapsedTicks;

                Console.WriteLine($"[Обсяг: {size,5} док.] Лінійний пошук: {linearTicks,6} ticks | Inverted Index: {indexTicks,6} ticks | Знайдено: {indexResults.Count}");
            }
            Console.WriteLine();
        }

        public static List<Document> GenerateDocuments(int count)
        {
            var baseTexts = new[]
            {
                "C# is a modern object-oriented programming language developed by Microsoft.",
                "Java is a popular programming language known for its portability and performance.",
                "Python is a high-level programming language with simple syntax.",
                "C# supports LINQ, async/await, and many modern programming paradigms.",
                "C#, Java, and Python are among the most popular programming languages today."
            };

            var extraWords = new[] { "development", "software", "code", "engineer", "web", "data", "cloud", "system", "dotnet", "framework" };
            var list = new List<Document>();
            var rand = new Random(42);

            for (int i = 1; i <= count; i++)
            {
                string baseText = baseTexts[(i - 1) % baseTexts.Length];
                string extra = extraWords[rand.Next(extraWords.Length)] + " " + extraWords[rand.Next(extraWords.Length)];
                list.Add(new Document
                {
                    Id = i,
                    Title = $"Document #{i}",
                    Content = $"{baseText} Additional context: {extra} (ID: {i})"
                });
            }
            return list;
        }
    }
}
