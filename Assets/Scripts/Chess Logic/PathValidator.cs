using System;
using UnityEngine;

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

