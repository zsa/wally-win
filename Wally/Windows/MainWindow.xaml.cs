using System;
using System.Windows;
using System.Windows.Input;
using Wally.Models;
using System.Reflection;
using System.IO;

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
            var model = new StateViewModel(AppVersion);
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var filePath = args[1];
                if (File.Exists(filePath))
                {
                    var extension = System.IO.Path.GetExtension(filePath);
                    if (extension == ".hex" || extension == ".bin")
                    {
                        model.SelectFirmare(filePath);
                    }
                }
            }
            DataContext = model;
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
            AboutWindow dlg = new AboutWindow();

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
