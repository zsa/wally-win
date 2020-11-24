using System.Windows.Controls;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for Flashing.xaml
    /// </summary>
    public partial class FlashingPage : Page
    {
        public FlashingPage()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }
    }
}
