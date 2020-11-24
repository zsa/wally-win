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
using Wally.Converters;
using Microsoft.Win32;
using System.Reflection;

namespace Wally
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string AppVersion 
        {
            get {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
        public MainWindow()
        {
            DataContext = new StateViewModel(AppVersion);
            InitializeComponent();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            _ = new Updater(AppVersion);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var state = (StateViewModel)(this.DataContext);
            state.Start();
        }
        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            var state = (StateViewModel)(this.DataContext);
            state.ToggleLog();
        }

        private void Version_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            About dlg = new About();

            dlg.Owner = this;

            dlg.ShowDialog();
        }
        public void ShowUpdateDialog(UpdateViewModel update)
        {
            var dlg = new UpdateWindow(update);
            dlg.Owner = this;
            dlg.ShowDialog();
        }
    }
}
