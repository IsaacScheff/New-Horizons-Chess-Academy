using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Converters : MonoBehaviour {
    public static char[][] FenToBoard(string fen) {
        string[] rows = fen.Split('/');
        char[][] board = new char[rows.Length][];

        for (int i = 0; i < rows.Length; i++) {
            List<char> row = new List<char>();
            int j = 0;

            while (j < rows[i].Length) {
                if (char.IsDigit(rows[i][j])) {
                    int numBlanks = int.Parse(rows[i][j].ToString());
                    for (int k = 0; k < numBlanks; k++) {
                        row.Add('-');
                    }
                    j++;
                }
                else {
                    row.Add(rows[i][j]);
                    j++;
                }
            }
            board[i] = row.ToArray();
        }
        return board;
    }
    public static string BoardToFen(char[][] board) {
        string fen = "";
        for (int i = 0; i < board.Length; i++) {
            char[] row = board[i];
            int numBlanks = 0;
            for (int j = 0; j < row.Length; j++) {
                char piece = row[j];
                if (piece == '-') {
                    numBlanks++;
                } else {
                    if (numBlanks > 0) {
                        fen += numBlanks.ToString();
                        numBlanks = 0;
                    }
                    fen += piece;
                }
            }
            if (numBlanks > 0) {
                fen += numBlanks.ToString();
            }
            if (i < board.Length - 1) {
                fen += '/';
            }
        }
        return fen;
    }

    public static string BoardToString(char[][] board) {
        StringBuilder sb = new StringBuilder();

        foreach (char[] row in board) {
            sb.Append("['");
            for (int i = 0; i < row.Length; i++) {
                sb.Append(row[i]);
                if (i < row.Length - 1) {
                    sb.Append("', '");
                }
            }
            sb.AppendLine("']");
        }

        return sb.ToString();
    }
    public static int[] SquareToArray(string square) {
        int rank = '8' - square[1];
        int file = square[0] - 'a';
        return new int[] { rank, file };
    }
    public static Tile.PieceType FenCharToPieceType(char fenChar) {
        switch (fenChar) {
            case 'p': return Tile.PieceType.PawnBlack;
            case 'r': return Tile.PieceType.RookBlack;
            case 'n': return Tile.PieceType.KnightBlack;
            case 'b': return Tile.PieceType.BishopBlack;
            case 'q': return Tile.PieceType.QueenBlack;
            case 'k': return Tile.PieceType.KingBlack;
            case 'P': return Tile.PieceType.Pawn;
            case 'R': return Tile.PieceType.Rook;
            case 'N': return Tile.PieceType.Knight;
            case 'B': return Tile.PieceType.Bishop;
            case 'Q': return Tile.PieceType.Queen;
            case 'K': return Tile.PieceType.King;
            default:  return Tile.PieceType.None;
        }
    }
}
