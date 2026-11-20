using System;
using System.Net.Http;
using System.Threading;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace IndependentWork14
{
    public class Program
    {
        private static int _dbAttempts = 0;
        private static int _queueAttempts = 0;

        public static string SimulateDatabaseConnection(string connectionString)
        {
            _dbAttempts++;
            Console.WriteLine($"[DB] Спроба підключення #{_dbAttempts} до: {connectionString}");
            
            if (_dbAttempts < 3)
            {
                throw new TimeoutException("Тимчасовий збій мережі при підключенні до бази даних.");
            }

            Console.WriteLine("[DB] Успішне підключення та виконання транзакції!");
            return "Data from DB";
        }

        public static void SimulateQueuePublish(string message)
        {
            _queueAttempts++;
            Console.WriteLine($"[Queue] Спроба відправки повідомлення #{_queueAttempts}...");
            
            Thread.Sleep(2500); 
            
            Console.WriteLine("[Queue] Повідомлення успішно віправлено в чергу!");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №14: Кейси Polly та відмовостійкість ===\n");

            Console.WriteLine("--- Сценарій 1: Доступ до бази даних із політикою Retry ---");
            var dbPipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<TimeoutException>(),
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(500),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    OnRetry = args =>
                    {
                        Console.WriteLine($"[Polly Retry Log] Повторна спроба #{args.AttemptNumber} через {args.RetryDelay.TotalMilliseconds}мс через помилку: {args.Outcome.Exception?.Message}");
                        return default;
                    }
                })
                .Build();

            try
            {
                string dbResult = dbPipeline.Execute(token => SimulateDatabaseConnection("Server=sql.internal;Database=Prod;"), CancellationToken.None);
                Console.WriteLine($"[Результат 1]: {dbResult}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка 1]: Операція не вдалася: {ex.Message}\n");
            }

            Console.WriteLine("--- Сценарій 2: Відправка в чергу з комбінацією Timeout та Retry ---");
            var queuePipeline = new ResiliencePipelineBuilder()
                .AddTimeout(TimeSpan.FromSeconds(1.5)) 
                .AddRetry(new RetryStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<TimeoutRejectedException>(),
                    MaxRetryAttempts = 2,
                    Delay = TimeSpan.FromMilliseconds(300),
                    OnRetry = args =>
                    {
                        Console.WriteLine($"[Polly Timeout/Retry Log] Перевищено ліміт часу! Повторна спроба #{args.AttemptNumber}...");
                        return default;
                    }
                })
                .Build();

            try
            {
                queuePipeline.Execute(token => SimulateQueuePublish("Order_Payload_991"), CancellationToken.None);
                Console.WriteLine("[Результат 2]: Операцію в черзі завершено успішно!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка 2]: Запит скасовано або вичерпано спроби: {ex.GetType().Name} - {ex.Message}\n");
            }

            Console.WriteLine("Дослідження кейсів Polly успішно завершено!");
        }
    }
}
