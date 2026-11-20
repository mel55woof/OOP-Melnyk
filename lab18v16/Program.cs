using System;

namespace lab18v16
{
    public class ProductAlreadyExistsException : Exception
    {
        public string ProductName { get; }

        public ProductAlreadyExistsException(string productName) 
            : base($"Продукт з ім'ям '{productName}' вже існує в каталозі.")
        {
            ProductName = productName;
        }

        public ProductAlreadyExistsException(string productName, string message) 
            : base(message)
        {
            ProductName = productName;
        }

        public ProductAlreadyExistsException(string productName, string message, Exception innerException) 
            : base(message, innerException)
        {
            ProductName = productName;
        }
    }

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private Result(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(default!, false, error);
        }

        public static Result<T> Failure(Exception ex)
        {
            return new Result<T>(default!, false, ex.Message);
        }
    }

    public class ProductCatalogService
    {
        private string[] _existingProducts = { "Ноутбук", "Мишка", "Клавіатура" };

        public void AddProduct(string productName)
        {
            foreach (var p in _existingProducts)
            {
                if (p.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ProductAlreadyExistsException(productName);
                }
            }

            Console.WriteLine($"[ProductCatalogService] Продукт '{productName}' успішно додано.");
        }

        public Result<bool> UpdateProductPrice(int productId, decimal newPrice)
        {
            if (productId <= 0 || productId > 3)
            {
                return Result<bool>.Failure("Помилка: Продукт з таким ID не знайдено в каталозі.");
            }

            if (newPrice < 0)
            {
                return Result<bool>.Failure("Помилка: Ціна продукту не може бути від'ємною.");
            }

            return Result<bool>.Success(true);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №18: Власні винятки та патерн Result (Варіант 16) ===\n");

            var service = new ProductCatalogService();

            Console.WriteLine("--- Демонстрація 1: Використання власного винятку (try-catch) ---");
            try
            {
                service.AddProduct("Ноутбук"); 
            }
            catch (ProductAlreadyExistsException ex)
            {
                Console.WriteLine($"[Спіймано виняток] {ex.Message} (Продукт: {ex.ProductName})");
            }

            try
            {
                service.AddProduct("Монітор"); 
                Console.WriteLine("[Успіх] Додавання нового продукту завершено.\n");
            }
            catch (ProductAlreadyExistsException ex)
            {
                Console.WriteLine($"[Спіймано виняток] {ex.Message}\n");
            }

            Console.WriteLine("--- Демонстрація 2: Використання патерну Result<T> ---");
            
            var resultValid = service.UpdateProductPrice(1, 15000.00m);
            if (resultValid.IsSuccess)
            {
                Console.WriteLine($"[Result Success] Ціну успішно оновлено! Статус: {resultValid.Value}");
            }
            else
            {
                Console.WriteLine($"[Result Failure] {resultValid.Error}");
            }

            var resultInvalidPrice = service.UpdateProductPrice(2, -500m);
            if (resultInvalidPrice.IsSuccess)
            {
                Console.WriteLine($"[Result Success] Ціну успішно оновлено! Статус: {resultInvalidPrice.Value}");
            }
            else
            {
                Console.WriteLine($"[Result Failure] {resultInvalidPrice.Error}");
            }

            var resultNotFound = service.UpdateProductPrice(99, 1000m);
            if (resultNotFound.IsSuccess)
            {
                Console.WriteLine($"[Result Success] Ціну успішно оновлено! Статус: {resultNotFound.Value}");
            }
            else
            {
                Console.WriteLine($"[Result Failure] {resultNotFound.Error}");
            }

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}

