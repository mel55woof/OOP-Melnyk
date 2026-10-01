using System;

namespace SportApp
{
    public class SportTeam
    {
        
        private string _name;
        private string _coach;
        private int _playersCount;

        
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Coach
        {
            get => _coach;
            set => _coach = value;
        }

        public int PlayersCount
        {
            get => _playersCount;
            set
            {
               
                if (value > 0)
                {
                    _playersCount = value;
                }
                else
                {
                    Console.WriteLine("Помилка: кількість гравців повинна бути більшою за 0!");
                }
            }
        }

        
        public SportTeam(string name, string coach, int playersCount)
        {
            Name = name;
            Coach = coach;
            PlayersCount = playersCount;
            Console.WriteLine($"[Конструктор] Створено команду: {Name}");
        }

       
        public SportTeam() : this("Team A", "Unknown", 11)
        {
            Console.WriteLine("[Конструктор за замовчуванням] Ініціалізація базовими значеннями.");
        }

       
        public void PlayMatch(string opponent)
        {
            Console.WriteLine($"Команда \"{Name}\" під керівництвом тренера {Coach} грає матч проти \"{opponent}\" у складі {PlayersCount} гравців!");
        }

        
        ~SportTeam()
        {
            Console.WriteLine($"[Деструктор] Об'єкт команди \"{_name}\" знищено з пам'яті.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Створення об'єктів ===");

            
            SportTeam team1 = new SportTeam();
            team1.PlayMatch("Реал Мадрид");

            Console.WriteLine();

            
            SportTeam team2 = new SportTeam("Динамо", "Олександр Шовковський", 11);
            team2.PlayMatch("Шахтар");

            Console.WriteLine();
            
            SportTeam team3 = new SportTeam("Челсі", "Енцо Мареска", 22);
            team3.PlayersCount = -5;
            team3.PlayMatch("Арсенал");

            Console.WriteLine("\n=== Завершення роботи Main, підготовка до GC ===");
            
            team1 = null;
            team2 = null;
            team3 = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Робота програми завершена.");
        }
    }
}
