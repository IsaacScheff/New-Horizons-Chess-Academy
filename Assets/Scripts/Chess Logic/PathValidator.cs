using System;
using UnityEngine;
/*
    Character to Index Conversion: Since the board array is 2D, characters from squares 
    (like 'a', 'b', '1', '2') are converted to array indices. 
    This involves subtracting 'a' from file characters and '8' from rank characters, 
    assuming a standard 8x8 chessboard with ranks 1-8 and files a-h
*/
public class PathValidator : MonoBehaviour {
    public static bool IsPathClear(int[] startSquare, int[] endSquare, char[][] pieces) {
        int startFile = startSquare[0];
        int startRank = startSquare[1];
        int endFile = endSquare[0];
        int endRank = endSquare[1];

        int fileDirection = Math.Sign(endFile - startFile);
        int rankDirection = Math.Sign(endRank - startRank);

        for (int file = startFile + fileDirection, rank = startRank + rankDirection; 
            file != endFile || rank != endRank; 
            file += fileDirection, rank += rankDirection) {
            // Skip the first square, which is the starting position
            //if (file == startFile && rank == startRank) continue;

            if (pieces[file][rank] != '-') {
                return false;
            }
        }

        return true;
    }
    public static bool IsDiagonalClear(int[] startSquare, int[] endSquare, char[][] pieces) {
        int startFile = startSquare[0];
        int startRank = startSquare[1];
        int endFile = endSquare[0];
        int endRank = endSquare[1];

        int rankDirection = startRank < endRank ? 1 : -1;
        int fileDirection = startFile < endFile ? 1 : -1;

        for (int rank = startRank + rankDirection, file = startFile + fileDirection; 
            rank != endRank; 
            rank += rankDirection, file += fileDirection) {
                if (pieces[file][rank] != '-') {
                    return false;
                }
        }
        return true;
    }
}

