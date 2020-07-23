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
    /// Page in which Sudoku is set by a user
    /// </summary>
    public partial class Own : Page
    {
        private Canvas _SudokuArray;
        private int setButtorClicked = 0;
        private int[,] notSolvedSudoku;
        private int[,] _SolvedSudoku;
        private Pages.RandomSudoku randomSudokuClass;
        /// <summary>
        /// Constructor where empty array is set a shown on canvas.
        /// </summary>
        public Own()
        {
            InitializeComponent();

            this.notSolvedSudoku = new int[9, 9] {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            Canvas cas = CanvasMaker(notSolvedSudoku);

            this._SudokuArray = cas;
        }
        /// <summary>
        /// Returns to Menu page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainMenu(object sender, RoutedEventArgs e)
        {
            Pages.Menu rp = new Pages.Menu();
            this.NavigationService.Navigate(rp);
        }
        /// <summary>
        /// Button which display Sudoku rows on canvas by user's choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Set(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if button is clicked more than nine times  
                if (setButtorClicked >= 9)
                    throw new ArithmeticException();

                string[] s = rowBox.Text.Split(',');

                // User has to write nine numbers
                if (s.Length != 9)
                    throw new Exception("error");

                // Displays writed rows on Canvas
                for (int i = 0; i < 9; i++)
                {
                    int.Parse(s[i]);
                    Label l = (Label)_SudokuArray.Children[i + 9 * setButtorClicked];
                    l.Content = s[i];
                }
                rowBox.Text = "0,0,0,0,0,0,0,0,0";
                setButtorClicked++;
                if (setButtorClicked >= 9)
                {
                    stackPanelViz.Visibility = Visibility.Visible;
                    // Method that transform numbers from canvas to Sudoku array
                    generateSudoku();
                }
            }
            catch (ArithmeticException)
            {
                MessageBox.Show("Sudoku is filled");
            }
            catch
            {
                MessageBox.Show("Write nine comma separeted numbers");
            }
        }
        /// <summary>
        /// Button that start a Visualization of algorithm by steps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks> 
        /// Users set how many steps will program goes through. Boxes through algorithm goes are red. 
        /// </remarks>
        private async void Visualization(object sender, RoutedEventArgs e)
        {
            Extract.Visibility = Visibility.Visible;
            try
            {
                int.Parse(numberOfStepsBox.Text);
            }
            catch
            {
                MessageBox.Show("Write a number in the box");
            }
            if (numberOfStepsBox.Text == "" || numberOfStepsBox.Text == "0")
                MessageBox.Show("Write a number in the box");
            else
            {
                int[,] sudokuCopy = this.notSolvedSudoku.Clone() as int[,];
                Classes.Sudoku viz = new Classes.Sudoku(Extract);
                try
                {
                    for (int i = 0; i < int.Parse(numberOfStepsBox.Text); i++)
                    {
                        // Finds first empty cell in given sudoku
                        Extract.Text = "Finding empty cell.";
                        await Task.Delay(500);

                        Tuple<int, int> indexes = viz.EmptyCell(this.notSolvedSudoku, sudokuCopy);
                        if (indexes == null)
                        {
                            bool condition = viz.Completed(sudokuCopy);
                            if (!condition)
                            {
                                throw new Exception("Error");
                            }
                            else
                            {
                                MessageBox.Show("Solved");
                                break;
                            }
                        }

                        int row = indexes.Item1;
                        int column = indexes.Item2;
                        // Calculation of index on canvas by row and column
                        int canvasIndex = row * 9 + column;

                        Label l = (Label)this._SudokuArray.Children[canvasIndex];
                        l.Background = new SolidColorBrush(Colors.Red);

                        // Filler method fill right number in empty cell by row and column
                        Extract.Text = "Try number\n from 1 to 9\n and fill empty cell\n with the right one.";
                        await Task.Delay(700);

                        sudokuCopy = await viz.Filler(this.notSolvedSudoku, sudokuCopy, row, column);
                            
                        if (sudokuCopy == null)
                            throw new Exception("Error");

                        l.Background = new SolidColorBrush(Colors.Transparent);
                       
                        // Display Sudoku on Canvas
                        int temp = 0;
                        for (int k = 0; k < 9; k++)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                l = (Label)this._SudokuArray.Children[temp];
                                l.Content = sudokuCopy[k, j].ToString();
                                temp++;
                            }
                        }
                        await Task.Delay(500);


                    }
                    Extract.Visibility = Visibility.Hidden;
                    stackPanelViz.Visibility = Visibility.Hidden;
                    solvedButton.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("Sudoku has no solution");
                    stackPanelViz.Visibility = Visibility.Hidden;
                }
            }
        }
        /// <summary>
        /// Button witch solves given Sudoku and shows it on Canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SolveButton(object sender, RoutedEventArgs e)
        {
            randomSudokuClass = new Pages.RandomSudoku();
            Classes.Sudoku sudokuClass = new Classes.Sudoku(Extract);
            // Returns solved Sudoku
            int[,] solvedSudoku = await sudokuClass.SudokuMain(notSolvedSudoku);
            this._SolvedSudoku = solvedSudoku;

            if (solvedSudoku == null)
                MessageBox.Show("Sudoku has no solution");
            else
            {
                // Check if Sudoku has been completed because of pressing button more than once
                bool condition = randomSudokuClass.Completed(this._SudokuArray);
                if (condition)
                    MessageBox.Show("Sudoku has been solved");
                else
                {
                    // Display completed Sudoku on Canvas
                    int temp = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            Label l = (Label)this._SudokuArray.Children[temp];
                            l.Content = this._SolvedSudoku[i, j].ToString();
                            temp++;
                        }

                    }
                }
            }
        }
        /// <summary>
        /// Method that transform numbers from canvas to Sudoku array
        /// </summary>
        private void generateSudoku()
        {
            try
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        Label l = (Label)this._SudokuArray.Children[i + 9 * j];
                        this.notSolvedSudoku[j, i] = int.Parse(l.Content.ToString());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Parse Error");
            }

        }
        /// <summary>
        /// Fill canvas with given Sudoku
        /// </summary>
        /// <param name="sudoku"> Randomly generated Sudoku </param>
        /// <returns> Output is Canvas fill with Sudoku </returns>
        public Canvas CanvasMaker(int[,] sudoku)
        {
            Canvas sudokuArray = new Canvas();
            sudokuArray.Background = new SolidColorBrush(Colors.Transparent);
            sudokuArray.Margin = new Thickness(280, 60, 250, 50);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label l = new Label();
                    l.BorderBrush = new SolidColorBrush(Colors.Black);
                    l.BorderThickness = new Thickness(1.5);
                    l.Content = sudoku[i, j].ToString();
                    Canvas.SetLeft(l, j * 25);
                    Canvas.SetTop(l, i * 35);

                    sudokuArray.Children.Add(l);
                }
            }
            mainGrid.Children.Add(sudokuArray);
            return sudokuArray;
        }
    }
}
