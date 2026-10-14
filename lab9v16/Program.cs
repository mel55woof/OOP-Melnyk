using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lab9v16
{
    public abstract class BackupService
    {
        public string ServiceName { get; protected set; }

        protected BackupService(string serviceName)
        {
            ServiceName = serviceName;
        }

        public abstract void Backup(string source);

        protected void ValidateSource(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException($"[{ServiceName}] Помилка: Шлях до джерела не може бути порожнім.");
            }

            if (source.Contains("invalid") || source.Contains("nonexistent"))
            {
                throw new DirectoryNotFoundException($"[{ServiceName}] Помилка: Джерело '{source}' недоступне або не існує.");
            }
        }
    }

    public class FullBackup : BackupService
    {
        public bool CompressOutput { get; set; }

        public FullBackup(bool compressOutput = true) : base("Повне копіювання (Full Backup)")
        {
            CompressOutput = compressOutput;
        }

        public override void Backup(string source)
        {
            ValidateSource(source);
            Console.WriteLine($"[FullBackup] Успішно створено повний архів джерела '{source}'. Стрічка стиснення: {CompressOutput}.");
        }
    }

    public class IncrementalBackup : BackupService
    {
        public DateTime LastBackupTime { get; set; }

        public IncrementalBackup(DateTime lastBackupTime) : base("Інкрементальне копіювання (Incremental Backup)")
        {
            LastBackupTime = lastBackupTime;
        }

        public override void Backup(string source)
        {
            ValidateSource(source);

            if (LastBackupTime > DateTime.Now)
            {
                throw new InvalidOperationException($"[{ServiceName}] Помилка: Дата останнього бэкапу не може бути у майбутньому.");
            }

            Console.WriteLine($"[IncrementalBackup] Успішно скопійовано зміни у '{source}', що виникли після {LastBackupTime:dd.MM.yyyy HH:mm}.");
        }
    }

    public class DifferentialBackup : BackupService
    {
        public string BaseFullBackupPath { get; set; }

        public DifferentialBackup(string baseFullBackupPath) : base("Диференційне копіювання (Differential Backup)")
        {
            BaseFullBackupPath = baseFullBackupPath;
        }

        public override void Backup(string source)
        {
            ValidateSource(source);

            if (string.IsNullOrWhiteSpace(BaseFullBackupPath))
            {
                throw new InvalidOperationException($"[{ServiceName}] Помилка: Вказано невалідний базовий повний архів.");
            }

            Console.WriteLine($"[DifferentialBackup] Збережено відмінності джерела '{source}' відносно базового архіву '{BaseFullBackupPath}'.");
        }
    }

    public class BackupManager
    {
        public void ExecuteAllBackups(List<BackupService> backupServices, string sourcePath)
        {
            Console.WriteLine($"=== Запуск пакетного резервного копіювання для джерела: '{sourcePath}' ===\n");

            int successCount = 0;
            int errorCount = 0;

            foreach (var service in backupServices)
            {
                try
                {
                    service.Backup(sourcePath);
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ПОРУШЕННЯ СЕРВІСУ] Помилка під час виконання {service.ServiceName}: {ex.Message}");
                    Console.ResetColor();
                    errorCount++;
                }
            }

            Console.WriteLine($"\n--- Підсумок виконання: Успішно: {successCount}, З помилками: {errorCount} ---");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 9 (Варіант 16: BackupService) ===\n");

            List<BackupService> services = new List<BackupService>
            {
                new FullBackup(compressOutput: true),
                new IncrementalBackup(DateTime.Now.AddDays(-1)),
                new DifferentialBackup("C:\\Backups\\Full_2026_10_01.zip")
            };

            BackupManager manager = new BackupManager();

            Console.WriteLine("--- 1. Демонстрація коректної роботи всіх сервісів ---");
            manager.ExecuteAllBackups(services, "C:\\Data\\Projects\\InkPaw");
            Console.WriteLine();

            Console.WriteLine("--- 2. Демонстрація обробки помилок при виклику невалідного джерела ---");
            manager.ExecuteAllBackups(services, "D:\\invalid_folder_path");

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}

