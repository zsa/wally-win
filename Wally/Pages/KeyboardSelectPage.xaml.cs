using System.Windows.Controls;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for KeyboardSelect.xaml
    /// </summary>
    public partial class KeyboardSelectPage : Page
    {
        public KeyboardSelectPage()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }
    }
}
