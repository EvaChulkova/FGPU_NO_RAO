using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContactsTest
{
    internal class DetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
            if(SaveCommand != null)
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DetailViewModel(MainData mainData, bool isNew) { 
            MainData = mainData;
            IsNew = isNew;
            lastname = MainData.lastName;
            firstname = MainData.firstName;
            patronicalname = MainData.patronicalName;
            birthdate = MainData.birthDate;
            nickname = MainData.nickname;
            comment = MainData.comment;
            number =new ObservableCollection<Phones>( MainData.phoneNumber??new List<Phones>());

            SaveCommand = new OCommand(save, canSave);
            CloseCommand = new OCommand(close);


        }
        private MainData MainData;
        private bool IsNew;
        public ObservableCollection<Phones> number
        {
            get { return _number; }
            set {
                _number = value;
                OnPropertyChanged(nameof(number));
            }
        }
        private ObservableCollection<Phones> _number;

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { 
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }

        private OCommand _saveCommand;

        public OCommand SaveCommand
        {
            get { return _saveCommand; }
            set { _saveCommand = value; }
        }

        public OCommand CloseCommand
        {
            get; set;
        }

        private void close()
        {
            DialogResult = true;
        }



        private bool canSave()
        {
            return !string.IsNullOrEmpty(lastname) && !string.IsNullOrEmpty(firstname);
        }

        private async void save()
        {
            MainData.lastName = lastname;
            MainData.firstName = firstname;
            MainData.patronicalName = patronicalname;
            MainData.birthDate = birthdate;
            MainData.nickname = nickname;
            MainData.comment = comment;
            MainData.phoneNumber = number.ToList();


            //Save to BD
            using (PhoneDataContext db = new PhoneDataContext())
            {
                var id = -1;
                if (IsNew)
                {
                    id = await db.MainData.CountAsync();
                    MainData.id = id  > 0 ? await db.MainData.Select(t => t.id).MaxAsync() +1 : 1;
                    db.MainData.Add(MainData);
                    db.Phones.AddRange(MainData.phoneNumber);
                    id = (await db.Phones.CountAsync()) > 0 ? await db.Phones.Select(t => t.id).MaxAsync() : 1;
                    foreach (var item in MainData.phoneNumber)
                    {
                        item.id = id + 1;
                        id = id + 1;
                    }
                }
                else
                {
                    db.Update(MainData);
                    id = await db.Phones.CountAsync() > 0 ? await db.Phones.Select(t => t.id).MaxAsync() : 1;
                    foreach (var item in MainData.phoneNumber)
                    {
                        if (item.id == default)
                        {
                            item.id = id + 1;
                            id = id + 1;
                            db.Add(item);
                        }
                        else db.Update(item);
                    }
                }
                await db.SaveChangesAsync();

            }
            IsNew = false;
            MessageBox.Show("Done");
        }


        public String lastname
        {
            get { return _lastname; }
            set
            {
                _lastname = value;
                OnPropertyChanged(nameof(lastname));
            }
        }
        private string _lastname;


        public string firstname
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(firstname));
            }
        }
        private string _firstName;


        public string patronicalname
        {
            get { return _patronicalname; }
            set
            {
                _patronicalname = value;
                OnPropertyChanged(nameof(patronicalname));
            }
        }
        private string _patronicalname;


        public DateTime birthdate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(birthdate));
            }
        }
        private DateTime _birthDate;


        public string nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(nickname));
            }
        }
        private string _nickname;


        public string comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(comment));
            }
        }
        private string _comment;
    }
}
