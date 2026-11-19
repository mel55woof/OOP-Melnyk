using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab16v16
{
    public class Course
    {
        public int Id { get; set; }
        public required string Title { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Course> Courses { get; set; } = new();
    }

    public class University
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<Student> Students { get; set; } = new();
    }

    public static class StudentExtensions
    {
        public static bool IsEnrolledIn(this Student student, string courseTitle)
        {
            return student.Courses.Any(c => c.Title.Equals(courseTitle, StringComparison.OrdinalIgnoreCase));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №16: SelectMany, пагінація та Custom Extensions (Варіант 16) ===\n");


            var universities = new List<University>
            {
                new University
                {
                    Id = 1,
                    Name = "National University",
                    Students = new List<Student>
                    {
                        new Student { Id = 101, Name = "Ангеліна Мельник", Courses = new() { new() { Id = 1, Title = "OOP" }, new() { Id = 2, Title = "Databases" } } },
                        new Student { Id = 102, Name = "Іван Петренко", Courses = new() { new() { Id = 1, Title = "OOP" }, new() { Id = 3, Title = "Algorithms" } } },
                        new Student { Id = 103, Name = "Ольга Коваленко", Courses = new() { new() { Id = 4, Title = "Cybersecurity" } } },
                        new Student { Id = 104, Name = "Максим Шевчук", Courses = new() { new() { Id = 2, Title = "Databases" } } },
                        new Student { Id = 105, Name = "Софія Богдан", Courses = new() { new() { Id = 5, Title = "Unity 2D" }, new() { Id = 1, Title = "OOP" } } }
                    }
                },
                new University
                {
                    Id = 2,
                    Name = "Technical Institute",
                    Students = new List<Student>
                    {
                        new Student { Id = 201, Name = "Андрій Ткачук", Courses = new() { new() { Id = 3, Title = "Algorithms" }, new() { Id = 4, Title = "Cybersecurity" } } },
                        new Student { Id = 202, Name = "Катерина Рудь", Courses = new() { new() { Id = 1, Title = "OOP" } } },
                        new Student { Id = 203, Name = "Дмитро Павленко", Courses = new() { new() { Id = 5, Title = "Unity 2D" }, new() { Id = 2, Title = "Databases" } } },
                        new Student { Id = 204, Name = "Вікторія Бондар", Courses = new() { new() { Id = 3, Title = "Algorithms" } } },
                        new Student { Id = 205, Name = "Артем Левченко", Courses = new() { new() { Id = 4, Title = "Cybersecurity" }, new() { Id = 1, Title = "OOP" } } }
                    }
                }
            };

            Console.WriteLine("--- 1. SelectMany: Усі унікальні курси всіх студентів ---");
            var allCourses = universities
                .SelectMany(u => u.Students)
                .SelectMany(s => s.Courses)
                .DistinctBy(c => c.Id)
                .ToList();

            foreach (var course in allCourses)
            {
                Console.WriteLine($" - Курс #{course.Id}: {course.Title}");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. Пагінація: Виведення всіх студентів по 3 на сторінку ---");
            var allStudents = universities.SelectMany(u => u.Students).ToList();
            int pageSize = 3;
            int totalPages = (int)Math.Ceiling(allStudents.Count / (double)pageSize);

            for (int page = 1; page <= totalPages; page++)
            {
                Console.WriteLine($" Сторінка {page} з {totalPages}:");
                var pageStudents = allStudents.Skip((page - 1) * pageSize).Take(pageSize);

                foreach (var student in pageStudents)
                {
                    Console.WriteLine($"   * Студент: {student.Name} (ID: {student.Id}), курсів: {student.Courses.Count}");
                }
            }
            Console.WriteLine();

            Console.WriteLine("--- 3. Custom Extension: Метод IsEnrolledIn() ---");
            var targetStudent = allStudents.First();
            string searchCourse = "OOP";
            bool isEnrolled = targetStudent.IsEnrolledIn(searchCourse);
            Console.WriteLine($" Студент {targetStudent.Name} записаний на курс '{searchCourse}'? -> {isEnrolled}");

            Console.WriteLine("\n=== Роботу програми завершено успішно ===");
        }
    }
}


