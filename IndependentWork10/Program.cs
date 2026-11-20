using System;
using System.Collections.Generic;
using System.Text;

namespace IndependentWork10
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №10: Делегати й лямбда-вирази ===\n");

            // Колекція 1: Продукти (клас)
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 25000 },
                new Product { Name = "Mouse", Price = 500 },
                new Product { Name = "Keyboard", Price = 1500 },
                new Product { Name = "Monitor", Price = 9000 }
            };

            // Колекція 2: Числа (int)
            var numbers = new List<int> { 5, 12, 3, 8, 20, 15 };

            // Колекція 3: Рядки (string)
            var words = new List<string> { "C#", "Delegates", "Lambda", "LINQ", "Collections" };

            // 1. Тест Map
            Console.WriteLine("--- 1. Тест Map ---");
            var names = CollectionUtils.Map(products, p => p.Name);
            Console.WriteLine("Імена продуктів: " + string.Join(", ", names));

            var squares = CollectionUtils.Map(numbers, n => n * n);
            Console.WriteLine("Квадрати чисел: " + string.Join(", ", squares));

            // 2. Тест MaxBy
            Console.WriteLine("\n--- 2. Тест MaxBy ---");
            var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
            Console.WriteLine($"Найдорожчий продукт: {mostExpensive.Name} - {mostExpensive.Price} грн");

            var maxNumber = CollectionUtils.MaxBy(numbers, n => n);
            Console.WriteLine($"Максимальне число: {maxNumber}");

            // 3. Тест Filter
            Console.WriteLine("\n--- 3. Тест Filter ---");
            var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
            Console.WriteLine("Продукти дорожчі за 1000 грн:");
            CollectionUtils.ForEach(expensive, p => Console.WriteLine($" - {p.Name}: {p.Price} грн"));

            var evenNumbers = CollectionUtils.Filter(numbers, n => n % 2 == 0);
            Console.WriteLine("Парні числа: " + string.Join(", ", evenNumbers));

            // 4. Тест ForEach
            Console.WriteLine("\n--- 4. Тест ForEach ---");
            Console.WriteLine("Виведення всіх слів довжиною > 4:");
            var longWords = CollectionUtils.Filter(words, w => w.Length > 4);
            CollectionUtils.ForEach(longWords, w => Console.WriteLine($" * {w}"));
        }
    }
}
