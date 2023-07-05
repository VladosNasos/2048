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

namespace _2048
{
    public partial class MainWindow : Window
    {
        private const int BoardSize = 4;
        private int[,] board;
        private bool isGameOver;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            board = new int[BoardSize, BoardSize];
            isGameOver = false;
            AddRandomTile();
            AddRandomTile();
            UpdateBoard();
        }

        private void UpdateBoard()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    int value = board[row, col];
                    TextBlock tile = GetTile(row, col);
                    tile.Text = value > 0 ? value.ToString() : "";
                    tile.Style = GetTileStyle(value);
                }
            }
        }

        private TextBlock GetTile(int row, int col)
        {
            string tileName = "tile_" + row + col;
            return (TextBlock)LogicalTreeHelper.FindLogicalNode(BoardGrid, tileName);
        }

        private Style GetTileStyle(int value)
        {
            string styleName = "Tile" + value;
            return (Style)Resources[styleName];
        }

        private void AddRandomTile()
        {
            List<int> emptyCells = new List<int>();

            for (int r = 0; r < BoardSize; r++)
            {
                for (int cock = 0; cock < BoardSize; cock++)
                {
                    if (board[r, cock] == 0)
                    {
                        emptyCells.Add(r * BoardSize + cock);
                    }
                }
            }

            if (emptyCells.Count == 0)
            {
                return;
            }

            Random random = new Random();
            int randomIndex = random.Next(0, emptyCells.Count);
            int cell = emptyCells[randomIndex];
            int row = cell / BoardSize;
            int col = cell % BoardSize;

            board[row, col] = random.Next(1, 3) * 2; // Generates 2 or 4
        }

        private void Move(Direction direction)
        {
            if (isGameOver)
            {
                return;
            }

            bool moved = false;

            switch (direction)
            {
                case Direction.Up:
                    moved = MoveUp();
                    break;
                case Direction.Down:
                    moved = MoveDown();
                    break;
                case Direction.Left:
                    moved = MoveLeft();
                    break;
                case Direction.Right:
                    moved = MoveRight();
                    break;
            }

            if (moved)
            {
                AddRandomTile();
                UpdateBoard();

                if (IsGameOver())
                {
                    MessageBox.Show("Game Over!");
                    isGameOver = true;
                }
            }
        }

        private bool MoveUp()
        {
            bool moved = false;

            for (int c = 0; c < BoardSize; c++)
            {
                for (int r = 1; r < BoardSize; r++)
                {
                    if (board[r, c] != 0)
                    {
                        for (int k = r; k > 0; k--)
                        {
                            if (board[k - 1, c] == 0)
                            {
                                board[k - 1, c] = board[k, c];
                                board[k, c] = 0;
                                moved = true;
                            }
                            else if (board[k - 1, c] == board[k, c])
                            {
                                board[k - 1, c] *= 2;
                                board[k, c] = 0;
                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveDown()
        {
            bool moved = false;

            for (int c = 0; c < BoardSize; c++)
            {
                for (int r = BoardSize - 2; r >= 0; r--)
                {
                    if (board[r, c] != 0)
                    {
                        for (int k = r; k < BoardSize - 1; k++)
                        {
                            if (board[k + 1, c] == 0)
                            {
                                board[k + 1, c] = board[k, c];
                                board[k, c] = 0;
                                moved = true;
                            }
                            else if (board[k + 1, c] == board[k, c])
                            {
                                board[k + 1, c] *= 2;
                                board[k, c] = 0;
                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveLeft()
        {
            bool moved = false;

            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 1; c < BoardSize; c++)
                {
                    if (board[r, c] != 0)
                    {
                        for (int k = c; k > 0; k--)
                        {
                            if (board[r, k - 1] == 0)
                            {
                                board[r, k - 1] = board[r, k];
                                board[r, k] = 0;
                                moved = true;
                            }
                            else if (board[r, k - 1] == board[r, k])
                            {
                                board[r, k - 1] *= 2;
                                board[r, k] = 0;
                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private bool MoveRight()
        {
            bool moved = false;

            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = BoardSize - 2; c >= 0; c--)
                {
                    if (board[r, c] != 0)
                    {
                        for (int k = c; k < BoardSize - 1; k++)
                        {
                            if (board[r, k + 1] == 0)
                            {
                                board[r, k + 1] = board[r, k];
                                board[r, k] = 0;
                                moved = true;
                            }
                            else if (board[r, k + 1] == board[r, k])
                            {
                                board[r, k + 1] *= 2;
                                board[r, k] = 0;
                                moved = true;
                                break;
                            }
                        }
                    }
                }
            }

            return moved;
        }

        private bool IsGameOver()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == 0 ||
                        (row > 0 && board[row - 1, col] == board[row, col]) ||
                        (row < BoardSize - 1 && board[row + 1, col] == board[row, col]) ||
                        (col > 0 && board[row, col - 1] == board[row, col]) ||
                        (col < BoardSize - 1 && board[row, col + 1] == board[row, col]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    Move(Direction.Up);
                    break;
                case Key.Down:
                    Move(Direction.Down);
                    break;
                case Key.Left:
                    Move(Direction.Left);
                    break;
                case Key.Right:
                    Move(Direction.Right);
                    break;
                case Key.R:
                    NewGame();
                    break;
            }
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
