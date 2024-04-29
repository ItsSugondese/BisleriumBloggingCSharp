using Domain.Blogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging
{
    public interface StudentService
    {
        Task<Student> saveStudent(Student student);
    }
}
