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

namespace Sudoku
{
    /// <summary>
    /// Main window of the application
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Sets the content of the window to the menu page
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Pages.Menu menu = new Pages.Menu();
            this.Content = menu;
        }
    }
}
