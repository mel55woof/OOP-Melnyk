using System;
using System.Collections.Generic;
using System.Text;

namespace lab10v16
{

    public interface IQueueProcessor<T>
    {
        void ProcessItem(T item);
    }

    public class EmailProcessor : IQueueProcessor<string>
    {
        public void ProcessItem(string item)
        {
            Console.WriteLine($"[EmailProcessor] Відправка Email-сповіщення: '{item}'");
        }
    }

    public class SmsProcessor : IQueueProcessor<string>
    {
        public void ProcessItem(string item)
        {
            Console.WriteLine($"[SmsProcessor] Відправка SMS-сповіщення: '{item}'");
        }
    }

    public abstract class WorkflowStep
    {
        public string StepName { get; set; }

        protected WorkflowStep(string stepName)
        {
            StepName = stepName;
        }

        public abstract bool ExecuteStep();

        public void LogStepCompletion(string stepName)
        {
            Console.WriteLine($"[WorkflowLog] Крок '{stepName}' успішно зафіксовано в журналі аудиту.");
        }
    }

    public class DataValidationStep : WorkflowStep
    {
        public DataValidationStep() : base("Валідація вхідних даних") { }

        public override bool ExecuteStep()
        {
            Console.WriteLine($"[WorkflowStep] Виконання: {StepName}. Перевірка схеми та формату...");
            bool isSuccess = true;
            LogStepCompletion(StepName);
            return isSuccess;
        }
    }

    public class DataTransformationStep : WorkflowStep
    {
        public DataTransformationStep() : base("Трансформація даних") { }

        public override bool ExecuteStep()
        {
            Console.WriteLine($"[WorkflowStep] Виконання: {StepName}. Перетворення даних у формат JSON...");
            bool isSuccess = true;
            LogStepCompletion(StepName); 
            return isSuccess;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Демонстрація роботи лаб. 10 (Варіант 16: Інтерфейси та Абстрактні класи) ===\n");

            Console.WriteLine("--- 1. Демонстрація поліморфізму через колекцію Інтерфейсів ---");
            List<IQueueProcessor<string>> processors = new List<IQueueProcessor<string>>
            {
                new EmailProcessor(),
                new SmsProcessor(),
                new EmailProcessor()
            };

            string messagePayload = "Замовлення #10042 успішно сформовано";

            foreach (var processor in processors)
            {
                processor.ProcessItem(messagePayload);
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Демонстрація роботи Абстрактного класу та його наслідування ---");
            List<WorkflowStep> pipeline = new List<WorkflowStep>
            {
                new DataValidationStep(),
                new DataTransformationStep()
            };

            foreach (var step in pipeline)
            {
                bool result = step.ExecuteStep();
                Console.WriteLine($"Результат виконання кроку [{step.StepName}]: {(result ? "Успішно" : "Помилка")}\n");
            }

            Console.WriteLine("=== Виконання програми завершено ===");
        }
    }
}
