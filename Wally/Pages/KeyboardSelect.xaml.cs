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
    /// Interaction logic for KeyboardSelect.xaml
    /// </summary>
    public partial class KeyboardSelect : Page
    {
        public KeyboardSelect()
        {
            DataContext = App.Current.MainWindow.DataContext;
            InitializeComponent();
        }
    }
}
