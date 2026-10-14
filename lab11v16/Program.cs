using System;
using System.Collections.Generic;
using System.Text;

namespace lab11v16
{
    public abstract class TemplateEngine
    {
        public string TemplateName { get; set; }
     public bool IsRenderedSuccessfully { get; protected set; }

        protected TemplateEngine(string templateName)
        {
            TemplateName = templateName;
            IsRenderedSuccessfully = false;
        }

        public void Render(string data)
        {
            Console.WriteLine($"\n--- Початок рендерингу шаблону: [{TemplateName}] ---");
            
            LoadTemplate();
            InjectData(data);
            ApplyFormatting();
            
            if (Validate())
            {
                Output();
                IsRenderedSuccessfully = true;
                Console.WriteLine($"[Успіх] Шаблон '{TemplateName}' успішно згенеровано.");
            }
            else
            {
                IsRenderedSuccessfully = false;
                Console.WriteLine($"[Помилка] Валідація шаблону '{TemplateName}' провалена.");
            }
        }

        protected abstract void LoadTemplate();
        protected abstract void InjectData(string data);

        protected virtual void ApplyFormatting()
        {
            Console.WriteLine("[Форматування] Застосовано базове форматування тексту.");
        }

        protected virtual bool Validate()
        {
            Console.WriteLine("[Валідація] Базова перевірка структури проведена.");
            return true;
        }

        protected abstract void Output();
    }

    public class HTMLTemplate : TemplateEngine
    {
        public HTMLTemplate() : base("HTML Документ") { }

        protected override void LoadTemplate()
        {
            Console.WriteLine("[HTML] Завантаження шаблону index.html з тегами <html> та <body>...");
        }

        protected override void InjectData(string data)
        {
            Console.WriteLine($"[HTML] Вставка даних у DOM-дерево: <div>{data}</div>");
        }

        protected override void ApplyFormatting()
        {
            Console.WriteLine("[HTML] Підключення CSS-стилів та форматування HTML-структури.");
        }

        protected override void Output()
        {
            Console.WriteLine("[HTML] Генерація підсумкового файлу output.html.");
        }
    }

    public class EmailTemplate : TemplateEngine
    {
        public string Subject { get; set; } = "Системне сповіщення";

        public EmailTemplate() : base("Email Лист") { }

        protected override void LoadTemplate()
        {
            Console.WriteLine("[Email] Завантаження макету листа з шапкою та футером...");
        }

        protected override void InjectData(string data)
        {
            Console.WriteLine($"[Email] Підстановка теми '{Subject}' та вмісту: {data}");
        }

        protected override void ApplyFormatting()
        {
            Console.WriteLine("[Email] Застосування inline-стилів для сумісності з поштовими клієнтами.");
        }

        protected override bool Validate()
        {
            Console.WriteLine("[Email] Перевірка відповідності макету вимогам поштових антиспам-фільтрів.");
            return true;
        }

        protected override void Output()
        {
            Console.WriteLine("[Email] Формування MIME-повідомлення для відправки через SMTP.");
        }
    }

    public class PDFTemplate : TemplateEngine
    {
        public PDFTemplate() : base("PDF Звіт") { }

        protected override void LoadTemplate()
        {
            Console.WriteLine("[PDF] Ініціалізація векторного документа з макетом сторінки A4...");
        }

        protected override void InjectData(string data)
        {
            Console.WriteLine($"[PDF] Запис текстових блоків та таблиць даних: {data}");
        }

        protected override bool Validate()
        {
            Console.WriteLine("[PDF] Перевірка виходу тексту за межі сторінки та коректності шрифтів.");
            return true;
        }

        protected override void Output()
        {
            Console.WriteLine("[PDF] Експорт та бінарна компресія у файл report.pdf.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 11 (Варіант 16: Template Engine Pattern) ===");

            List<TemplateEngine> engines = new List<TemplateEngine>
            {
                new HTMLTemplate(),
                new EmailTemplate(),
                new PDFTemplate()
            };

            string sampleData = "Користувач: Ангеліна | Статус: Активний";

            foreach (var engine in engines)
            {
                engine.Render(sampleData);
            }

            Console.WriteLine("\n=== Виконання алгоритму Template Method завершено ===");
        }
    }
}
