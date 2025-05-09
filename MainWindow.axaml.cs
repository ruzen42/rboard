using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace rboard;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeChess();
    }

    private void InitializeChess()
    { 
       const int rows = 8;
       var chessGrid = this.FindControl<Grid>("ChessGrid"); 

       for (int row = 0; row < rows; row++)
       {
          chessGrid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star)); 
          chessGrid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star)); 
       }

       for (int col = 0; col < rows; col++)
       {
           for (int row = 0; row < rows; row++)
           {
               var cell = new Border
               {
                   Background = (row + col) % 2 == 0 ? Brushes.Azure : Brushes.Black,
                   BorderBrush = SolidColorBrush.Parse("#333333"),
                   BorderThickness = new Thickness(0, 5),
                   CornerRadius = new CornerRadius(2),
                   RenderTransform = new ScaleTransform(1, 1)
               };
               Grid.SetRow(cell, row); 
               Grid.SetColumn(cell, col);
               
               cell.PointerPressed += (s, e) => Cell_Clicked(s, e, row, col);
           }
       }
    }
    
    private void Cell_Clicked(object? sender, RoutedEventArgs e, int row, int col)
    {
        var cell = sender as Border;
        cell.Background = Brushes.LightGoldenrodYellow;
    }

    private void StartButtonPressed(object? sender, RoutedEventArgs e)
    {
        InitializeChess();
    }
}