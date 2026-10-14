using System;
using System.Text;

namespace lab7v16
{
    public class Sensor
    {
        public string Name { get; set; }

        public Sensor(string name)
        {
            Name = name;
        }

        public virtual double ReadValue()
        {
            Console.WriteLine($"[Sensor] Базовий зчитувач даних ({Name})");
            return 0.0;
        }
    }

    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(string name) : base(name) { }

        public override double ReadValue()
        {
            double value = 23.5;
            Console.WriteLine($"[TemperatureSensor (override)] Датчик температури '{Name}': {value} °C");
            return value;
        }
    }

    public class PressureSensor : Sensor
    {
        public PressureSensor(string name) : base(name) { }

        public new double ReadValue()
        {
            double value = 101.3; 
            Console.WriteLine($"[PressureSensor (new)] Датчик тиску '{Name}': {value} кПа");
            return value;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 7 (Варіант 16: Sensor / TemperatureSensor / PressureSensor) ===\n");

            Sensor tempSensorAsBase = new TemperatureSensor("Кімнатна температура");
            Sensor pressureSensorAsBase = new PressureSensor("Атмосферний тиск");

            Console.WriteLine("--- 1. Виклик методів через посилання БАЗОВОГО типу (Sensor) ---");
            Console.Write("TemperatureSensor через Sensor: ");
            tempSensorAsBase.ReadValue(); 

            Console.Write("PressureSensor через Sensor:    ");
            pressureSensorAsBase.ReadValue(); 
            Console.WriteLine();

            Console.WriteLine("--- 2. Виклик методів через посилання ПОХІДНИХ типів ---");
            Console.Write("((TemperatureSensor)tempSensorAsBase): ");
            ((TemperatureSensor)tempSensorAsBase).ReadValue();

            Console.Write("((PressureSensor)pressureSensorAsBase):    ");
            ((PressureSensor)pressureSensorAsBase).ReadValue(); 
            Console.WriteLine();

            Console.WriteLine("--- 3. Висновки щодо різниці override vs new ---");
            Console.WriteLine("1. override: .NET дивиться на РЕАЛЬНИЙ ОБ'ЄКТ у пам'яті (спрацював метод TemperatureSensor).");
            Console.WriteLine("2. new: .NET дивиться на ТИП ПОСИЛАННЯ/ЗМІННОЇ (для Sensor викликався метод базового класу Sensor).");

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}
