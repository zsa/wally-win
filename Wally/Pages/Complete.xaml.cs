using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wally.Models;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class Complete : Page
    {
        public Complete()
        {
            InitializeComponent();
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            var state = (StateViewModel)(App.Current.MainWindow.DataContext);
            state.Start();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            return;
        }
    }
}
