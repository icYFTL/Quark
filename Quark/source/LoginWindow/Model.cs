using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MaterialDesignThemes.Wpf;

namespace Quark.source.LoginWindow
{
    class Model : BindableBase {


        private SnackbarMessageQueue _snackBarMessageQueue;
        public SnackbarMessageQueue snackBarMessageQueue
        {
            get { return _snackBarMessageQueue; }
            set { _snackBarMessageQueue = value; }
        }

        private readonly ObservableCollection<string> _groupItems = new ObservableCollection<string>();
        public readonly ReadOnlyObservableCollection<string> GroupItems;

        private readonly ObservableCollection<string> _studentItems = new ObservableCollection<string>();
        public readonly ReadOnlyObservableCollection<string> StudentItems;
        public Model()
        {
            StudentItems = new ReadOnlyObservableCollection<string>(_studentItems);
            GroupItems = new ReadOnlyObservableCollection<string>(_groupItems);
        }

        public void UpdateGroups(List<string> _data)
        {
            foreach (var _t in _data)
                _groupItems.Add(_t);

            RaisePropertyChanged("GroupItems");
        }

        public void UpdateStudents(List<string> _data)
        {
            foreach (var _t in _data)
                _studentItems.Add(_t);

            RaisePropertyChanged("StudentItems");
        }

        public void StudentsClear()
        {
            _studentItems.Clear();
        }
    }
}
