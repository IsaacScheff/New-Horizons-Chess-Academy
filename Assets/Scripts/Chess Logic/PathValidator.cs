using System;
using UnityEngine;
/*
    Character to Index Conversion: Since the board array is 2D, characters from squares 
    (like 'a', 'b', '1', '2') are converted to array indices. 
    This involves subtracting 'a' from file characters and '8' from rank characters, 
    assuming a standard 8x8 chessboard with ranks 1-8 and files a-h
*/
public class PathValidator : MonoBehaviour {
    public bool IsPathClear(string startSquare, string endSquare, char[][] pieces) {
        int startFile = startSquare[0] - 'a';
        int startRank = '8' - startSquare[1];
        int endFile = endSquare[0] - 'a';
        int endRank = '8' - endSquare[1];

        int fileDirection = Math.Sign(endFile - startFile);
        int rankDirection = Math.Sign(endRank - startRank);

        for (int file = startFile + fileDirection, rank = startRank + rankDirection; 
            file != endFile || rank != endRank; 
            file += fileDirection, rank += rankDirection) {
                if (pieces[rank][file] != '-') {
                    return false;
                }
        }
        return true;
    }
    public bool IsDiagonalClear(string startSquare, string endSquare, char[][] pieces) {
        int startFile = startSquare[0] - 'a';
        int startRank = '8' - startSquare[1];
        int endFile = endSquare[0] - 'a';
        int endRank = '8' - endSquare[1];

        int rankDirection = startRank < endRank ? 1 : -1;
        int fileDirection = startFile < endFile ? 1 : -1;

        for (int rank = startRank + rankDirection, file = startFile + fileDirection; 
            rank != endRank; 
            rank += rankDirection, file += fileDirection) {
                if (pieces[rank][file] != '-') {
                    return false;
                }
        }
        return true;
    }
}

