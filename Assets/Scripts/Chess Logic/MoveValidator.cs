using System;
using System.Linq;
using UnityEngine;

public class MoveValidator : MonoBehaviour {
    public static bool ValidateMove(string move, string fen) {
        //should give variables proper typing
        var fenParts = fen.Split(' ');
        var boardState = fenParts[0];
        var playerTurn = fenParts[1];
        var castlingRights = fenParts[2];
        var enPassantSquare = fenParts[3];

        var pieces = Converters.FenToBoard(boardState);
        
        var startSquare = Converters.SquareToArray(move.Substring(0, 2));
        var endSquare = Converters.SquareToArray(move.Substring(2, 2));

        if (startSquare.SequenceEqual(endSquare))
            return false;
        
        var piece = pieces[startSquare[0]][startSquare[1]];
        
        // Verify that piece belongs to the correct player
        if (piece == (playerTurn == "b" ? char.ToUpper(piece) : char.ToLower(piece)))
            return false;
        
        // Check if the move is legal for the specific piece type
        switch (char.ToLower(piece)) {
            case 'p':
                Debug.Log("pawn move");
                int startFile = startSquare[1];
                int startRank = startSquare[0];
                int endFile = endSquare[1];
                int endRank = endSquare[0];
                char target = pieces[endRank][endFile];

                if (endRank == (piece == 'P' ? 0 : 7) && (move.Length != 5 || !"bnrq".Contains(move[4].ToString().ToLower()))) {
                    /* checking for promotino moves that a piece to promote to is provided 
                        and that the provided piece is a valid choice of Bishop, Knight, Rook, or Queen */
                    return false;
                }
                
                int direction = (piece == 'P') ? -1 : 1;
                // Pawn moves forward one square or two if on starting square
                if (startFile == endFile && target == '-' && 
                    (endRank - startRank == direction || 
                    (endRank - startRank == 2 * direction && 
                    (startRank == 1 || startRank == 6) && pieces[startRank + direction][startFile] == '-'))) {
                        break;
                }
                // Pawn takes diagonally
                if (endRank - startRank == direction && Math.Abs(endFile - startFile) == 1) {
                    if (enPassantSquare != "-" && 
                        enPassantSquare[0] == endFile + 'a' && 
                        enPassantSquare[1] == 8 - endRank + '0' &&
                        target == pieces[8 - (enPassantSquare[1] - '0')][enPassantSquare[0] - 'a']
                        ) { 
                            break;
                    } else if (target != '-')
                        break;
                }
                return false;
            case 'r':
                if (!LineCertifier.StraightVerify(startSquare, endSquare))
                    return false;
                if (!PathValidator.IsPathClear(startSquare, endSquare, pieces))
                    return false;
                break;
            case 'n':
                int x1 = startSquare[0];
                int y1 = startSquare[1];
                int x2 = endSquare[0];
                int y2 = endSquare[1];

                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);

                if (!(dx == 1 && dy == 2) && !(dx == 2 && dy == 1))
                    return false;
                break;
            case 'b':
                if (!LineCertifier.DiagonalVerify(startSquare, endSquare))
                    return false;
                if (!PathValidator.IsDiagonalClear(startSquare, endSquare, pieces))
                    return false;
                break;
            case 'q':
                if (LineCertifier.StraightVerify(startSquare, endSquare)) {
                    if (!PathValidator.IsPathClear(startSquare, endSquare, pieces))
                        return false;
                } else if (LineCertifier.DiagonalVerify(startSquare, endSquare)) {
                    if (!PathValidator.IsDiagonalClear(startSquare, endSquare, pieces))
                        return false;
                } else {
                    return false; // Neither straight nor diagonal, so invalid move for a queen
                }
                break;
            case 'k':
                // Check for castling
                switch (move) {
                    case "e1c1":
                        if (castlingRights.Contains("K") && PathValidator.IsPathClear(startSquare, endSquare, pieces) && !CheckValidator.IsCheckAfterMove(move, fen))
                            return true;
                        else
                            return false;
                    case "e1g1":
                        if (castlingRights.Contains("Q") && PathValidator.IsPathClear(startSquare, endSquare, pieces) && !CheckValidator.IsCheckAfterMove(move, fen))
                            return true;
                        else
                            return false;
                    case "e8c8":
                        if (castlingRights.Contains("k") && PathValidator.IsPathClear(startSquare, endSquare, pieces) && !CheckValidator.IsCheckAfterMove(move, fen))
                            return true;
                        else
                            return false;
                    case "e8g8":
                        if (castlingRights.Contains("q") && PathValidator.IsPathClear(startSquare, endSquare, pieces) && !CheckValidator.IsCheckAfterMove(move, fen))
                            return true;
                        else
                            return false;
                    default:
                        break;
                }
                if (Math.Abs(startSquare[0] - endSquare[0]) > 1 || Math.Abs(startSquare[1] - endSquare[1]) > 1)
                    return false;
                break;
        }
        if (!TargetCheck(endSquare, playerTurn, pieces))
            return false;

        if (CheckValidator.IsCheckAfterMove(move, fen))
            return false;

        // All checks passed, move is valid
        return true;
    }
    private static bool TargetCheck(int[] targetSquare, string color, char[][] pieces) {
        int targetRank = targetSquare[0];
        int targetFile = targetSquare[1];
        char targetPiece = pieces[targetRank][targetFile];

        if (color == "b") {
            if ("k,n,r,p,q,b".Contains(targetPiece)) {
                return false;
            }
        }
        else {
            if ("K,N,R,P,Q,B".Contains(targetPiece)) {
                return false;
            }
        }
        return true;
    }
    public static bool IsSquareOnBoard(int x, int y) {
        return x >= 0 && x <= 7 && y >= 0 && y <= 7;
    }
}
