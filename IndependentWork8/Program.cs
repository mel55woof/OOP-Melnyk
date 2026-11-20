using System;
using System.Text;

namespace IndependentWork8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №8: Узагальнений кеш Cache<TKey, TValue> ===\n");

            var cache = new Cache<int, string>(3);
            cache.Add(1, "Один");
            cache.Add(2, "Два");
            cache.Add(3, "Три");

            Console.WriteLine($"Кількість елементів: {cache.Count}");
            Console.WriteLine($"Отримано ключ 1: {cache.Get(1)}");
            
            Console.WriteLine("\nДодаємо 4-й елемент (має видалитися найстаріший - ключ 1)...");
            cache.Add(4, "Чотири");

            Console.WriteLine($"Чи є ключ 1 у кеші: {cache.ContainsKey(1)}");
            Console.WriteLine($"Кількість елементів після переповнення: {cache.Count}");

            Console.WriteLine("\n=== Завершено ===");
        }
    }
}
