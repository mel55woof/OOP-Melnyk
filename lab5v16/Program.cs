using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lab5v16
{
    public class ContactList : IEquatable<ContactList>
    {
        private readonly Dictionary<string, string> _contacts = new(StringComparer.OrdinalIgnoreCase);

        public int Count => _contacts.Count;

        public string this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Ім'я не може бути порожнім.", nameof(name));

                if (_contacts.TryGetValue(name, out var phone))
                    return phone;

                throw new KeyNotFoundException($"Контакт з ім'ям '{name}' не знайдено.");
            }
            set
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Ім'я не може бути порожнім.", nameof(name));

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Номер телефону не може бути порожнім.", nameof(value));

                _contacts[name] = value;
            }
        }

        public void AddContact(string name, string phone)
        {
            this[name] = phone;
        }

        public bool ContainsName(string name) => _contacts.ContainsKey(name);

        public static ContactList operator +(ContactList list1, ContactList list2)
        {
            if (list1 is null && list2 is null) return new ContactList();
            if (list1 is null) return list2.Clone();
            if (list2 is null) return list1.Clone();

            var result = list1.Clone();
            foreach (var pair in list2._contacts)
            {
                result._contacts[pair.Key] = pair.Value;
            }
            return result;
        }

        public static bool operator ==(ContactList? list1, ContactList? list2)
        {
            if (ReferenceEquals(list1, list2)) return true;
            if (list1 is null || list2 is null) return false;
            return list1.Equals(list2);
        }

        public static bool operator !=(ContactList? list1, ContactList? list2)
        {
            return !(list1 == list2);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ContactList);
        }

        public bool Equals(ContactList? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (_contacts.Count != other._contacts.Count) return false;

            foreach (var pair in _contacts)
            {
                if (!other._contacts.TryGetValue(pair.Key, out var otherPhone) || pair.Value != otherPhone)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var pair in _contacts)
            {
                hash = hash * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(pair.Key);
                hash = hash * 31 + pair.Value.GetHashCode();
            }
            return hash;
        }

        public override string ToString()
        {
            if (_contacts.Count == 0)
                return "Список контактів порожній.";

            var sb = new StringBuilder();
            sb.AppendLine($"Список контактів (всього: {Count}):");
            foreach (var pair in _contacts)
            {
                sb.AppendLine($" - {pair.Key}: {pair.Value}");
            }
            return sb.ToString().TrimEnd();
        }

        public ContactList Clone()
        {
            var clone = new ContactList();
            foreach (var pair in _contacts)
            {
                clone._contacts[pair.Key] = pair.Value;
            }
            return clone;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 5 (Варіант 16: ContactList) ===\n");

            // 1. Створення об'єктів
            var personalContacts = new ContactList();
            personalContacts.AddContact("Ангеліна", "+380971112233");
            personalContacts.AddContact("Максим", "+380504445566");

            var workContacts = new ContactList();
            workContacts["Олена (Деканат)"] = "+380362000000"; 
            workContacts["Максим"] = "+380509999999"; 

            Console.WriteLine("1. Персональні контакти:");
            Console.WriteLine(personalContacts);
            Console.WriteLine();

            Console.WriteLine("2. Робочі контакти:");
            Console.WriteLine(workContacts);
            Console.WriteLine();

            Console.WriteLine($"3. Читання за допомогою індексатора:");
            Console.WriteLine($"Номер Ангеліни: {personalContacts["Ангеліна"]}");
            Console.WriteLine();

            ContactList allContacts = personalContacts + workContacts;
            Console.WriteLine("4. Результат об'єднання (personalContacts + workContacts):");
            Console.WriteLine(allContacts);
            Console.WriteLine();

            var duplicatePersonal = personalContacts.Clone();

            Console.WriteLine("5. Перевірка рівності:");
            Console.WriteLine($"personalContacts == duplicatePersonal: {personalContacts == duplicatePersonal}");
            Console.WriteLine($"personalContacts == workContacts: {personalContacts == workContacts}");
            Console.WriteLine($"personalContacts.Equals(duplicatePersonal): {personalContacts.Equals(duplicatePersonal)}");
            Console.WriteLine();

            Console.WriteLine("6. Перевірка виняткових ситуацій (неіснуючий контакт):");
            try
            {
                string phone = personalContacts["Іван"];
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Помилка спіймана: {ex.Message}");
            }

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}
