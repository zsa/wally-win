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
using System.Windows.Shapes;
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
