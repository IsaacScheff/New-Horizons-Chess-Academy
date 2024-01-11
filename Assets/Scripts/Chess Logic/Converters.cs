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

}
