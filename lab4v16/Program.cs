using System;

namespace OOP_Melnyk
{
    public class Angle
    {
        private double _degrees;

      
        public double Degrees
        {
            get => _degrees;
            set
            {
                if (value < 0 || value > 360)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Значення кута має бути в діапазоні від 0 до 360 градусів.");
                }
                _degrees = value;
            }
        }

        // Конструктор
        public Angle(double degrees)
        {
            Degrees = degrees;
        }

       
        public static Angle RightAngle => new Angle(90);

       
        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: 
                        return Math.Floor(_degrees);
                    case 1: 
                        return Math.Floor((_degrees - Math.Floor(_degrees)) * 60);
                    case 2: 
                        double fractionalMinutes = (_degrees - Math.Floor(_degrees)) * 60;
                        return Math.Round((fractionalMinutes - Math.Floor(fractionalMinutes)) * 60, 2);
                    default: 
                        throw new IndexOutOfRangeException("Індекс може бути 0 (градуси), 1 (мінути) або 2 (секунди).");
                }
            }
        }

    
        public static Angle operator +(Angle a1, Angle a2)
        {
            double result = (a1.Degrees + a2.Degrees) % 360;
            return new Angle(result);
        }

   
        public static Angle operator -(Angle a1, Angle a2)
        {
            double result = (a1.Degrees - a2.Degrees) % 360;
            if (result < 0) result += 360;
            return new Angle(result);
        }

      
        public static bool operator ==(Angle a1, Angle a2)
        {
            if (ReferenceEquals(a1, a2)) return true;
            if (a1 is null || a2 is null) return false;
            return Math.Abs(a1.Degrees - a2.Degrees) < 0.0001;
        }

        public static bool operator !=(Angle a1, Angle a2)
        {
            return !(a1 == a2);
        }

    
        public override bool Equals(object obj)
        {
            if (obj is Angle other)
            {
                return this == other;
            }
            return false;
        }

       
        public override int GetHashCode()
        {
            return _degrees.GetHashCode();
        }

    
        public override string ToString()
        {
            return $"{_degrees}°";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №4 (Варіант 16: Клас Angle) ===\n");

           
            Angle a1 = new Angle(45.5);
            Angle a2 = new Angle(60.0);
            Angle right = Angle.RightAngle; 

            Console.WriteLine($"Кут 1 (a1): {a1}");
            Console.WriteLine($"Кут 2 (a2): {a2}");
            Console.WriteLine($"Прямий кут (статичний RightAngle): {right}\n");

           
            Console.WriteLine($"--- Робота з індексатором для кута a1 ({a1}) ---");
            Console.WriteLine($"[0] Градуси: {a1[0]}°");
            Console.WriteLine($"[1] Мінути:  {a1[1]}'");
            Console.WriteLine($"[2] Секунди: {a1[2]}\"\n");

        
            Angle sum = a1 + a2;
            Angle diff = a2 - a1;
            Console.WriteLine($"Додавання (a1 + a2): {a1} + {a2} = {sum}");
            Console.WriteLine($"Віднімання (a2 - a1): {a2} - {a1} = {diff}\n");

            
            Angle a3 = new Angle(45.5);
            Console.WriteLine($"a1 == a3 (45.5° == 45.5°): {a1 == a3}");
            Console.WriteLine($"a1 == a2 (45.5° == 60°):   {a1 == a2}");
            Console.WriteLine($"a1.Equals(a3):              {a1.Equals(a3)}\n");

            
            Console.WriteLine("--- Перевірка валідації (спроба встановити недопустимий кут 400°) ---");
            try
            {
                Angle invalidAngle = new Angle(400);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Помилка спіймана: {ex.Message}");
            }

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}

