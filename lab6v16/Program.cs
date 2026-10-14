using System;
using System.Collections.Generic;
using System.Text;

namespace lab6v16
{
    public class Document
    {
        private string _fileName;
        private long _fileSize; 

        public string FileName
        {
            get => _fileName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я файлу не може бути порожнім.");
                _fileName = value;
            }
        }

        public long FileSize
        {
            get => _fileSize;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Розмір файлу не може бути від'ємним.");
                _fileSize = value;
            }
        }

        public Document(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }

        public virtual void Open()
        {
            Console.WriteLine($"[Document] Відкриття базового документа '{FileName}' (Розмір: {FileSize} байт).");
        }

        public string GetDocumentType()
        {
            return "Базовий документ (Document)";
        }
    }

    public class TextDocument : Document
    {
        public int WordCount { get; set; }

        public TextDocument(string fileName, long fileSize, int wordCount)
            : base(fileName, fileSize)
        {
            WordCount = wordCount;
        }

        public override void Open()
        {
            Console.WriteLine($"[TextDocument] Відкриття текстового документа '{FileName}' ({WordCount} слів). Читання тексту...");
        }

        public void EditContent()
        {
            Console.WriteLine($"[TextDocument] Редагування вмісту файлу '{FileName}'...");
        }
        public new string GetDocumentType()
        {
            return "Текстовий документ (TextDocument)";
        }
    }

    public class ImageDocument : Document
    {
        public string Resolution { get; set; }

        public ImageDocument(string fileName, long fileSize, string resolution)
            : base(fileName, fileSize) 
        {
            Resolution = resolution;
        }

        public override void Open()
        {
            Console.WriteLine($"[ImageDocument] Відкриття графічного документа '{FileName}' (Роздільна здатність: {Resolution}). Рендеринг зображення...");
        }

        public void ResizeImage()
        {
            Console.WriteLine($"[ImageDocument] Зміна розміру зображення '{FileName}'...");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 6 (Варіант 16: Document Hierarchy) ===\n");

            Document baseDoc = new Document("abstract.dat", 1024);
            TextDocument textDoc = new TextDocument("lab_report.docx", 20480, 1500);
            ImageDocument imgDoc = new ImageDocument("screenshot.png", 524288, "1920x1080");

            Console.WriteLine("--- 1. Демонстрація поліморфізму (через масив базового типу Document[]) ---");
            Document[] documents = { baseDoc, textDoc, imgDoc };

            foreach (var doc in documents)
            {
            
                doc.Open();
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Виклик унікальних методів похідних класів ---");
            textDoc.EditContent();
            imgDoc.ResizeImage();
            Console.WriteLine();

            Console.WriteLine("--- 3. Демонстрація різниці між override та new ---");
    
            Console.WriteLine($"textDoc.GetDocumentType() [тип TextDocument]: {textDoc.GetDocumentType()}");

            Document textDocAsBase = textDoc;
            Console.WriteLine($"textDocAsBase.GetDocumentType() [посилання типу Document]: {textDocAsBase.GetDocumentType()}");

            Console.WriteLine("\nПояснення: При new вибір методу залежить від ТИПУ ПОСИЛАННЯ, а не від об'єкта в пам'яті!");

            Console.WriteLine("\n=== Виконання програми завершено ===");
        }
    }
}
