using System;

namespace IndependentWork15
{
    class Program
    {
        private static int _unstableCallCount = 0;

        public static string UnstableOperation()
        {
            _unstableCallCount++;
            if (_unstableCallCount < 3)
            {
                throw new TimeoutException("Тимчасова недоступність зовнішнього сервісу.");
            }
            return "Успішні дані від сервера!";
        }

        public static string FailingOperation()
        {
            throw new InvalidOperationException("Критична та постійна помилка бази даних.");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №15: Власна реалізація Exponential Backoff та RetryPolicy ===\n");

            var retryPolicy = new RetryPolicy(maxRetries: 3, initialDelay: TimeSpan.FromMilliseconds(200), jitterFactor: 0.2);

            Console.WriteLine("--- Сценарій А: Операція спочатку падає, але згодом успішна ---");
            try
            {
                string result = retryPolicy.Execute(
                    operation: UnstableOperation,
                    shouldRetry: ex => ex is TimeoutException
                );
                Console.WriteLine($"[Результат А]: {result}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка А]: {ex.Message}\n");
            }

            Console.WriteLine("--- Сценарій Б: Операція завжди завершується помилкою (вичерпання спроб) ---");
            try
            {
                retryPolicy.Execute(
                    operation: FailingOperation,
                    shouldRetry: ex => ex is InvalidOperationException
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Результат Б]: Перехоплено остаточний виняток після всіх спроб -> {ex.Message}\n");
            }

            Console.WriteLine("Дослідження власної політики повторів успішно завершено!");
        }
    }
}
