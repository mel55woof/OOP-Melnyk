using System;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace lab17v16
{
    public class FileProcessor
    {
        private int _attemptCount = 0;

        public string GetSmsTemplate(string path)
        {
            _attemptCount++;
            Console.WriteLine($"[FileProcessor] Спроба читання файлу #{_attemptCount}...");
            
            if (_attemptCount <= 3)
            {
                throw new FileNotFoundException($"Файл шаблону за шляхом '{path}' не знайдено.");
            }

            return "Шаблон SMS: Шановний клієнте, ваше замовлення успішно відправлено!";
        }
    }

    public class NetworkClient
    {
        private int _attemptCount = 0;

        public void SendSms(string phoneNumber, string message)
        {
            _attemptCount++;
            Console.WriteLine($"[NetworkClient] Спроба відправки SMS #{_attemptCount} на номер {phoneNumber}...");

            if (_attemptCount <= 2)
            {
                throw new HttpRequestException("Мережева помилка (503 Service Unavailable).");
            }

            Console.WriteLine("[NetworkClient] SMS успішно надіслано!");
        }
    }

    public static class RetryHelper
    {
        public static T ExecuteWithRetry<T>(
            Func<T> operation, 
            int retryCount = 3, 
            TimeSpan initialDelay = default, 
            Func<Exception, bool> shouldRetry = null)
        {
            if (initialDelay == default)
            {
                initialDelay = TimeSpan.FromMilliseconds(300);
            }

            int attempt = 0;

            while (true)
            {
                try
                {
                    attempt++;
                    return operation();
                }
                catch (Exception ex)
                {
                    if (attempt > retryCount || (shouldRetry != null && !shouldRetry(ex)))
                    {
                        throw;
                    }

                    TimeSpan currentDelay = TimeSpan.FromTicks(initialDelay.Ticks * (long)Math.Pow(2, attempt - 1));
                    Console.WriteLine($"[RetryHelper] Помилка: {ex.Message}. Спроба {attempt} не вдалася. Очікування {currentDelay.TotalMilliseconds} мс...\n");
                    
                    Thread.Sleep(currentDelay);
                }
            }
        }

        public static void ExecuteWithRetry(
            Action operation, 
            int retryCount = 3, 
            TimeSpan initialDelay = default, 
            Func<Exception, bool> shouldRetry = null)
        {
            ExecuteWithRetry<object>(() =>
            {
                operation();
                return null!;
            }, retryCount, initialDelay, shouldRetry);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №17 (Варіант 16) ===\n");

            var fileProcessor = new FileProcessor();
            var networkClient = new NetworkClient();

            Func<Exception, bool> retryCondition = ex => 
                ex is FileNotFoundException || ex is HttpRequestException;

            try
            {
                string template = RetryHelper.ExecuteWithRetry(
                    () => fileProcessor.GetSmsTemplate("templates/sms.txt"),
                    retryCount: 4,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryCondition
                );
                Console.WriteLine($"[Результат] {template}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка] {ex.Message}\n");
            }

            try
            {
                RetryHelper.ExecuteWithRetry(
                    () => networkClient.SendSms("+380501234567", "Тест"),
                    retryCount: 3,
                    initialDelay: TimeSpan.FromMilliseconds(200),
                    shouldRetry: retryCondition
                );
                Console.WriteLine("[Результат] Мережеву операцію завершено успішно!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Помилка] {ex.Message}\n");
            }

            Console.WriteLine("=== Завершено ===");
        }
    }
}

