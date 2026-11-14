using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace lab15v16
{
    public class Airport
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string City { get; set; }
    }

    public class Flight
    {
        public int Id { get; set; }
        public required string FlightNumber { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public TimeSpan DepartureTime { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №15: LINQ (Варіант 16 - Управління авіарейсами) ===\n");

            var stopwatch = new Stopwatch();

            var airports = new List<Airport>
            {
                new Airport { Id = 1, Name = "Kyiv Boryspil", City = "Kyiv" },
                new Airport { Id = 2, Name = "Lviv Danylo Halytskyi", City = "Lviv" },
                new Airport { Id = 3, Name = "Odesa International", City = "Odesa" },
                new Airport { Id = 4, Name = "Heathrow", City = "London" }
            };

            var flights = new List<Flight>
            {
                new Flight { Id = 1, FlightNumber = "PS-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new TimeSpan(8, 30, 0) },
                new Flight { Id = 2, FlightNumber = "PS-202", DepartureAirportId = 1, ArrivalAirportId = 4, DepartureTime = new TimeSpan(14, 15, 0) },
                new Flight { Id = 3, FlightNumber = "LH-404", DepartureAirportId = 4, ArrivalAirportId = 1, DepartureTime = new TimeSpan(11, 00, 0) },
                new Flight { Id = 4, FlightNumber = "QU-303", DepartureAirportId = 2, ArrivalAirportId = 3, DepartureTime = new TimeSpan(15, 45, 0) },
                new Flight { Id = 5, FlightNumber = "PS-505", DepartureAirportId = 1, ArrivalAirportId = 3, DepartureTime = new TimeSpan(18, 00, 0) },
                new Flight { Id = 6, FlightNumber = "OM-606", DepartureAirportId = 3, ArrivalAirportId = 2, DepartureTime = new TimeSpan(9, 15, 0) },
                new Flight { Id = 7, FlightNumber = "PS-707", DepartureAirportId = 1, ArrivalAirportId = 4, DepartureTime = new TimeSpan(6, 45, 0) },
                new Flight { Id = 8, FlightNumber = "QU-808", DepartureAirportId = 2, ArrivalAirportId = 1, DepartureTime = new TimeSpan(20, 10, 0) }
            };

            Console.WriteLine("--- Запит 1: Фільтрація (Рейси з 'Kyiv Boryspil' після 12:00) ---");
            stopwatch.Restart();
            var q1Query = (from f in flights
                           join a in airports on f.DepartureAirportId equals a.Id
                           where a.Name == "Kyiv Boryspil" && f.DepartureTime > new TimeSpan(12, 0, 0)
                           select f).ToList();
            stopwatch.Stop();
            Console.WriteLine($"[Query Syntax] Знайдено рейсів: {q1Query.Count} (Час: {stopwatch.ElapsedTicks} ticks)");

            stopwatch.Restart();
            var q1Method = flights
                .Join(airports, f => f.DepartureAirportId, a => a.Id, (f, a) => new { f, a })
                .Where(x => x.a.Name == "Kyiv Boryspil" && x.f.DepartureTime > new TimeSpan(12, 0, 0))
                .Select(x => x.f)
                .ToList();
            stopwatch.Stop();
            Console.WriteLine($"[Method Syntax] Знайдено рейсів: {q1Method.Count} (Час: {stopwatch.ElapsedTicks} ticks)\n");

            Console.WriteLine("--- Запит 2: Об'єднання (Join аеропортів відправлення та прибуття) ---");
            stopwatch.Restart();
            var q2Query = (from f in flights
                           join dep in airports on f.DepartureAirportId equals dep.Id
                           join arr in airports on f.ArrivalAirportId equals arr.Id
                           select new { f.FlightNumber, DepCity = dep.City, ArrCity = arr.City }).ToList();
            stopwatch.Stop();
            Console.WriteLine($"[Query Syntax] Оброблено пар: {q2Query.Count} (Час: {stopwatch.ElapsedTicks} ticks)");
            foreach (var item in q2Query.Take(3))
            {
                Console.WriteLine($"   Рейс {item.FlightNumber}: {item.DepCity} -> {item.ArrCity}");
            }
            Console.WriteLine();

            Console.WriteLine("--- Запит 3: Агрегація (Кількість рейсів з кожного аеропорту) ---");
            stopwatch.Restart();
            var q3Method = flights
                .GroupBy(f => f.DepartureAirportId)
                .Select(g => new { AirportId = g.Key, Count = g.Count() })
                .ToList();
            stopwatch.Stop();
            Console.WriteLine($"[Method Syntax] Оброблено груп: {q3Method.Count} (Час: {stopwatch.ElapsedTicks} ticks)");
            foreach (var item in q3Method)
            {
                var airportName = airports.First(a => a.Id == item.AirportId).Name;
                Console.WriteLine($"   Аеропорт '{airportName}': {item.Count} рейсів");
            }
            Console.WriteLine();

            Console.WriteLine("--- Запит 4: Агрегація (Рейс з найранішим часом відправлення) ---");
            stopwatch.Restart();
            var earliestFlight = flights.OrderBy(f => f.DepartureTime).FirstOrDefault();
            stopwatch.Stop();
            Console.WriteLine($"[Method Syntax] Найраніший рейс: {earliestFlight?.FlightNumber} о {earliestFlight?.DepartureTime} (Час: {stopwatch.ElapsedTicks} ticks)");

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}
