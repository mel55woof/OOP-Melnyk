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
                new Airport { Id = 4, Name = "Heathrow", City = "London" },
                new Airport { Id = 5, Name = "Charles de Gaulle", City = "Paris" },
                new Airport { Id = 6, Name = "Frankfurt Airport", City = "Frankfurt" },
                new Airport { Id = 7, Name = "Amsterdam Schiphol", City = "Amsterdam" },
                new Airport { Id = 8, Name = "Vienna International", City = "Vienna" },
                new Airport { Id = 9, Name = "Warsaw Chopin", City = "Warsaw" },
                new Airport { Id = 10, Name = "Prague Vaclav Havel", City = "Prague" }
            };

            var flights = new List<Flight>
            {
                new Flight { Id = 1, FlightNumber = "PS-101", DepartureAirportId = 1, ArrivalAirportId = 2, DepartureTime = new TimeSpan(8, 30, 0) },
                new Flight { Id = 2, FlightNumber = "PS-202", DepartureAirportId = 1, ArrivalAirportId = 4, DepartureTime = new TimeSpan(14, 15, 0) },
                new Flight { Id = 3, FlightNumber = "LH-404", DepartureAirportId = 4, ArrivalAirportId = 1, DepartureTime = new TimeSpan(11, 00, 0) },
                new Flight { Id = 4, FlightNumber = "QU-303", DepartureAirportId = 2, ArrivalAirportId = 3, DepartureTime = new TimeSpan(15, 45, 0) },
                new Flight { Id = 5, FlightNumber = "PS-505", DepartureAirportId = 1, ArrivalAirportId = 5, DepartureTime = new TimeSpan(18, 00, 0) },
                new Flight { Id = 6, FlightNumber = "OM-606", DepartureAirportId = 3, ArrivalAirportId = 2, DepartureTime = new TimeSpan(9, 15, 0) },
                new Flight { Id = 7, FlightNumber = "PS-707", DepartureAirportId = 1, ArrivalAirportId = 6, DepartureTime = new TimeSpan(6, 45, 0) },
                new Flight { Id = 8, FlightNumber = "QU-808", DepartureAirportId = 2, ArrivalAirportId = 1, DepartureTime = new TimeSpan(20, 10, 0) },
                new Flight { Id = 9, FlightNumber = "AF-909", DepartureAirportId = 5, ArrivalAirportId = 1, DepartureTime = new TimeSpan(16, 20, 0) },
                new Flight { Id = 10, FlightNumber = "W6-111", DepartureAirportId = 9, ArrivalAirportId = 1, DepartureTime = new TimeSpan(13, 10, 0) },
                new Flight { Id = 11, FlightNumber = "OS-222", DepartureAirportId = 8, ArrivalAirportId = 1, DepartureTime = new TimeSpan(15, 00, 0) },
                new Flight { Id = 12, FlightNumber = "PS-888", DepartureAirportId = 1, ArrivalAirportId = 7, DepartureTime = new TimeSpan(22, 30, 0) }
            };

            Console.WriteLine("--- Запит 1: Фільтрація (Рейси з 'Kyiv Boryspil' після 12:00) ---");
            
            stopwatch.Restart();
            var q1Query = (from f in flights
                           join a in airports on f.DepartureAirportId equals a.Id
                           where a.Name == "Kyiv Boryspil" && f.DepartureTime > new TimeSpan(12, 0, 0)
                           select f).ToList();
            stopwatch.Stop();
            long t1Query = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Query Syntax] Знайдено: {q1Query.Count} рейсів (Час: {t1Query} ticks)");

            stopwatch.Restart();
            var q1Method = flights
                .Join(airports, f => f.DepartureAirportId, a => a.Id, (f, a) => new { f, a })
                .Where(x => x.a.Name == "Kyiv Boryspil" && x.f.DepartureTime > new TimeSpan(12, 0, 0))
                .Select(x => x.f)
                .ToList();
            stopwatch.Stop();
            long t1Method = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Method Syntax] Знайдено: {q1Method.Count} рейсів (Час: {t1Method} ticks)\n");

            Console.WriteLine("--- Запит 2: Об'єднання (Join аеропортів відправлення та прибуття) ---");

            stopwatch.Restart();
            var q2Query = (from f in flights
                           join dep in airports on f.DepartureAirportId equals dep.Id
                           join arr in airports on f.ArrivalAirportId equals arr.Id
                           select new { f.FlightNumber, DepCity = dep.City, ArrCity = arr.City }).ToList();
            stopwatch.Stop();
            long t2Query = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Query Syntax] Оброблено пар: {q2Query.Count} (Час: {t2Query} ticks)");

            stopwatch.Restart();
            var q2Method = flights
                .Join(airports, f => f.DepartureAirportId, dep => dep.Id, (f, dep) => new { f, dep })
                .Join(airports, x => x.f.ArrivalAirportId, arr => arr.Id, (x, arr) => new { x.f.FlightNumber, DepCity = x.dep.City, ArrCity = arr.City })
                .ToList();
            stopwatch.Stop();
            long t2Method = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Method Syntax] Оброблено пар: {q2Method.Count} (Час: {t2Method} ticks)");
            
            foreach (var item in q2Method)
            {
                Console.WriteLine($"   Рейс {item.FlightNumber}: {item.DepCity} -> {item.ArrCity}");
            }
            Console.WriteLine();

            Console.WriteLine("--- Запит 3: Агрегація (Кількість рейсів з кожного аеропорту) ---");

            stopwatch.Restart();
            var q3Query = (from f in flights
                           group f by f.DepartureAirportId into g
                           select new { AirportId = g.Key, Count = g.Count() }).ToList();
            stopwatch.Stop();
            long t3Query = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Query Syntax] Оброблено груп: {q3Query.Count} (Час: {t3Query} ticks)");

            stopwatch.Restart();
            var q3Method = flights
                .GroupBy(f => f.DepartureAirportId)
                .Select(g => new { AirportId = g.Key, Count = g.Count() })
                .ToList();
            stopwatch.Stop();
            long t3Method = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Method Syntax] Оброблено груп: {q3Method.Count} (Час: {t3Method} ticks)");

            foreach (var item in q3Method)
            {
                var airportName = airports.First(a => a.Id == item.AirportId).Name;
                Console.WriteLine($"   Аеропорт '{airportName}': {item.Count} рейсів");
            }
            Console.WriteLine();

            Console.WriteLine("--- Запит 4: Агрегація (Рейс з найранішим часом відправлення) ---");

            stopwatch.Restart();
            var q4Query = (from f in flights
                           orderby f.DepartureTime
                           select f).FirstOrDefault();
            stopwatch.Stop();
            long t4Query = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Query Syntax] Найраніший рейс: {q4Query?.FlightNumber} о {q4Query?.DepartureTime} (Час: {t4Query} ticks)");

            stopwatch.Restart();
            var q4Method = flights.OrderBy(f => f.DepartureTime).FirstOrDefault();
            stopwatch.Stop();
            long t4Method = stopwatch.ElapsedTicks;
            Console.WriteLine($"[Method Syntax] Найраніший рейс: {q4Method?.FlightNumber} о {q4Method?.DepartureTime} (Час: {t4Method} ticks)");

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}
