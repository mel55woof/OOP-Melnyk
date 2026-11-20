using System;
using System.Text;

namespace IndependentWork12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Самостійна робота №12: LINQ pipelines: ETL та аналітика ===\n");

            var (sales, products, customers) = DataSeeder.GenerateData();
            Console.WriteLine($"[ETL Init] Згенеровано: {sales.Count} продажу, {products.Count} продуктів, {customers.Count} клієнтів.\n");

            var pipelines = new AnalyticsPipelines(sales, products, customers);

            pipelines.RunPipeline1_TopProductsByCategory();
            pipelines.RunPipeline2_RevenueByCity();
            pipelines.RunPipeline3_SalesByYearAndMonth();
            pipelines.RunPipeline4_TopCustomersBySpending();

            Console.WriteLine("Успішно виконано всі аналітичні LINQ-пайплайни!");
        }
    }
}
