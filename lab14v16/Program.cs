using System;
using System.Collections.Generic;
using System.Text;

namespace lab14v16
{
    public class ValidatedItem
    {
        public int Id { get; set; }
        public string Data { get; set; }

        public ValidatedItem(int id, string data)
        {
            Id = id;
            Data = data;
        }

        public override string ToString() => $"[Item #{Id}] Дані: '{Data}'";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №14: HashSet, Queue, Stack (Варіант 16 - Система валідації) ===\n");

   
            Console.WriteLine("--- 1. HashSet<T>: Унікальні правила валідації ---");
            var validationRules = new HashSet<string> { "NotNull", "MinLength:5", "IsEmail", "UniqueEmail" };
            
         
            bool added = validationRules.Add("NotNull");
            Console.WriteLine($"Додано правило 'NotNull' повторно? {added}");
            validationRules.Add("AlphaNumeric");

            Console.WriteLine("Активні правила валідації в системі:");
            foreach (var rule in validationRules)
            {
                Console.WriteLine($" - {rule}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Queue<T>: Черга об’єктів на валідацію (FIFO) ---");
            var validationQueue = new Queue<ValidatedItem>();
            validationQueue.Enqueue(new ValidatedItem(1, "test@mail.com"));
            validationQueue.Enqueue(new ValidatedItem(2, "abc"));
            validationQueue.Enqueue(new ValidatedItem(3, "user@domain.net"));

            Console.WriteLine($"Кількість елементів у черзі на перевірку: {validationQueue.Count}");
            
    
            Console.WriteLine($"Наступний на черзі (Peek): {validationQueue.Peek()}");

     
            while (validationQueue.Count > 0)
            {
                var item = validationQueue.Dequeue();
                Console.WriteLine($"Обробляється в черзі: {item}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 3. Stack<T>: Історія помилок валідації (LIFO) ---");
            var errorHistory = new Stack<string>();
            errorHistory.Push("Помилка: Поле 'Data' занадто коротке для Item #2");
            errorHistory.Push("Помилка: Невірний формат email для Item #5");
            errorHistory.Push("Помилка: Відсутнє обов'язкове правило 'NotNull' для Item #7");

            Console.WriteLine($"Всього зафіксовано помилок у стеку: {errorHistory.Count}");
            
  
            Console.WriteLine($"Остання зареєстрована помилка (Peek): {errorHistory.Peek()}");


            while (errorHistory.Count > 0)
            {
                var resolvedError = errorHistory.Pop();
                Console.WriteLine($"Виправлено/опрацьовано: {resolvedError}");
            }

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}

