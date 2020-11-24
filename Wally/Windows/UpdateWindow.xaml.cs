using System.Windows;
using Wally.Models;

namespace Wally
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private UpdateViewModel _update;
        public UpdateWindow(UpdateViewModel update)
        {
            _update = update;
            DataContext = update;
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Download_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(_update.Url);
            var mainWindow = (MainWindow)App.Current.MainWindow;
            mainWindow.Close();
        }
    }
}
