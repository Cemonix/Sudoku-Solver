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

namespace Sudoku.Pages
{
    /// <summary>
    /// Menu page
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button that gets user to random generated sudoku page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Random(object sender, RoutedEventArgs e)
        {
            Pages.RandomSudoku rp = new Pages.RandomSudoku();
            frame.Navigate(rp);
        }
        /// <summary>
        /// Button that gets user to page where sudoku is write manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Own(object sender, RoutedEventArgs e)
        {
            Pages.Own own = new Pages.Own();
            frame.Navigate(own);
        }
    }
}
