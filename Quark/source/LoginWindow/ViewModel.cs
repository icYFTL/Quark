using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;

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

            LoginWindow_Loaded = new DelegateCommand(() => {
                Globals.socketClient = new Utils.WebSocketClient();
                Globals.socketClient.Connect();
                // TODO: https://github.com/rafallopatka/ToastNotifications/blob/master-v2/Docs/CustomNotificatios.md TOASTS

                Globals.AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Quark");
                Directory.CreateDirectory(Path.Combine(Globals.AppDataPath, "logs"));
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
            foreach (var _temp in obj)
                if (_temp == null)
                    return;

            JObject jobj = new JObject();

            jobj["group"] = obj[0].ToString();
            jobj["username"] = obj[1].ToString();
            jobj["password"] = (obj[2] as PasswordBox).Password;

            //string ans = "";

            //Globals.socketClient.Send(jobj, ref ans);

            Globals.User = new Dictionary<string, string>();
            Globals.User.Add("group", obj[0].ToString());
            Globals.User.Add("username", obj[1].ToString());
            Globals.User.Add("password", (obj[2] as PasswordBox).Password);

            if ((bool)obj[3])
                this.save_auth_data(jobj);

            MainWindow main = new MainWindow();
            main.Show();

            (obj[4] as ModernWindow).Close();
        }

        private void save_auth_data(JObject data)
        {
            File.WriteAllText(Path.Combine(Globals.AppDataPath, "userdata.json"), data.ToString());
        }


        //delegate void Auth(out string message);
       /* private bool auth() TODO
        { 
            JObject jobj = JObject.Parse(message);
            return true ? jobj["status"].ToString() == "OK" : false;
        }*/
    
        public DelegateCommand<string> GroupsItemsSelectionChanged { get; }
        public DelegateCommand<PasswordBox> PasswordField_GotFocus { get; }
        public DelegateCommand<PasswordBox> PasswordField_LostFocus { get; }
        public DelegateCommand LoginWindow_Loaded { get; }
        public DelegateCommand<Object []> LoginCommand { get; }

        public ReadOnlyObservableCollection<string> GroupItems => _model.GroupItems;
        public ReadOnlyObservableCollection<string> StudentItems => _model.StudentItems;

    }
}
