using System;

namespace OOP_Melnyk
{
    public class SensorReader : IDisposable
    {
        private bool _disposed = false;
        private int _sensorId;
        private bool _isReading;

        public int SensorId => _sensorId;
        public bool IsReading => _isReading;

        public SensorReader(int sensorId)
        {
            _sensorId = sensorId;
            _isReading = true;
            Console.WriteLine($"[Сенсор {_sensorId}] Підключено. Читання даних розпочато.");
        }

        public void ReadValue()
        {
            if (_disposed || !_isReading)
            {
                Console.WriteLine($"[Сенсор {_sensorId}] Помилка: Спроба читання з закритого сенсора!");
                return;
            }

            double simulatedValue = new Random().NextDouble() * 100;
            Console.WriteLine($"[Сенсор {_sensorId}] Показники сенсора: {simulatedValue:F2} °C");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine($"[Сенсор {_sensorId}] Звільнення керованих ресурсів...");
                }

                if (_isReading)
                {
                    Console.WriteLine($"[Сенсор {_sensorId}] Читання зупинено, ресурс сенсора звільнено.");
                    _isReading = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SensorReader()
        {
            Console.WriteLine($"[Деструктор] Сенсор {_sensorId} звільняється через фіналізатор/GC.");
            Dispose(false);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Лабораторна робота №3 | Варіант 16 ===");
            Console.WriteLine("=== Продемонструємо 3 сценарії роботи з IDisposable ===\n");

            Console.WriteLine("--- Сценарій 1: Використання конструкції 'using' ---");
            using (SensorReader sensor1 = new SensorReader(101))
            {
                sensor1.ReadValue();
            }
            Console.WriteLine("Блок using завершено.\n");

            Console.WriteLine("--- Сценарій 2: Явний виклик методу Dispose() ---");
            SensorReader sensor2 = new SensorReader(202);
            sensor2.ReadValue();
            sensor2.Dispose();
            sensor2.ReadValue();
            Console.WriteLine();

            Console.WriteLine("--- Сценарій 3: Без Dispose() (фіналізація через GC) ---");
            CreateAndForgetSensor();

            Console.WriteLine("Викликаємо збирання сміття GC.Collect()...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Збирання сміття завершено.\n");

            Console.WriteLine("=== Виконання програми завершено ===");
        }

        static void CreateAndForgetSensor()
        {
            SensorReader sensor3 = new SensorReader(303);
            sensor3.ReadValue();
        }
    }
}
