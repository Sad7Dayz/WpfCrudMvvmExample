using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using WpfCrudMvvmExample.Model;

namespace WpfCrudMvvmExample.DataAccess
{
	public class StudentRepository
	{
		private StudentEntities studentContext = null;

		public StudentRepository()
		{
			studentContext = new StudentEntities();
		}

		public Students Get(int id)
		{
			return studentContext.Students.Find(id);
		}

		public List<Students> GetAll()
		{
			return studentContext.Students.ToList();
		}

		public void AddStudent(Students student)
		{
			if (student != null)
			{
				studentContext.Students.Add(student);
				studentContext.SaveChanges();
			}
		}

		public void UpdateStudent(Students student)
		{
			var studentFind = this.Get(student.ID);
			if (studentFind != null)
			{
				studentFind.Name = student.Name;
				studentFind.Contact = student.Contact;
				studentFind.Age = student.Age;
				studentFind.Address = student.Address;
				studentContext.SaveChanges();
			}
		}

		public void RemoveStudent(int id)
		{
			var studObj = studentContext.Students.Find(id);
			if (studObj != null)
			{
				studentContext.Students.Remove(studObj); 
				studentContext.SaveChanges();
			}
		}
	}
}