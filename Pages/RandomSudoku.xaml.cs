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
    /// Page in which is Sudoku randomly generated
    /// </summary>
    public partial class RandomSudoku : Page
    {
        private int[,] _GeneratedSudoku;
        private int[,] _SolvedSudoku;
        private Canvas _SudokuArray;
        /// <summary>
        /// Constructor where is Sudoku randomly generated and shown on canvas
        /// </summary>
        public RandomSudoku()
        {
            InitializeComponent();
            // Sudoku generator
            int[,] sudoku = SudokuRandomGenerator();
            // Canvas creator
            Canvas sudokuArray = CanvasMaker(sudoku);
            
            this._GeneratedSudoku = sudoku;
            this._SudokuArray = sudokuArray;

        }
        /// <summary>
        /// Algorithm for random generation of Sudoku
        /// </summary>
        /// <returns> Output is randomly generated Sudoku </returns>
        private int[,] SudokuRandomGenerator()
        {
            Classes.Sudoku sudokuClass = new Classes.Sudoku(Extract);

            int[,] sudoku = new int[9,9];

            // Array filling with zeros
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sudoku[i, j] = 0;
                }
            }

            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                // Random number of numbers in one row
                int quantityOfNumbersInRow = rand.Next(0, 9);
                for (int j = 0; j < quantityOfNumbersInRow; j++)
                {
                    // Index where will number be
                    int index = rand.Next(0, 9);
                    int randNumber = rand.Next(0, 10);
                    sudoku[i, index] = randNumber;
                    // Check for duplicates. If duplicate is finded number on specified index is set to zero
                    bool cond = sudokuClass.Check(sudoku, i, index);
                    if (!cond)
                        sudoku[i, index] = 0;
                }
            }
            return sudoku;
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
        /// Fill canvas with given Sudoku
        /// </summary>
        /// <param name="sudoku"> Randomly generated Sudoku </param>
        /// <returns> Output is Canvas fill with Sudoku </returns>
        public Canvas CanvasMaker(int[,] sudoku)
        {
            Canvas sudokuArray = new Canvas();
            sudokuArray.Background = new SolidColorBrush(Colors.Transparent);
            sudokuArray.Margin = new Thickness(270,50,270,50);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Label l = new Label();
                    l.BorderBrush = new SolidColorBrush(Colors.Black);
                    l.BorderThickness = new Thickness(1.5);
                    l.Content = sudoku[i, j].ToString();
                    Canvas.SetLeft(l, j*25);
                    Canvas.SetTop(l, i*35);

                    sudokuArray.Children.Add(l);
                }
            }
            mainGrid.Children.Add(sudokuArray);
            return sudokuArray;
        }
        /// <summary>
        /// Button witch solves given Sudoku and shows it on Canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SolveButton(object sender, RoutedEventArgs e)
        {
            Classes.Sudoku sudokuClass = new Classes.Sudoku(Extract);
            // Returns solved Sudoku
            int[,] solvedSudoku = await sudokuClass.SudokuMain(_GeneratedSudoku);
            this._SolvedSudoku = solvedSudoku;

            if (solvedSudoku == null)
                MessageBox.Show("Sudoku has no solution");
            else
            {
                // Check if Sudoku has been completed because of pressing button more than once
                bool condition = Completed(this._SudokuArray);
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
        /// Check if Sudoku was already displayed
        /// </summary>
        /// <param name="can"> Canvas with Sudoku </param>
        /// <returns> Output is true if Sudoku was already completed otherwise it is false </returns>
        public bool Completed(Canvas can)
        {
            for(int i = 0; i < can.Children.Count; i++)
            {
                Label l = (Label)can.Children[i];
                int number = int.Parse(l.Content.ToString());
                if (number == 0)
                    return false;
            }
            return true;
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
            try
            {
                int numberInTheBox = int.Parse(numberOfStepsBox.Text);
                if (numberOfStepsBox.Text == "" || numberOfStepsBox.Text == "0")
                    MessageBox.Show("Write a number in the box");
                else
                {
                    Extract.Visibility = Visibility.Visible;
                    int[,] sudokuCopy = this._GeneratedSudoku.Clone() as int[,];     
                    Classes.Sudoku sudokuClass = new Classes.Sudoku(Extract);
                    try
                    {
                        for (int i = 0; i < numberInTheBox; i++)
                        {
                            // Finds first empty cell in given sudoku
                            Extract.Text = "Finding empty cell.";
                            await Task.Delay(500);

                            Tuple<int, int> indexes = sudokuClass.EmptyCell(this._GeneratedSudoku, sudokuCopy);
                            if (indexes == null)
                            {
                                // Check if algorithm completed Sudoku 
                                bool condition = sudokuClass.Completed(sudokuCopy);
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
                            Extract.Text = "Trying number\n from 1 to 9\n and fill empty cell\n with the right one.";
                            await Task.Delay(700);

                            sudokuCopy = await sudokuClass.Filler(this._GeneratedSudoku, sudokuCopy, row, column);

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
                    }
                    catch
                    {
                        MessageBox.Show("Sudoku has no solution");
                    }
                    Extract.Visibility = Visibility.Hidden;
                    stackPanelViz.Visibility = Visibility.Hidden;
                    solvedButton.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                MessageBox.Show("Write a number in the box");
            }
        }
    }
}
