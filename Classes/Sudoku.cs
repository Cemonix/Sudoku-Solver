using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sudoku.Classes
{
    /// <summary>
    /// Main components for Sudoku solution
    /// </summary>
    class Sudoku : Page
    {
        public TextBlock Extract { get; private set; }
        /// <summary>
        /// Constructor that initializes variables
        /// </summary>
        /// <param name="extract"></param>
        public Sudoku(TextBlock extract)
        {
            this.Extract = extract;
        }
        /// <summary>
        /// Main method which run algorithm 
        /// </summary>
        /// <param name="sudoku"> Gets empty Sudoku </param>
        /// <returns>
        /// Standard output is filled sudoku. If sudoku has no solution it return null. 
        /// </returns>
        /// <remarks> 
        /// Firstly it makes copy o empty sudoku. After that it calls method EmptyCell that find first empty cell in the copy. 
        /// After it check if sudoku is filled. If not method Filled is called and filling begin. Whole process is repeated untill sudoku is filled.
        /// </remarks>
        public async Task<int[,]> SudokuMain(int[,] sudoku)
        {
            bool condition = false;
            int[,] sudokuCopy = sudoku.Clone() as int[,];
            while (!condition)
            {
                // Searching for empty cell
                Tuple<int, int> indexes = EmptyCell(sudoku, sudokuCopy);
                if (indexes == null)
                {
                    // Check if Sudoku is completed
                    condition = Completed(sudokuCopy);
                    if (!condition)
                    {
                        return null;
                    }
                }
                else
                {
                    // Call method which try fill right number in given position
                    sudokuCopy = await Filler(sudoku, sudokuCopy, indexes.Item1, indexes.Item2);
                    if(sudokuCopy == null)
                    {
                        return null;
                    }
                    // Check if Sudoku is completed
                    condition = Completed(sudokuCopy);
                }
            }
            return sudokuCopy;
        }
        /// <summary>
        /// Check if sudoku is filled/completed
        /// </summary>
        /// <param name="sudoku"> Gets the sudoku </param>
        /// <returns>
        /// If sudoku is filled/completed it returns boolean true otherwise false
        /// </returns>
        public bool Completed(int[,] sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Finds first empty cell in given sudoku.
        /// </summary>
        /// <param name="sudoku"> Empty original sudoku </param>
        /// <param name="sudokuCopy"> Sudoku which is going to be filled </param>
        /// <returns>
        /// Tuple with number of row and column of empty cell. If empty cell is not found returns null.
        /// </returns>
        public Tuple<int, int> EmptyCell(int[,] sudoku, int[,] sudokuCopy)
        {
            Tuple<int, int> tuple = null;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0 && sudokuCopy[i, j] == 0)
                    {
                        int row = i;
                        int column = j;
                        tuple = Tuple.Create(i, j);
                        return tuple;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Most important method. Put number in given position. If it can't put the number in position it calls Backtrack method.
        /// </summary>
        /// <param name="sudoku"> Empty original sudoku </param>
        /// <param name="sudokuCopy"> Sudoku which is going to be filled </param>
        /// <param name="row"> Number of row where number is located </param>
        /// <param name="column"> Number of column where number is located </param>
        /// <param name="number"> Starting number which will be increased untill it fits in sudoku </param>
        /// <returns> Method output is sudoku with right number in given position. If attempt fails output is null. </returns>
        /// <remarks> Firstly method is trying put number which is increasing in given position. 
        /// There is a method called Check which monitor if number can be put in postion. If it fill the position method return the sudoku.
        /// If it fails the Backtrack method is called. After that method tries another numbers untill it fill right one. </remarks>
        public async virtual Task<int[,]> Filler(int[,] sudoku, int[,] sudokuCopy, int row, int column, int number = 1)
        {
            try
            {
                bool condition = false;
                // Trying put right number in Sudoku
                for (int i = number; i < 10; i++)
                {
                    sudokuCopy[row, column] = i;
                    // Check for duplicates
                    condition = Check(sudokuCopy, row, column);
                    // Right number finded
                    if (condition)
                    {
                        break;
                    }
                }
                // Can't put ant number in positon
                if (!condition)
                {
                    // Check if program is not in the end of Sudoku
                    if (column == 0 && row == 0)
                    {
                        throw new Exception("Zadne reseni");
                    }
                    else
                    {
                        // Filled number is set to zero
                        sudokuCopy[row, column] = 0;
                        (row, column, number) = await BackTrack(sudoku, sudokuCopy, row, column - 1);
                        // Inversion call untill program finds right number or fails.
                        sudokuCopy = await Filler(sudoku, sudokuCopy, row, column, number);
                    }
                }
            }
            catch
            {
                // Program fails. Sudoku has no solution
                return null;
            }
            return sudokuCopy;
        }
        /// <summary>
        /// This method only checks for duplicate numbers in row, column and 3x3 box where is located filling number.
        /// </summary>
        /// <param name="sudoku"> Sudoku trying to be filled</param>
        /// <param name="row"> Number of row where number is located </param>
        /// <param name="column"> Number of column where number is located </param>
        /// <returns> Output is true if there are no duplicates. </returns>
        /// <remarks> There is a special method CheckBoxes that controls box where the number is located. </remarks>
        public bool Check(int[,] sudoku, int row, int column)
        {
            //Check row and column duplicate
            bool condition = true;
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[row, i] == sudoku[row, column] && i != column)
                {
                    condition = false;
                    break;
                }
                if (sudoku[row, column] == sudoku[i, column] && i != row)
                {
                    condition = false;
                    break;
                }
            }

            //Check the boxes for duplicate in every posibility
            if (condition)
            {
                if (row >= 0 && row < 3)
                {
                    if (column >= 0 && column < 3)
                    {
                        condition = CheckBoxes(sudoku, row, column, 0, 0);
                    }
                    else if (column >= 3 && column < 6)
                    {
                        condition = CheckBoxes(sudoku, row, column, 0, 3);
                    }
                    else if (column >= 6 && column < 9)
                    {
                        condition = CheckBoxes(sudoku, row, column, 0, 6);
                    }
                }
                else if (row >= 3 && row < 6)
                {
                    if (column >= 0 && column < 3)
                    {
                        condition = CheckBoxes(sudoku, row, column, 3, 0);
                    }
                    else if (column >= 3 && column < 6)
                    {
                        condition = CheckBoxes(sudoku, row, column, 3, 3);
                    }
                    else if (column >= 6 && column < 9)
                    {
                        condition = CheckBoxes(sudoku, row, column, 3, 6);
                    }
                }
                else if (row >= 6 && row < 9)
                {
                    if (column >= 0 && column < 3)
                    {
                        condition = CheckBoxes(sudoku, row, column, 6, 0);
                    }
                    else if (column >= 3 && column < 6)
                    {
                        condition = CheckBoxes(sudoku, row, column, 6, 3);
                    }
                    else if (column >= 6 && column < 9)
                    {
                        condition = CheckBoxes(sudoku, row, column, 6, 6);
                    }
                }
            }
            return condition;
        }
        /// <summary>
        /// Check the box where the number is located.
        /// </summary>
        /// <param name="sudoku"> Sudoku </param>
        /// <param name="rowOfCell"> Number of row of cell </param>
        /// <param name="columnOfCell"> Number of column of cell </param>
        /// <param name="row"> Number of row of the box </param>
        /// <param name="column"> Number of column of the box </param>
        /// <returns> Returns true if there are no duplicates otherwise false. </returns>
        private bool CheckBoxes(int[,] sudoku, int rowOfCell, int columnOfCell, int row, int column)
        {
            bool condition = true;
            for (int i = row; i < row + 3; i++)
            {
                for (int j = column; j < column + 3; j++)
                {
                    if (sudoku[rowOfCell, columnOfCell] == sudoku[i, j] && i != rowOfCell && j != columnOfCell)
                    {
                        condition = false;
                    }
                }
            }
            return condition;
        }
        /// <summary>
        /// Changes number of row, column and number which should be put in sudoku.
        /// </summary>
        /// <param name="sudoku"> Empty original Sudoku </param>
        /// <param name="sudokuCopy"> Copy of Sudoku </param>
        /// <param name="row"> Number of row where number is located </param>
        /// <param name="column"> Number of column where number is located </param>
        /// <returns> Output is tuple with new row, column and number </returns>
        public async Task<Tuple<int, int, int>> BackTrack(int[,] sudoku, int[,] sudokuCopy, int row, int column)
        {
            int number = 0;
            if (Extract.Visibility == System.Windows.Visibility.Visible)
            {
                Extract.Text = "Backtracking \n-> move to the last \nright element";
                await Task.Delay(500);
            }
            // End of the sudoku
            while (row >= 0)
            {
                for (int i = column; i >= 0; i--)
                {
                    // Move to location where is zero in original sudoku due to unwanted rewrite 
                    // Check if number in current sudoku is nine and in original sudoku is zero
                    if (sudokuCopy[row, i] == 9 && sudoku[row, i] == 0)
                    {
                        sudokuCopy[row, i] = 0;
                        continue;
                    }
                    // Check if only in original sudoku is zero
                    if (sudoku[row, i] == 0)
                    {
                        column = i;
                        // Number increased by one because it muset be grater than before
                        number = sudokuCopy[row, column] + 1;
                        Tuple<int, int, int> tuple = Tuple.Create(row, column, number);
                        return tuple;
                    }
                }
                // Program gets to the end of row. Decrease the number of row and set number of column to the end of a new row
                row--;
                column = 8;
            }
            throw new Exception("No solution");
        }
    }
}
