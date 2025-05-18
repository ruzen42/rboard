using System;

namespace RBoard;

public class Logic
{
    private static bool _isWhiteTurn = true;

    public static bool IsLegalMove(char[,] board, int fromRow, int fromCol, int toRow, int toCol, char targetPiece)
    {
        if (fromRow < 0 || fromRow > 7 || fromCol < 0 || fromCol > 7 ||
            toRow < 0 || toRow > 7 || toCol < 0 || toCol > 7)
        {
            return false;
        }

        char piece = board[fromRow, fromCol];
        
        if (piece == '0') return false;
        
        if ((_isWhiteTurn && !char.IsUpper(piece)) || (!_isWhiteTurn && char.IsUpper(piece)))
        {
            return false;
        }

        if (_isWhiteTurn)
        {
            if (targetPiece != '0' && char.IsUpper(piece) == char.IsUpper(targetPiece))
            {
                return false;
            }
        }
        else
        {
            if (targetPiece != '0' && char.IsLower(piece) == char.IsLower(targetPiece))
            {
                return false;
            }
        }

        bool isValidMove = char.ToLower(piece) switch
        {
            'p' => IsPawnMoveValid(board, fromRow, fromCol, toRow, toCol, piece),
            'r' => IsRookMoveValid(board, fromRow, fromCol, toRow, toCol),
            'n' => IsKnightMoveValid(fromRow, fromCol, toRow, toCol),
            'b' => IsBishopMoveValid(board, fromRow, fromCol, toRow, toCol),
            'q' => IsQueenMoveValid(board, fromRow, fromCol, toRow, toCol),
            'k' => IsKingMoveValid(fromRow, fromCol, toRow, toCol),
            _ => false
        };

        if (isValidMove)
        {
            _isWhiteTurn = !_isWhiteTurn;
        }

        return isValidMove;
    }

    private static bool IsPawnMoveValid(char[,] board, int fromRow, int fromCol, int toRow, int toCol, char piece)
    {
        var isWhitePiece = char.IsUpper(piece);
        var direction = isWhitePiece ? -1 : 1;
        var startRow = isWhitePiece ? 6 : 1;
    
        if (fromCol == toCol && toRow == fromRow + direction && board[toRow, toCol] == '0')
        {
            return true;
        }

        const int increase = 2; 
    
        if (fromCol == toCol && fromRow == startRow && 
            toRow == fromRow + increase * direction && 
            board[fromRow + direction, fromCol] == '0' && 
            board[toRow, toCol] == '0')
        {
            return true;
        }

        return Math.Abs(fromCol - toCol) == 1 && toRow == fromRow + direction &&
               board[toRow, toCol] != '0';
    }

    private static bool IsRookMoveValid(char[,] board, int fromRow, int fromCol, int toRow, int toCol)
    {
        if (fromRow != toRow && fromCol != toCol) return false;

        int rowStep = fromRow == toRow ? 0 : (toRow > fromRow ? 1 : -1);
        int colStep = fromCol == toCol ? 0 : (toCol > fromCol ? 1 : -1);

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        while (currentRow != toRow || currentCol != toCol)
        {
            if (board[currentRow, currentCol] != '0') return false;
            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }

    private static bool IsKnightMoveValid(int fromRow, int fromCol, int toRow, int toCol)
    {
        int rowDiff = Math.Abs(fromRow - toRow);
        int colDiff = Math.Abs(fromCol - toCol);
        return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
    }

    private static bool IsBishopMoveValid(char[,] board, int fromRow, int fromCol, int toRow, int toCol)
    {
        if (Math.Abs(fromRow - toRow) != Math.Abs(fromCol - toCol)) return false;

        int rowStep = toRow > fromRow ? 1 : -1;
        int colStep = toCol > fromCol ? 1 : -1;

        int currentRow = fromRow + rowStep;
        int currentCol = fromCol + colStep;

        while (currentRow != toRow && currentCol != toCol)
        {
            if (board[currentRow, currentCol] != '0') return false;
            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }

    private static bool IsQueenMoveValid(char[,] board, int fromRow, int fromCol, int toRow, int toCol)
    {
        return IsRookMoveValid(board, fromRow, fromCol, toRow, toCol) || 
               IsBishopMoveValid(board, fromRow, fromCol, toRow, toCol);
    }

    private static bool IsKingMoveValid(int fromRow, int fromCol, int toRow, int toCol)
    {
        int rowDiff = Math.Abs(fromRow - toRow);
        int colDiff = Math.Abs(fromCol - toCol);
        return rowDiff <= 1 && colDiff <= 1;
    }

    public static void ResetTurn()
    {
        _isWhiteTurn = true;
    }
}