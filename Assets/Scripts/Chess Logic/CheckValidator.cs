using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CheckValidator : MonoBehaviour {
    public static bool IsCheckAfterMove(string move, string fen) {
        string[] fenParts = fen.Split(' ');
        string board = fenParts[0];
        string color = fenParts[1];

        // For castling, verify that the king is not in check currently and not moving through a square that would be in check
        switch (move) {
            case "e1c1":
                if (IsCheckAfterMove("e1e1", fen) || IsCheckAfterMove("e1d1", fen))
                    return true;
                break;
            case "e1g1":
                if (IsCheckAfterMove("e1e1", fen) || IsCheckAfterMove("e1f1", fen))
                    return true;
                break;
            case "e8c8":
                if (IsCheckAfterMove("e8e8", fen) || IsCheckAfterMove("e8d8", fen))
                    return true;
                break;
            case "e8g8":
                if (IsCheckAfterMove("e8e8", fen) || IsCheckAfterMove("e8f8", fen))
                    return true;
                break;
            default:
                break;
        }

        // Make a copy of the board
        char[][] newBoard = Converters.FenToBoard(MoveExecutor.ExecuteMove(move, fen).Split(' ')[0]);

        int startX = move[0] - 'a';
        int startY = 8 - int.Parse(move[1].ToString());
        int endX = move[2] - 'a';
        int endY = 8 - int.Parse(move[3].ToString());
        char piece = Converters.FenToBoard(board)[startY][startX];

        newBoard[endY][endX] = piece;

        if (startX != endX || startY != endY)
            newBoard[startY][startX] = '-';

        // Find the king's position
        int[] kingPosition = null;
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                if (color == "w" && newBoard[y][x] == 'K') {
                    kingPosition = new int[] { y, x };
                    break;
                } else if (color == "b" && newBoard[y][x] == 'k') {
                    kingPosition = new int[] { y, x };
                    break;
                }
            }
            if (kingPosition != null) break;
        }

        string oppositeColor = color == "w" ? "b" : "w";
        // Check for attacking moves from opponent's pawns
        int pawnDirection = oppositeColor == "w" ? 1 : -1;
        if ((kingPosition[0] + pawnDirection >= 0 && kingPosition[0] + pawnDirection < 8) && 
            ((kingPosition[1] + 1 < 8 && newBoard[kingPosition[0] + pawnDirection][kingPosition[1] + 1] == (oppositeColor == "w" ? 'P' : 'p')) || 
            (kingPosition[1] - 1 >= 0 && newBoard[kingPosition[0] + pawnDirection][kingPosition[1] - 1] == (oppositeColor == "w" ? 'P' : 'p')))) {
            return true;
        }

        // Check for the 8 different squares a knight could attack the king
        char oppKnight = oppositeColor == "w" ? 'N' : 'n';
        if (MoveValidator.IsSquareOnBoard(kingPosition[0] - 1, kingPosition[1] - 2) && newBoard[kingPosition[0] - 1][kingPosition[1] - 2] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] + 1, kingPosition[1] - 2) && newBoard[kingPosition[0] + 1][kingPosition[1] - 2] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] - 2, kingPosition[1] - 1) && newBoard[kingPosition[0] - 2][kingPosition[1] - 1] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] + 2, kingPosition[1] - 1) && newBoard[kingPosition[0] + 2][kingPosition[1] - 1] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] - 1, kingPosition[1] + 2) && newBoard[kingPosition[0] - 1][kingPosition[1] + 2] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] + 1, kingPosition[1] + 2) && newBoard[kingPosition[0] + 1][kingPosition[1] + 2] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] - 2, kingPosition[1] + 1) && newBoard[kingPosition[0] - 2][kingPosition[1] + 1] == oppKnight ||
            MoveValidator.IsSquareOnBoard(kingPosition[0] + 2, kingPosition[1] + 1) && newBoard[kingPosition[0] + 2][kingPosition[1] + 1] == oppKnight) {
            return true;
        }

        // Check for threats from bishops
        List<int[]> bishopPositions = new List<int[]>();
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                if ((color == "w" && newBoard[y][x] == 'b') || (color == "b" && newBoard[y][x] == 'B')) {
                    bishopPositions.Add(new int[] { y, x });
                }
            }
        }

        foreach (int[] bishop in bishopPositions) {
            if (LineCertifier.DiagonalVerify(bishop, kingPosition) && PathValidator.IsDiagonalClear(bishop, kingPosition, newBoard)) {
                return true;
            }
        }

        // Check for threats from rooks
        List<int[]> rookPositions = new List<int[]>();
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                if ((color == "w" && newBoard[y][x] == 'r') || (color == "b" && newBoard[y][x] == 'R')) {
                    rookPositions.Add(new int[] { y, x });
                }
            }
        }

        foreach (int[] rook in rookPositions) {
            if (LineCertifier.StraightVerify(rook, kingPosition) && PathValidator.IsPathClear(rook, kingPosition, newBoard)) {
                return true;
            }
        }

        // Check for threats from queens
        List<int[]> queenPositions = new List<int[]>();
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                if ((color == "w" && newBoard[y][x] == 'q') || (color == "b" && newBoard[y][x] == 'Q')) {
                    queenPositions.Add(new int[] { y, x });
                }
            }
        }

        foreach (int[] queen in queenPositions) {
            if ((LineCertifier.StraightVerify(queen, kingPosition) && PathValidator.IsPathClear(queen, kingPosition, newBoard)) || 
                (LineCertifier.DiagonalVerify(queen, kingPosition) && PathValidator.IsDiagonalClear(queen, kingPosition, newBoard))) {
                return true;
            }
        }

        // Check for threat from the opposing king
        int[] oppKing = null;
        for (int y = 0; y < 8; y++) {
            for (int x = 0; x < 8; x++) {
                if ((color == "w" && newBoard[y][x] == 'k') || (color == "b" && newBoard[y][x] == 'K')) {
                    oppKing = new int[] { y, x };
                    break;
                }
            }
            if (oppKing != null) break;
        }

        if (Math.Abs(kingPosition[0] - oppKing[0]) < 2 && Math.Abs(kingPosition[1] - oppKing[1]) < 2) {
            return true;
        }

        return false; // King is not under attack
    }
}
