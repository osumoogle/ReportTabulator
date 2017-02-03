using ReportTabulator.Viewmodels;

namespace ReportTabulator
{
    public partial class MainWindow
    {
        public MainWindowViewmodel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewmodel();
            DataContext = ViewModel;
        }
    }
}
