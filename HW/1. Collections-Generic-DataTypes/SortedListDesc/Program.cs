using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SortedListDesc
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList<int, Student> list = new();
            list.Add(1, new Student { Name = "Andrey", Age = 20 });
            list.Add(2, new Student { Name = "Artiom", Age = 21 });
            list.Add(3, new Student { Name = "Sergey", Age = 23 });
            list.Add(4, new Student { Name = "Stas", Age = 20 });

            Console.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));  // unsorted by Age

            Console.WriteLine("\n---------------\n");

            List<KeyValuePair<int, Student>> listDesc = SortStudentsByAge(list);  

            Console.WriteLine(JsonConvert.SerializeObject(listDesc, Formatting.Indented)); // sorted by Age Desc

        }

        public static List<KeyValuePair<int,Student>> SortStudentsByAge(SortedList<int, Student> sortedListOfStudents)
        {
                return sortedListOfStudents.OrderByDescending(x => x.Value.Age).ToList(); 
        }
    }

    class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }

    }
}
