using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IndependentWork12
{
    public class AnalyticsPipelines
    {
        private readonly List<Sale> _sales;
        private readonly List<Product> _products;
        private readonly List<Customer> _customers;

        public AnalyticsPipelines(List<Sale> sales, List<Product> products, List<Customer> customers)
        {
            _sales = sales;
            _products = products;
            _customers = customers;
        }

        // Пайплайн 1: Найпопулярніші продукти за категоріями (ТОП-3)
        public void RunPipeline1_TopProductsByCategory()
        {
            var sw = Stopwatch.StartNew();

            var query = _sales
                .GroupBy(s => s.ProductId)
                .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(s => s.Quantity) })
                .Join(_products,
                    s => s.ProductId,
                    p => p.Id,
                    (s, p) => new { p.Name, p.Category, s.TotalSold })
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TopProducts = g.OrderByDescending(p => p.TotalSold).Take(3).ToList()
                });

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 1: Найпопулярніші продукти за категоріями [Час: {sw.ElapsedTicks} ticks] ===");
            foreach (var cat in query)
            {
                Console.WriteLine($" Категорія: {cat.Category}");
                foreach (var p in cat.TopProducts)
                {
                    Console.WriteLine($"    - {p.Name} (продано: {p.TotalSold} од.)");
                }
            }
            Console.WriteLine();
        }

        // Пайплайн 2: Загальний дохід за містами клієнтів
        public void RunPipeline2_RevenueByCity()
        {
            var sw = Stopwatch.StartNew();

            var query = _sales
                .Join(_products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
                .Join(_customers, sp => sp.CustomerId, c => c.Id, (sp, c) => new { c.City, sp.Revenue })
                .GroupBy(x => x.City)
                .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
                .OrderByDescending(x => x.TotalRevenue);

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 2: Загальний дохід за містами [Час: {sw.ElapsedTicks} ticks] ===");
            foreach (var item in query)
            {
                Console.WriteLine($" Місто: {item.City,-10} | Дохід: {item.TotalRevenue,10:C2}");
            }
            Console.WriteLine();
        }

        // Пайплайн 3: Продажі за місяцями та роками
        public void RunPipeline3_SalesByYearAndMonth()
        {
            var sw = Stopwatch.StartNew();

            var query = _sales
                .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    TotalUnits = g.Sum(s => s.Quantity),
                    TotalRevenue = g.Sum(s => s.Quantity * _products.First(p => p.Id == s.ProductId).UnitPrice)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month);

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 3: Продажі за роками та місяцями [Час: {sw.ElapsedTicks} ticks] ===");
            foreach (var item in query)
            {
                Console.WriteLine($" {item.Year}.{item.Month:D2} | Одиниць: {item.TotalUnits,4} | Дохід: {item.TotalRevenue,10:C2}");
            }
            Console.WriteLine();
        }

        // Пайплайн 4 (Власний): ТОП-5 найактивніших клієнтів за сумою покупок
        public void RunPipeline4_TopCustomersBySpending()
        {
            var sw = Stopwatch.StartNew();

            var query = _sales
                .Join(_products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, Spent = s.Quantity * p.UnitPrice })
                .GroupBy(x => x.CustomerId)
                .Select(g => new { CustomerId = g.Key, TotalSpent = g.Sum(x => x.Spent), PurchaseCount = g.Count() })
                .Join(_customers, x => x.CustomerId, c => c.Id, (x, c) => new { c.Name, c.City, x.TotalSpent, x.PurchaseCount })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5);

            sw.Stop();

            Console.WriteLine($"=== Пайплайн 4: ТОП-5 клієнтів за витратами [Час: {sw.ElapsedTicks} ticks] ===");
            foreach (var c in query)
            {
                Console.WriteLine($" Клієнт: {c.Name,-12} ({c.City}) | Витрачено: {c.TotalSpent,9:C2} | Замовлень: {c.PurchaseCount}");
            }
            Console.WriteLine();
        }
    }
}
