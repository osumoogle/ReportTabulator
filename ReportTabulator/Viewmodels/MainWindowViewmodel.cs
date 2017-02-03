using System.Threading;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using PropertyChanged;
using ReportTabulatorLibrary;

namespace ReportTabulator.Viewmodels
{
    [ImplementPropertyChanged]
    public class MainWindowViewmodel
    {
        public string FileName { get; set; }
        public string Separator { get; set; }
        public string Results { get; set; }
        public Visibility ShouldShowResults => string.IsNullOrWhiteSpace(Results) ? Visibility.Hidden : Visibility.Visible;

        public ICommand ChooseFileCommand { get; set; }
        public ICommand ProcessFileCommand { get; set; }

        public MainWindowViewmodel()
        {
            ChooseFileCommand = new RelayCommand(ChooseFile);
            ProcessFileCommand = new RelayCommand(ProcessFile);
            Separator = "|";
        }

        private void ChooseFile()
        {
            new Thread(() =>
            {
                var dlg = new OpenFileDialog {Filter = "csv|*.csv"};
                dlg.ShowDialog();
                FileName = dlg.FileName;
            }) {IsBackground = true}.Start();
        }

        private void ProcessFile()
        {
            if (string.IsNullOrWhiteSpace(FileName)) return;
            var parser = new CsvParser(FileName, Separator);
            Results = parser.Process();
        }
        
    }
}
