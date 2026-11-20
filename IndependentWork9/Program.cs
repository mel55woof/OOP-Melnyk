using System;
using System.Text;

namespace IndependentWork9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №9: Dictionary<TKey, TValue> та HashSet<T> ===\n");

            // Сценарій 1: FileMetadataCache
            Console.WriteLine("--- Сценарій 1: Кеш метаданих файлів ---");
            var cache = new FileMetadataCache();
            cache.Add("C:/docs/report.pdf", new FileMetadata
            {
                FileName = "report.pdf",
                Size = 204800,
                LastModified = DateTime.Now,
                Hash = "A1B2C3D4"
            });
            cache.Add("C:/code/Program.cs", new FileMetadata
            {
                FileName = "Program.cs",
                Size = 4096,
                LastModified = DateTime.Now,
                Hash = "E5F6G7H8"
            });

            Console.WriteLine($"Contains 'c:/docs/report.pdf' (case-insensitive): {cache.Contains("c:/docs/report.pdf")}");
            var meta = cache.Get("C:/docs/report.pdf");
            Console.WriteLine($"Отримано: {meta?.FileName}, Розмір: {meta?.Size} байт, Хеш: {meta?.Hash}");
            cache.Remove("C:/code/Program.cs");
            Console.WriteLine($"Елементів у кеші після видалення: {cache.Count}\n");

            // Сценарій 2: TagManager
            Console.WriteLine("--- Сценарій 2: Менеджер унікальних тегів ---");
            var tagManager = new TagManager();
            tagManager.AddTag("art-1", "C#");
            tagManager.AddTag("art-1", "OOP");
            tagManager.AddTag("art-1", "Collections");
            tagManager.AddTag("art-1", "c#"); // дублікат ігнорується

            tagManager.AddTag("art-2", "C#");
            tagManager.AddTag("art-2", "Performance");
            tagManager.AddTag("art-2", "OOP");

            Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tagManager.GetAllUniqueTags())}");
            Console.WriteLine($"Спільні теги art-1 та art-2 (IntersectWith): {string.Join(", ", tagManager.FindCommonTags("art-1", "art-2"))}");
            Console.WriteLine($"Унікальні лише для art-1 (ExceptWith): {string.Join(", ", tagManager.FindUniqueToFirstArticle("art-1", "art-2"))}");
            tagManager.RemoveTag("art-1", "OOP");
            Console.WriteLine($"Спільні після видалення OOP з art-1: {string.Join(", ", tagManager.FindCommonTags("art-1", "art-2"))}\n");

            // Сценарій 3: SearchIndex
            Console.WriteLine("--- Сценарій 3: Інвертований пошуковий індекс ---");
            var index = new SearchIndex();
            index.AddDocument(1, "C# Dictionary provides fast O(1) lookup by key.");
            index.AddDocument(2, "HashSet stores unique elements and supports set operations.");
            index.AddDocument(3, "Combining C# Dictionary and HashSet builds a fast search index.");

            Console.WriteLine($"Документи зі словом 'Dictionary': [{string.Join(", ", index.Search("Dictionary"))}]");
            Console.WriteLine($"Документи зі словами 'C#' ТА 'Dictionary' ТА 'fast': [{string.Join(", ", index.SearchMultiple("C#", "Dictionary", "fast"))}]");
            Console.WriteLine($"Документи зі словами 'HashSet' ТА 'fast': [{string.Join(", ", index.SearchMultiple("HashSet", "fast"))}]");
        }
    }
}
