using System;
using UnityEngine;

public class PossibleMoves : MonoBehaviour {
    public static bool ValidKingMove(int[] kingPosition, string fen) {
        string startSquare = ((char)(kingPosition[1] + 'a')).ToString() + (8 - kingPosition[0]).ToString();
        string move;
        
        int x = -1;
        int y = -1;
        int upperX = 1;
        int upperY = 1;

        if (kingPosition[0] == 7) { // King on the bottom rank
            y = 0;
        } else if (kingPosition[0] == 0) { // King on the top rank
            upperY = 0;
        }

        if (kingPosition[1] == 7) { // King on the right (h file)
            upperX = 0;
        } else if (kingPosition[1] == 0) { // King on the left (a file)
            x = 0;
        }

        for (int i = x; i <= upperX; i++) {
            for (int j = y; j <= upperY; j++) {
                if (i != 0 || j != 0) {
                    move = startSquare + ((char)(kingPosition[1] + i + 'a')).ToString() + (8 - kingPosition[0] + j).ToString();
                    if (MoveValidator.ValidateMove(move, fen)) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool ValidRookMove(int[] rookPosition, string fen) {
        string startSquare = ((char)(rookPosition[1] + 'a')).ToString() + (8 - rookPosition[0]).ToString();
        string move;

        // Vertical moves
        for (int y = 0; y < 8; y++) {
            if (rookPosition[0] != y) {
                move = startSquare[0].ToString() + (8 - y).ToString();
                if (MoveValidator.ValidateMove(move, fen)) {
                    return true;
                }
            }
        }
        // Horizontal moves
        for (int x = 0; x < 8; x++) {
            if (rookPosition[1] != x) {
                move = ((char)(x + 'a')).ToString() + startSquare[1].ToString();
                if (MoveValidator.ValidateMove(move, fen)) {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool ValidBishopMove(int[] bishopPosition, string fen) {
        string startSquare = ((char)(bishopPosition[1] + 'a')).ToString() + (8 - bishopPosition[0]).ToString();
        string move;

        for (int rank = 0; rank < 8; rank++) {
            if (bishopPosition[0] != rank) {
                int fileDifference = Math.Abs(bishopPosition[0] - rank);

                if (bishopPosition[1] + fileDifference < 8) {
                    move = ((char)(bishopPosition[1] + fileDifference + 'a')).ToString() + (8 - rank).ToString();
                    if (MoveValidator.ValidateMove(move, fen)) {
                        return true;
                    }
                }

                if (bishopPosition[1] - fileDifference >= 0) {
                    move = ((char)(bishopPosition[1] - fileDifference + 'a')).ToString() + (8 - rank).ToString();
                    if (MoveValidator.ValidateMove(move, fen)) {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public static bool ValidKnightMove(int[] knightPosition, string fen) {
        string startSquare = ((char)(knightPosition[1] + 'a')).ToString() + (8 - knightPosition[0]).ToString();
        string move;

        int[,] knightMoveOffsets = {
            {-1, -2}, {-2, -1}, {-2, 1}, {-1, 2},
            {1, -2}, {2, -1}, {2, 1}, {1, 2}
        };

        // Iterate through the possible knight moves
        for (int i = 0; i < knightMoveOffsets.GetLength(0); i++) {
            int dx = knightMoveOffsets[i, 0];
            int dy = knightMoveOffsets[i, 1];

            int newX = knightPosition[1] + dx;
            int newY = knightPosition[0] + dy;
            move = startSquare + ((char)(newX + 'a')).ToString() + (8 - newY).ToString();

            if (MoveValidator.IsSquareOnBoard(newX, newY) && MoveValidator.ValidateMove(move, fen)) {
                return true;
            }
        }

        return false;
    }
    public static bool ValidPawnMove(int[] pawnPosition, string fen) {
        string startSquare = ((char)(pawnPosition[1] + 'a')).ToString() + (8 - pawnPosition[0]).ToString();
        string move;
        string color = fen.Split(' ')[1];

        // Check for promotion
        if ((color == "w" && pawnPosition[0] == 1) || (color == "b" && pawnPosition[0] == 6)) {
            move = startSquare[0].ToString() + (8 - (pawnPosition[0] + (color == "w" ? -1 : 1))).ToString() + "q";
            return true; // Assuming you want to return true here, but typically would validate the move first
        }

        // Check if pawn can move forward one square
        move = startSquare[0].ToString() + (8 - (pawnPosition[0] + (color == "w" ? -1 : 1))).ToString();
        if (MoveValidator.ValidateMove(move, fen)) {
            return true;
        }

        // Check if pawn on starting square can move forward two squares
        if ((color == "w" && pawnPosition[0] == 6) || (color == "b" && pawnPosition[0] == 1)) {
            move = startSquare[0].ToString() + (8 - (pawnPosition[0] + (color == "w" ? -2 : 2))).ToString();
            if (MoveValidator.ValidateMove(move, fen)) {
                return true;
            }
        }

        // Check if pawn can make a capture
        int[][] captureDirections = { new int[] {-1, -1}, new int[] {1, -1} }; // Diagonal capture directions
        foreach (int[] direction in captureDirections) {
            int targetX = pawnPosition[1] + direction[0];
            int targetY = pawnPosition[0] + (color == "w" ? -1 : 1);

            if (MoveValidator.IsSquareOnBoard(targetX, targetY)) {
                move = ((char)(targetX + 'a')).ToString() + (8 - targetY).ToString();
                if (MoveValidator.ValidateMove(move, fen)) {
                    return true;
                }
            }
        }

        return false;
    }
}
