using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace IndependentWork13
{
    class Program
    {
        // Інтенсивна обчислювальна операція для кожного елемента
        private static double HeavyComputation(int number)
        2026-11-20
        {
            double result = number;
            for (int i = 0; i < 50; i++)
            {
                result = Math.Sqrt(Math.Abs(result) * 3.14159) + Math.Sin(result);
            }
            return result;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №13: PLINQ дослідження продуктивності та безпеки ===\n");

            // Частина 2 та 4: Експерименти з різними розмірами колекцій (1M, 5M, 10M)
            int[] sizes = { 1_000_000, 5_000_000, 10_000_000 };

            foreach (var size in sizes)
            {
                Console.WriteLine($"--- Тестування на колекції з {size:N0} елементів ---");
                var rand = new Random(42);
                var data = Enumerable.Range(1, size).Select(_ => rand.Next(-100000, 100000)).ToList();

                // Звичайний LINQ
                var sw = Stopwatch.StartNew();
                var linqResult = data
                    .Where(x => x > 0)
                    .Select(x => HeavyComputation(x))
                    .Where(res => res > 10.0)
                    .Count();
                sw.Stop();
                long linqTime = sw.ElapsedMilliseconds;
                Console.WriteLine($" LINQ:      {linqTime,5} ms (Знайдено: {linqResult})");

                // PLINQ
                sw.Restart();
                var plinqResult = data
                    .AsParallel()
                    .Where(x => x > 0)
                    .Select(x => HeavyComputation(x))
                    .Where(res => res > 10.0)
                    .Count();
                sw.Stop();
                long plinqTime = sw.ElapsedMilliseconds;
                Console.WriteLine($" PLINQ:     {plinqTime,5} ms (Знайдено: {plinqResult})");

                double speedup = (double)linqTime / (plinqTime == 0 ? 1 : plinqTime);
                Console.WriteLine($" Прискорення: {speedup:F2}x\n");
            }

            // Частина 5: Дослідження побічних ефектів (Race Condition) та їх усунення
            Console.WriteLine("--- Дослідження побічних ефектів та потокобезпеки в PLINQ ---");
            var sampleData = Enumerable.Range(1, 100_000).ToList();

            // 1. Небезпечний підхід (без синхронізації)
            int unsafeSum = 0;
            sampleData.AsParallel().ForAll(x =>
            {
                // Порушення потокобезпеки: кілька потоків одночасно змінюють спільну змінну
                unsafeSum += x; 
            });
            Console.WriteLine($" Некоректна сума (з побічним ефектом): {unsafeSum} (очікувалося: 5000050000)");

            // 2. Безпечний підхід (використання lock)
            int safeSum = 0;
            object lockObj = new object();
            sampleData.AsParallel().ForAll(x =>
            {
                lock (lockObj)
                {
                    safeSum += x;
                }
            });
            Console.WriteLine($" Коректна сума (з блокуванням lock):    {safeSum} (очікувалося: 5000050000)");

            // Альтернативний безпечний підхід (Interlocked або LINQ .Sum())
            long threadSafeLinqSum = sampleData.AsParallel().Sum(x => (long)x);
            Console.WriteLine($" Коректна сума через PLINQ .Sum():      {threadSafeLinqSum}\n");

            Console.WriteLine("Дослідження успішно завершено!");
        }
    }
}
