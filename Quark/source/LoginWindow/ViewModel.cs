using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;

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

            LoginCommand = new DelegateCommand<Object[]>(obj =>
           {
               Login(obj);

           });

            LoginWindow_Loaded = new DelegateCommand(() =>
            {
                Logs.Logs._Init();

                if (!Task.Run(() => source.Utils.NetStat.is_connected()).Result)
                {
                    MessageBox.Show("Не удается подключиться к сети интернет.", "FATAL", MessageBoxButton.OK, MessageBoxImage.Error);
                    Globals.Logger.Error("FATAL: Net connection error.");    
                    Environment.Exit(-1);
                }
                Globals.socketClient = Utils.WebSocketClient.get_instance();
                Globals.socketClient.Connect();
                // TODO: https://github.com/rafallopatka/ToastNotifications/blob/master-v2/Docs/CustomNotificatios.md TOASTS


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

        public void Login(Object[] obj)
        {
            foreach (var _temp in obj)
                if (_temp == null)
                    return;

            JObject jobj = new JObject();

            jobj["group"] = obj[0].ToString();
            jobj["username"] = obj[1].ToString();
            jobj["password"] = (obj[2] as PasswordBox).Password;
            jobj["operation"] = "auth";

            List<object> obj_ = obj.ToList();
            obj_.Add(jobj);

            auth_ a = new auth_(Auth);

            Globals.socketClient.Send(jobj, a, obj_);

        }

        public static void save_auth_data(JObject data)
        {
            File.WriteAllText(Path.Combine(Globals.AppDataPath, "userdata.json"), data.ToString());
        }

        delegate bool auth_(JObject repl, ref List<object> obj);

        public bool Auth(JObject repl, ref List<object> obj)
        {
            if (repl["status"].ToString() == "OK")
            {
                Globals.User = new Dictionary<string, string>();
                Globals.User.Add("group", obj[0].ToString());
                Globals.User.Add("username", obj[1].ToString());
                Globals.User.Add("password", (obj[2] as PasswordBox).Password);

                if ((bool)obj[3])
                    save_auth_data(obj[5] as JObject);

                MainWindow main = new MainWindow();
                main.Show();

                (obj[4] as ModernWindow).Close();
                return true;
            }
            else
            {
                MessageBox.Show(repl["description"].ToString(), repl["status"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public DelegateCommand<string> GroupsItemsSelectionChanged { get; }
        public DelegateCommand<PasswordBox> PasswordField_GotFocus { get; }
        public DelegateCommand<PasswordBox> PasswordField_LostFocus { get; }
        public DelegateCommand LoginWindow_Loaded { get; }
        public DelegateCommand<Object[]> LoginCommand { get; }

        public ReadOnlyObservableCollection<string> GroupItems => _model.GroupItems;
        public ReadOnlyObservableCollection<string> StudentItems => _model.StudentItems;

    }
}
