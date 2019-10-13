using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyTaskServerTCP
{
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }

        private static int count = 0;

        //public static List<Student> Students { get; set; }

        public Student()
        {
            Name = "Name " + count.ToString();
            Group = count.ToString();
            //SetStudents();
            count++;
        }

        public Student(string name, string number)
        {
            Name = name;
            Group = number;
            //SetStudents();
            count++;
        }

        //private void SetStudents()
        //{
        //    Students.Add(this);
        //}
    }
}
