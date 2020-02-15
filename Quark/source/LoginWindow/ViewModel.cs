using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Quark.source.Utils.Database;
using Prism.Mvvm;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;

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
            InternalBD _bd = InternalBD.getInstance();
            _model.UpdateGroups(_bd.GetGroups());

        }

        public void UpdateStudents(string group)
        {
            InternalBD _bd = InternalBD.getInstance();
            _model.UpdateStudents(_bd.GetStudents(group));
        }

        public void Login(Object [] obj)
        {
            // TO DO
            MainWindow main = new MainWindow();
            main.Show();
            (obj[1] as ModernWindow).Close();
        }


    
        public DelegateCommand<string> GroupsItemsSelectionChanged { get; }
        public DelegateCommand<PasswordBox> PasswordField_GotFocus { get; }
        public DelegateCommand<PasswordBox> PasswordField_LostFocus { get; }
        public DelegateCommand<Object []> LoginCommand { get; }

        public ReadOnlyObservableCollection<string> GroupItems => _model.GroupItems;
        public ReadOnlyObservableCollection<string> StudentItems => _model.StudentItems;

    }
}
