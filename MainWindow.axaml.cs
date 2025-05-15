using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace rboard;

public partial class MainWindow : Window
{
    private char[,] board = new char[8, 8];
    private int _selectedRow = 0; // Выбранная строка
    private int _selectedCol = 0; // Выбранный столбец

    public MainWindow()
    {
        InitializeComponent();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                board[row, col] = '0';
            }
        }

        // Белые фигуры
        for (int col = 0; col < 8; col++)
        {
            board[1, col] = 'P'; // Пешки
        }
        board[0, 0] = 'R'; board[0, 7] = 'R'; // Ладьи
        board[0, 1] = 'N'; board[0, 6] = 'N'; // Кони
        board[0, 2] = 'B'; board[0, 5] = 'B'; // Слоны
        board[0, 3] = 'Q'; // Ферзь
        board[0, 4] = 'K'; // Король

        // Чёрные фигуры
        for (int col = 0; col < 8; col++)
        {
            board[6, col] = 'p'; // Пешки
        }
        board[7, 0] = 'r'; board[7, 7] = 'r'; // Ладьи
        board[7, 1] = 'n'; board[7, 6] = 'n'; // Кони
        board[7, 2] = 'b'; board[7, 5] = 'b'; // Слоны
        board[7, 3] = 'q'; // Ферзь
        board[7, 4] = 'k'; // Король
    }

    private void InitializeChess()
    {
        const int rows = 8;
        var chessGrid = this.FindControl<Grid>("ChessGrid") ?? throw new NullReferenceException("ChessGrid"); 


        chessGrid.RowDefinitions.Clear();
        chessGrid.ColumnDefinitions.Clear();
        chessGrid.Children.Clear();

        for (int i = 0; i < rows; i++)
        {
            chessGrid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
            chessGrid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < rows; col++)
            {
                var cell = new Border
                {
                    Background = (row + col) % 2 == 0 ? Brushes.White : Brushes.Black,
                    BorderBrush = SolidColorBrush.Parse("#333333"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(5),
                    RenderTransform = new ScaleTransform(1, 1)
                };

                var piece = new TextBlock
                {
                    Text = GetPieceSymbol(board[row, col]),
                    FontSize = 24,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = (row + col) % 2 == 0 ? Brushes.Black : Brushes.White
                };

                cell.Child = piece;

                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);

                cell.PointerPressed += Cell_Clicked;

                chessGrid.Children.Add(cell);
            }
        }
    }

    private string GetPieceSymbol(char piece)
    {
        return piece switch
        {
            'R' => "♖", 'r' => "♜",
            'N' => "♘", 'n' => "♞",
            'B' => "♗", 'b' => "♝",
            'Q' => "♕", 'q' => "♛",
            'K' => "♔", 'k' => "♚",
            'P' => "♙", 'p' => "♟",
            _ => ""
        };
    }

    private void Cell_Clicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Border cell)
        {
            var row = Grid.GetRow(cell); 
            var col = Grid.GetColumn(cell); 
            Console.WriteLine(row);
            Console.WriteLine(col);
            char piece = board[row, col];
            
            if (char.IsUpper(piece) && piece != '0')
            {
                if (_selectedRow == null || _selectedCol == null)
                {
                    _selectedRow = row;
                    _selectedCol = col;
                    cell.Background = Brushes.LightGreen;
                }
                else
                {
                    if (IsLegalMove(board, _selectedRow, _selectedCol, row, col, piece))
                    {
                        board[row, col] = piece; // Перемещаем фигуру
                        board[_selectedRow, _selectedCol] = '0'; // Очищаем исходную позицию
                        InitializeChess(); // Перерисовываем доску
                        _selectedRow = 0;
                        _selectedCol = 0;
                    }
                    else
                    {
                        InitializeChess(); // Возвращаем исходный вид
                        _selectedRow = 0;
                        _selectedCol = 0;
                    }
                }
            }
            else if (_selectedRow != null && _selectedCol != null)
            {
                if (IsLegalMove(board, _selectedRow, _selectedCol, row, col, board[_selectedRow, _selectedCol]))
                {
                    board[row, col] = board[_selectedRow, _selectedCol];
                    board[_selectedRow, _selectedCol] = '0';
                    InitializeChess();
                    _selectedRow = 0;
                    _selectedCol = 0;
                }
                else
                {
                    InitializeChess(); // Сбрасываем при нелегальном ходе
                    _selectedRow = 0;
                    _selectedCol = 0;
                }
            }
        }
    }

    private bool IsLegalMove(char[,] board, int fromRow, int fromCol, int toRow, int toCol, char piece)
    {
        if (fromRow == toRow && fromCol == toCol)
            return false;
        return true;
    }

    private void StartButtonPressed(object? sender, RoutedEventArgs e)
    {
        InitializeBoard();
        InitializeChess();
    }
}