using System.Windows;
using System.Windows.Controls;
using Wally.Models;

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
        private void Copy_Log_Button_Click(object sender, RoutedEventArgs e)
        {
            var state = (StateViewModel)(this.DataContext);
            state.CopyToClipboard();
        }
    }
}
