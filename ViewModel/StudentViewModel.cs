using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfCrudMvvmExample.DataAccess;
using WpfCrudMvvmExample.Model;

namespace WpfCrudMvvmExample.ViewModel
{
	public class StudentViewModel
	{
		private ICommand _saveCommand;
		private ICommand _resetCommand;
		private ICommand _editCommand;
		private ICommand _delteCommand;
		private StudentRepository _repository;
		private Students _studentEntity = null;
		public StudentRecord StudentRecord { get; set; }
		public StudentEntities StudentEntities { get; set; }

		public ICommand ResetCommand
		{
			get
			{
				if (_resetCommand == null)
					_resetCommand = new RelayCommand(param => ResetData(), null);
				return _resetCommand;
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
					_saveCommand = new RelayCommand(param => SaveData(), null);
				return _saveCommand;
			}
		}

		public ICommand EditCommand
		{
			get
			{
				if (_editCommand == null)
					_editCommand = new RelayCommand(param => EditData((int)param), null);
				return _editCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_delteCommand == null)
					_delteCommand = new RelayCommand(param => DeleteStudent((int)param), null);
				return _delteCommand;
			}
		}

		public StudentViewModel()
		{
			_studentEntity = new Students();
			_repository = new StudentRepository();
			StudentRecord = new StudentRecord();
			GetAll();
		}

		public void ResetData()
		{
			StudentRecord.Name = string.Empty;
			StudentRecord.Id = 0;
			StudentRecord.Address = string.Empty;
			StudentRecord.Contact = string.Empty;
			StudentRecord.Age = 0;
		}

		public void DeleteStudent(int id)
		{
			if (MessageBox.Show("Confirm delete of this record?", "Student", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				try
				{
					_repository.RemoveStudent(id);
					MessageBox.Show("Record successfully deleted");
				}
				catch(Exception ex)
				{
					MessageBox.Show("Error occured while saving." + ex.InnerException);
				}
				finally
				{
					GetAll();
				}
			}
		}

		public void SaveData()
		{
			if (StudentRecord != null)
			{
				_studentEntity.Name = StudentRecord.Name;
				_studentEntity.Age = StudentRecord.Age;
				_studentEntity.Address = StudentRecord.Address;
				_studentEntity.Contact = StudentRecord.Contact;

				try
				{
					if (StudentRecord.Id <= 0)
					{
						_repository.AddStudent(_studentEntity);
						MessageBox.Show("New record successfully saved.");
					}
					else
					{
						_studentEntity.ID = StudentRecord.Id;
						_repository.UpdateStudent(_studentEntity);
						MessageBox.Show("Record successfully updated.");
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show("Error occured while saving. " + ex.InnerException);
				}
				finally
				{
					GetAll();
					ResetData();
				}
			}
		}

		public void EditData(int id)
		{
			var model = _repository.Get(id);
			StudentRecord.Id = model.ID;
			StudentRecord.Name = model.Name;
			StudentRecord.Age = (int)model.Age;
			StudentRecord.Address = model.Address;
			StudentRecord.Contact = model.Contact;

		}

		public void GetAll()
		{
			StudentRecord.StudentRecords = new ObservableCollection<StudentRecord>();
			_repository.GetAll().ForEach(data => StudentRecord.StudentRecords.Add(new StudentRecord()
			{
				Id = data.ID,
				Name = data.Name,
				Age = Convert.ToInt32(data.Age),
				Address = data.Address,
				Contact = data.Contact
			}));
		}
	}
}
