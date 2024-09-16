using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ContactsTest 
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public List<MainData> Items { 
            get { return _items; } 
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            } }
        private List<MainData> _items;

        public MainData selectedData
        {
            get
            {
                return _selectedData;
            } 
            set
            {
                _selectedData = value;
                OnPropertyChanged(nameof(selectedData));
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        private MainData _selectedData;

        
        public void delete()
        {
            if ( MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление", 
                MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                using (PhoneDataContext db = new PhoneDataContext())
                {
                    db.RemoveRange(selectedData.phoneNumber);
                    db.Remove(selectedData);
                    db.SaveChanges();
                }
            }
            Filter();
        }
        
        public OCommand DeleteCommand
        {
            get; set;
        }


        public void add()
        {
            DetailViewModel viewModel = new DetailViewModel(new MainData() { 
                birthDate = DateTime.Now.AddYears(-5).Date }
            , true);
            var view = new DetailForm() { DataContext = viewModel };
            view.ShowDialog();
            Filter();
        }

        public OCommand AddCommand
        {
            get; 
            set;
        }


        public void edit()
        {
            DetailViewModel viewModel = new DetailViewModel(selectedData, false);
            var view = new DetailForm() { DataContext = viewModel };
            view.ShowDialog();
            Filter();
        }

        public bool canEdit()
        {
            return selectedData != null;
        }

        public OCommand EditCommand
        {
            get; set;
        }


        public MainViewModel()
        {
            Items = new List<MainData>();
            EditCommand = new OCommand(edit, canEdit);
            AddCommand = new OCommand(add);
            DeleteCommand = new OCommand(delete, canEdit);
            Filter();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public String lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(lastname));
                Filter();
            }
        }
        private string _lastname;

        public async void Filter()
        {
            selectedData = null;

            using (PhoneDataContext db = new PhoneDataContext())
            {
                var items = db.MainData.Select(t => t);
                items = items.Include(t => t.phoneNumber);

                if (!string.IsNullOrEmpty(lastname))
                {
                    items = items.Where(t => t.lastName.IndexOf(lastname) != -1);
                }

                if (!string.IsNullOrEmpty(firstname))
                {
                    items = items.Where(t => t.firstName.IndexOf(firstname) != -1);
                }
                Items = await items.ToListAsync();
            }
        }

        private string _firstName;

        public string firstname
        {
            get { return _firstName; }
            set { _firstName = value;
                OnPropertyChanged(nameof(firstname));
                Filter();
            }
        }
    }
}
