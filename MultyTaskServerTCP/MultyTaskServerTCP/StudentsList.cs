using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultyTaskServerTCP
{
    static class StudentsList
    {
        static public List<Student> Students { get; set; }

        static public void SetStudents(Student st)
        {
            Students.
                Add
                (st);
        }
    }
}
