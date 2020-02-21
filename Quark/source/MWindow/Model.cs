using Prism.Mvvm;


namespace Quark.source.MWindow
{
    class Model : BindableBase
    {
        private string _userNameTitle;
        public string UserNameTitle
        {
            get
            {
                return _userNameTitle;
            }
            set
            {
                _userNameTitle = value;
                RaisePropertyChanged("UserNameTitle");
            }
        }
    }
}
