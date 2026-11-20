using System;
using System.Threading;

namespace IndependentWork15
{
    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;
        private readonly double _jitterFactor;
        private readonly Random _random = new Random();

        public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
        {
            _maxRetries = maxRetries;
            _initialDelay = initialDelay;
            _jitterFactor = jitterFactor;
        }

        public T Execute<T>(Func<T> operation, Func<Exception, bool> shouldRetry = null)
        {
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
                    bool allowRetry = shouldRetry == null || shouldRetry(ex);

                    if (attempt > _maxRetries || !allowRetry)
                    {
                        Console.WriteLine($"[RetryPolicy] Спроба {attempt}: операція остаточно не вдалася. Виняток: {ex.Message}");
                        throw;
                    }

                    // Формула для експоненційної затримки з jitter
                    double delayMilliseconds = _initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1) 
                        * (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);
                    
                    if (delayMilliseconds < 0) delayMilliseconds = 0;

                    Console.WriteLine($"[RetryPolicy] Спроба {attempt} завершилась помилкою: {ex.Message}. Повторна спроба через {delayMilliseconds:F1} мс...");
                    Thread.Sleep(TimeSpan.FromMilliseconds(delayMilliseconds));
                }
            }
        }
    }
}
