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
            get { return _name; }
            set { _name = value; }
        }

        public string Coach
        {
            get { return _coach; }
            set { _coach = value; }
        }

        public int PlayersCount
        {
            get { return _playersCount; }
            set 
            { 
                if (value >= 0)
                    _playersCount = value; 
            }
        }

        public SportTeam(string name, string coach, int playersCount)
        {
            _name = name;
            _coach = coach;
            _playersCount = playersCount;
        }

        public void PlayMatch()
        {
            Console.WriteLine($"Команда \"{_name}\" під керівництвом тренера {_coach} грає матч у складі {_playersCount} гравців!");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            SportTeam myTeam = new SportTeam("Динамо", "Олександр Шовковський", 11);
            myTeam.PlayMatch();

            myTeam.PlayersCount = 12;
            Console.WriteLine($"Оновлена кількість гравців: {myTeam.PlayersCount}");

            Console.ReadKey();
        }
    }
}