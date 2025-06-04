using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string chois;
            while (true)
            {
                Console.WriteLine("Что вы хотите сделать?" +
                    "\n1)Добавить нового студента" +
                    "\n2)Вывести список всех студентов" +
                    "\n3)Обновить данные студента во id" +
                    "\n4)Удалить студента по id\n5)Выйти из программы");

                chois = Console.ReadLine();
                switch (chois) 
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ListStudent();
                        break;
                    case "3":
                        UpdateStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        Console.WriteLine("Производится выход из приложения...");
                        break;
                    default:
                        Console.WriteLine("Некоректный формат ввода!");
                        break;
                }
                if (chois == "5") { break; }
            }
        }

        public static int ValueIsNumber()
        {
            string number;
            while (true) 
            {
                number = Console.ReadLine();
                if (int.TryParse(number, out int result))
                    return result;
                else
                    Console.WriteLine("Неправильный ввод. Повторите попытку");
            }
        }
        
        // Добавление пользователя
        public static void AddStudent()
        {
            try
            {
                Console.WriteLine("Введите имя студента");
                string firstName = Console.ReadLine();
                Console.WriteLine("Введите фамилию студента");
                string lastName = Console.ReadLine();
                Console.WriteLine("Введите день рождения (формат ГГГГ-ММ-ДД)");
                string birthDate = Console.ReadLine();
                while (true)
                {
                    if (Regex.IsMatch(birthDate, @"\d{4}-\d{2}-\d{2}"))
                    {
                        break;
                    }
                    else
                        Console.WriteLine("Введён неверный формат даты. Повторите попытку");
                    birthDate = Console.ReadLine();
                }
                var student = new StudentDb
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = Convert.ToDateTime(birthDate)
                };
                Helper.st.StudentDb.Add(student);
                Helper.st.SaveChanges();
                Console.WriteLine("Студент добавлен");
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Произошла ошибка добавления: " + ex.Message);
            }
        }

        // Вывод 
        public static void ListStudent()
        {
            try
            {    
                if (Helper.st.StudentDb.Count() == 0)
                {
                    Console.WriteLine("Студентов нет");
                    return;
                }
                Console.WriteLine("Вот все студенты");
                foreach (var student in Helper.st.StudentDb)
                {
                    Console.WriteLine($"Id: {student.Id}, имя - {student.FirstName}, фамилия - {student.LastName}, день рождение - {student.BirthDate}");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка вывода: " + ex.Message);
            }
        }

        // Обновление
        public static void UpdateStudent()
        {
            try
            {
                Console.WriteLine("Введите id студента, которого хотите обновить");
                int id = ValueIsNumber();
                if (Helper.st.StudentDb.FirstOrDefault(x => x.Id == id) == null)
                {
                    Console.WriteLine("Студент не найден");
                    return;
                }
                Console.WriteLine("Введите имя студента");
                string firstName = Console.ReadLine();
                Console.WriteLine("Введите фамилию студента");
                string lastName = Console.ReadLine();
                Console.WriteLine("Введите день рождения (формат ГГГГ-ММ-ДД)");
                string birthDate = Console.ReadLine();
                while (true)
                {
                    if (Regex.IsMatch(birthDate, @"\d{4}-\d{2}-\d{2}"))
                    {
                        break;
                    }
                    else
                        Console.WriteLine("Введён неверный формат даты. Повторите попытку");
                    birthDate = Console.ReadLine();
                }
                var student = new StudentDb
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = Convert.ToDateTime(birthDate)
                };
                Helper.st.StudentDb.AddOrUpdate(student);
                Helper.st.SaveChanges();
                Console.WriteLine("Студент обновлён");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка добавления: " + ex.Message);
            }
        }

        // Удаление
        public static void DeleteStudent()
        {
            try
            {
                Console.WriteLine("Введите id студента, которого хотите удалить");
                int id = ValueIsNumber();
                if (Helper.st.StudentDb.FirstOrDefault(x => x.Id == id) == null)
                {
                    Console.WriteLine("Студент не найден");
                    return;
                }
                Console.WriteLine("Точно хотите удалить студента (если да - 1, всё остальное нет)");
                if (Console.ReadLine() != "1")
                {
                    Console.WriteLine("Оерация отменена");
                    return;
                }
                Helper.st.StudentDb.Remove(Helper.st.StudentDb.First(x => x.Id == id));
                Helper.st.SaveChanges();
                Console.WriteLine("Студент удалён");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка удаления: " + ex.Message);
            }
        }
    }

    public class Helper 
    {
        public static StudentsEntities st = new StudentsEntities();
    }
}
