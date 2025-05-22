
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using static RBoard.Logic;
using static RBoard.Logger;

namespace RBoard;

public partial class MainWindow : Window
{
    private readonly char[,] _board = new char[8, 8];
    private int[]? _selectedFigure;
    private int[]? _selectedMove;
    private Grid? _grid;
    private Label? _turnLabel;

    public MainWindow()
    {
        InitializeComponent();
        InitializeBoard();
        InitializeChess();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                _board[row, col] = '0';
            }
        }

        for (int col = 0; col < 8; col++)
        {
            _board[1, col] = 'p';
        }
        _board[0, 0] = 'r';
        _board[0, 7] = 'r';
        _board[0, 1] = 'n';
        _board[0, 6] = 'n';
        _board[0, 2] = 'b';
        _board[0, 5] = 'b';
        _board[0, 3] = 'q';
        _board[0, 4] = 'k';

        for (int col = 0; col < 8; col++)
        {
            _board[6, col] = 'P';
        }
        _board[7, 0] = 'R';
        _board[7, 7] = 'R';
        _board[7, 1] = 'N';
        _board[7, 6] = 'N';
        _board[7, 2] = 'B';
        _board[7, 5] = 'B';
        _board[7, 3] = 'Q';
        _board[7, 4] = 'K';
    }

    private void InitializeChess()
    {
        Info("Chess initialized");
        const int rows = 8;
        _grid = this.FindControl<Grid>("ChessGrid") ?? throw new NullReferenceException("ChessGrid");
        _turnLabel = this.FindControl<Label>("TurnLabel") ?? throw new NullReferenceException("TurnLabel");


        _grid.RowDefinitions.Clear();
        _grid.ColumnDefinitions.Clear();
        _grid.Children.Clear();

        for (int i = 0; i < rows; i++)
        {
            _grid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
            _grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
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
                    Text = GetPieceSymbol(_board[row, col]),
                    FontSize = 40,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Foreground = (row + col) % 2 == 0 ? Brushes.Black : Brushes.White,
                    Background = (row + col) % 2 == 1 ? Brushes.Black : Brushes.White
                };

                cell.Child = piece;

                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);

                cell.PointerPressed += Cell_Clicked;

                _grid.Children.Add(cell);
            }
        }
    }

    private static string GetPieceSymbol(char piece)
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
        if (sender is not Border cell) return;

        var row = Grid.GetRow(cell);
        var col = Grid.GetColumn(cell);
        var piece = _board[row, col];

        if (_selectedFigure == null)
        {
            _selectedFigure = [row, col];
            Debug($"Row: {row}, Col: {col}");
            Debug($"Selected figure: {piece}");
        }
        else if (_selectedFigure != null)
        {
            _selectedMove = [row, col];

            if (!IsLegalMove(_board, _selectedFigure[0], _selectedFigure[1], _selectedMove[0], _selectedMove[1],
                             _board[row, col])) return;

            _board[_selectedMove[0], _selectedMove[1]] = _board[_selectedFigure[0], _selectedFigure[1]];
            _board[_selectedFigure[0], _selectedFigure[1]] = '0';

            _selectedFigure = null;
            UpdateChessGrid();
        }
    }

    private void Rethink(object? sender, RoutedEventArgs e)
    {
        _selectedFigure = null;
    }

    private void UpdateTurn(object? sender, RoutedEventArgs? e)
    {
        if (_turnLabel != null)
            _turnLabel.Content = IsWhiteTurn ? "White Turn" : "Black Turn";
    }

    private void StartButtonPressed(object? sender, RoutedEventArgs e)
    {
        ResetTurn();
        UpdateTurn(null, null);
        InitializeBoard();
        InitializeChess();
    }

    private void UpdateChessGrid()
    {
        UpdateTurn(null, null);
        foreach (var child in (_grid ?? this.GetControl<Grid>("ChessGrid")).Children)
        {
            if (child is not Border cell) continue;

            var row = Grid.GetRow(cell);
            var col = Grid.GetColumn(cell);

            if (cell.Child is not TextBlock piece) continue;

            piece.Text = GetPieceSymbol(_board[row, col]);
            piece.Foreground = (row + col) % 2 == 0 ? Brushes.Black : Brushes.White;
            piece.Background = (row + col) % 2 == 1 ? Brushes.Black : Brushes.White;
        }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        Info("Game Closed");
    }
}
