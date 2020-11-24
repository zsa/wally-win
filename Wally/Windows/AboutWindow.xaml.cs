using System.Windows;
using System.Windows.Documents;

namespace Wally
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Hyperlink link = (Hyperlink)sender;
            var url = link.NavigateUri.ToString();
            System.Diagnostics.Process.Start(url);
        }
    }
}
