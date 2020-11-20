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
    public partial class FirmwareSelect : Page
    {
        public FirmwareSelect()
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
    }
}
