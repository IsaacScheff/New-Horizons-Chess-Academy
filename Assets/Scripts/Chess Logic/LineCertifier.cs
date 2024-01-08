using System;
using UnityEngine;

public class LineCertifier : MonoBehaviour {
    public static bool StraightVerify(string startSquare, string endSquare) {
        if (startSquare[0] == endSquare[0] || startSquare[1] == endSquare[1])
            return true;
        else
            return false;
    }
    public bool DiagonalVerify(string startSquare, string endSquare) {
        char startFile = startSquare[0];
        char startRank = startSquare[1];
        char endFile = endSquare[0];
        char endRank = endSquare[1];

        return Math.Abs(startFile - endFile) == Math.Abs(startRank - endRank);
    }
}
/*
    These functions assume that the startSquare and endSquare strings are formatted correctly (e.g., "a2", "b4"). 
    The calculations are based on the ASCII values of the characters, 
    which works well for this purpose since the letters and numbers in chess notation are consecutive in the ASCII table. 
    For instance, the difference between 'a' and 'b' is 1, which aligns with their positions on the chessboard
*/
