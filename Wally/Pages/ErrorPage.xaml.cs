using System.Windows;
using System.Windows.Controls;
using Wally.Models;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class ErrorPage : Page
    {
        public ErrorPage()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            var state = (StateViewModel)(this.DataContext);
            state.Start();
        }
    }
}
