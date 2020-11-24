using System.Windows.Controls;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        public LogsPage()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }
    }
}
