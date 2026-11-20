using System;
using System.Collections.Generic;

namespace IndependentWork12
{
    public static class DataSeeder
    {
        public static (List<Sale> sales, List<Product> products, List<Customer> customers) GenerateData()
        {
            var rand = new Random(42);

            var categories = new[] { "Electronics", "Clothing", "Home & Kitchen", "Books", "Sports" };
            var products = new List<Product>();
            for (int i = 1; i <= 25; i++)
            {
                products.Add(new Product
                {
                    Id = i,
                    Name = $"Product_{i}",
                    Category = categories[rand.Next(categories.Length)],
                    UnitPrice = Math.Round((decimal)(rand.NextDouble() * 500 + 10), 2)
                });
            }

            var cities = new[] { "Rivne", "Kyiv", "Lviv", "Odesa", "Kharkiv", "Dnipro" };
            var customers = new List<Customer>();
            for (int i = 1; i <= 50; i++)
            {
                customers.Add(new Customer
                {
                    Id = i,
                    Name = $"Customer_{i}",
                    City = cities[rand.Next(cities.Length)]
                });
            }

            var sales = new List<Sale>();
            DateTime startDate = new DateTime(2025, 1, 1);
            for (int i = 1; i <= 300; i++)
            {
                sales.Add(new Sale
                {
                    Id = i,
                    ProductId = rand.Next(1, 26),
                    Quantity = rand.Next(1, 10),
                    SaleDate = startDate.AddDays(rand.Next(0, 365)),
                    CustomerId = rand.Next(1, 51)
                });
            }

            return (sales, products, customers);
        }
    }
}
