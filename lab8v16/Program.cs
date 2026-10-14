using System;
using System.Collections.Generic;
using System.Text;

namespace lab8v16
{
    public class User
    {
        public string Username { get; set; }

        public User(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Ім'я користувача не може бути порожнім.", nameof(username));

            Username = username;
        }

        public virtual string GetPermissions()
        {
            return $"Базовий доступ для користувача '{Username}'";
        }
    }

    public class Admin : User
    {
        public string Department { get; set; }

        public Admin(string username, string department) : base(username)
        {
            Department = department;
        }

        public override string GetPermissions()
        {
            return $"[АДМІНІСТРАТОР] {Username} (Департамент: {Department}): Повний доступ до системи, керування користувачами та налаштуваннями.";
        }
    }

    public class Moderator : User
    {
        public string ForumName { get; set; }

        public Moderator(string username, string forumName) : base(username)
        {
            ForumName = forumName;
        }

        public override string GetPermissions()
        {
            return $"[МОДЕРАТОР] {Username} (Форум: {ForumName}): Доступ до модерації контенту, блокування порушників та редагування тем.";
        }
    }

    public class Guest : User
    {
        public string SessionId { get; set; }

        public Guest(string username, string sessionId) : base(username)
        {
            SessionId = sessionId;
        }

        public override string GetPermissions()
        {
            return $"[ГОСТЬ] {Username} (Сесія: {SessionId}): Обмежений доступ, лише перегляд публічних сторінок.";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 8 (Варіант 16: User Permissions) ===\n");

            List<User> users = new List<User>
            {
                new Admin("Ангеліна (Root)", "IT Безпека"),
                new Moderator("Максим_Mod", "Програмування C#"),
                new Guest("Гість_8921", "SESS-99214"),
                new Admin("Дмитро_Admin", "Системне адміністрування"),
                new Guest("Гість_1042", "SESS-00123")
            };

            Console.WriteLine("--- 1. Поліморфний виклик методів GetPermissions() для кожного об'єкта ---");
            
            List<string> aggregatedPermissions = new List<string>();
            int adminCount = 0, moderatorCount = 0, guestCount = 0;

            foreach (var user in users)
            {
                string permission = user.GetPermissions();
                aggregatedPermissions.Add(permission);

                if (user is Admin) adminCount++;
                else if (user is Moderator) moderatorCount++;
                else if (user is Guest) guestCount++;

                Console.WriteLine(permission);
            }

            Console.WriteLine("\n--- 2. Агреговані результати (Звіт) ---");
            Console.WriteLine($"Всього користувачів у колекції: {users.Count}");
            Console.WriteLine($" - Адміністраторів: {adminCount}");
            Console.WriteLine($" - Модераторів:     {moderatorCount}");
            Console.WriteLine($" - Гостей:          {guestCount}");
            Console.WriteLine($"Збережено записів у підсумковому списку дозволів: {aggregatedPermissions.Count}");

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}

