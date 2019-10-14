using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyTaskServerTCP
{
    class StudentsList
    {
        public Student[] Students { get; set; }
        
        public StudentsList(Student[] students)
        {
            Students = students;
        }
    }
}
