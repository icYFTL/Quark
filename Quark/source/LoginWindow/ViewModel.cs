using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;

namespace Quark.source.LoginWindow
{
    class ViewModel : BindableBase
    {
        readonly Model _model = new Model();
        
        public ViewModel()
        {
           
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            
            GroupsItemsSelectionChanged = new DelegateCommand<string>(str =>
            {
                if (GroupItems.Contains(str))
                    UpdateStudents(str);
            });
            

            PasswordField_GotFocus = new DelegateCommand<PasswordBox>(pbox =>
            {
                if (pbox.Password == "Password")
                    pbox.Password = "";
            });

            PasswordField_LostFocus = new DelegateCommand<PasswordBox>(pbox =>
            {
                if (pbox.Password == "")
                    pbox.Password = "Password";
            });

            LoginCommand = new DelegateCommand<Object []>(obj =>
            {
                Login(obj);

            });

            UpdateGroups();
        }

        public void UpdateGroups()
        {

            List<string> list = new List<string>();
            list.Add("ИКБО-16-19");
            list.Add("ИКБО-17-19");
            _model.UpdateGroups(list);

        }

        public void UpdateStudents(string group)
        {
            _model.StudentsClear();
            List<string> list = new List<string>();
            list.Add("БЕЛЯВСКИЙ");
            list.Add("ППАВЛАВ");
            list.Add("Ковалев Алексей");
            _model.UpdateStudents(list);
        }

        public void Login(Object [] obj)
        {
            // TO DO -> Realese the login method throught websockets;

            foreach (var _temp in obj)
                if (_temp == null)
                    return;

                

            Globals.group = obj[0].ToString();
            Globals.snp = obj[1].ToString();
            Globals.password = (obj[2] as PasswordBox).Password;
            Globals.remember = (bool)obj[3];

            MainWindow main = new MainWindow();
            main.Show();

            (obj[4] as ModernWindow).Close();
        }

 
    
        public DelegateCommand<string> GroupsItemsSelectionChanged { get; }
        public DelegateCommand<PasswordBox> PasswordField_GotFocus { get; }
        public DelegateCommand<PasswordBox> PasswordField_LostFocus { get; }
        public DelegateCommand<Object []> LoginCommand { get; }

        public ReadOnlyObservableCollection<string> GroupItems => _model.GroupItems;
        public ReadOnlyObservableCollection<string> StudentItems => _model.StudentItems;

        public SnackbarMessageQueue snackBarMessageQueue => _model.snackBarMessageQueue;
    }
}
