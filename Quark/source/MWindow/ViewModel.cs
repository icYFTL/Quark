using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace Quark.source.MWindow
{
    class ViewModel : BindableBase, IDropTarget
    {
        readonly Model _model = new Model();
        public ViewModel()
        {
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };

            ImportCodeCommand = new DelegateCommand(() => {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "CPP Code (*.cpp)|*.cpp|Text files (*.txt)|*.txt";
                dialog.FilterIndex = 2;

                Nullable<bool> result = dialog.ShowDialog();

                if (result == true)
                {
                    string filename = dialog.FileName;
                    if (File.Exists(filename))
                        UpdateCodeTab(filename);
                }
            });
        }
        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {
                var extension = Path.GetExtension(item);
                return extension != null && (extension.Equals(".cpp") || extension.Equals(".txt"));
            }) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            dropInfo.Effects = dragFileList.Any(item =>
            {
                var extension = Path.GetExtension(item);
                if (extension.Equals(".cpp") || extension.Equals(".txt") && extension != null)
                    UpdateCodeTab(item);
                return extension != null && (extension.Equals(".cpp") || extension.Equals(".txt"));
            }) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        public void UpdateCodeTab(string path) // Bad way to solve the problem. * REFACTOR *
        {
            //((Application.Current.Windows[0]) as MainWindow).CodeBox.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(path).Trim())));
        }

        public DelegateCommand ImportCodeCommand { get; }
    }
}
