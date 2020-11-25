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
using Microsoft.Win32;
using Wally.Models;
using UtilsLibrary;

namespace Wally.Pages
{
    /// <summary>
    /// Interaction logic for FirmwareSelect.xaml
    /// </summary>
    public partial class FirmwareSelectPage : Page
    {
        public FirmwareSelectPage()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }

        private void FileSelect_Click(object sender, RoutedEventArgs e)
        {

            var state = (StateViewModel)(this.DataContext);

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select firmware file";
            dlg.Filter = $"Firmware file ({state.FileExtension})|*{state.FileExtension}";

            if(dlg.ShowDialog() == true)
            {
                var fileName = dlg.FileName;
                if (fileName != String.Empty) state.SelectFirmare(fileName);
            }
        }

        private void Page_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                // If multiple files are dropped we keep the first one and ignore the rest.
                var firstFile = files[0];
                var state = (StateViewModel)(this.DataContext);
                state.SelectFirmare(firstFile);
            }
            this.DropTarget.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void Page_DragEnter(object sender, DragEventArgs e)
        {
            this.DropTarget.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#20000000"));
        }

        private void Page_DragLeave(object sender, DragEventArgs e)
        {
            this.DropTarget.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
