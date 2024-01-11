using System;
using System.Linq;
using UnityEngine;

public class MoveValidator : MonoBehaviour {
    public static bool ValidateMove(string move, string fen)
        {
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
            switch (char.ToLower(piece))
            {
                case 'p':
                    // ... (pawn logic as per JavaScript code, adapted to C#)
                    Debug.Log("pawn move check");
                    break;
                case 'r':
                    // ... (rook logic)
                    Debug.Log("rook move check");
                    break;
                case 'n':
                    // ... (knight logic)
                    Debug.Log("knight move check");
                    break;
                case 'b':
                    // ... (bishop logic)
                    Debug.Log("bishop move check");
                    break;
                case 'q':
                    // ... (queen logic)
                    Debug.Log("queen move check");
                    break;
                case 'k':
                    // ... (king logic, including castling)
                    Debug.Log("king move check");
                    break;
                default:
                    Debug.Log("Invalid piece");
                    return false; // Invalid piece
            }
            
            // if (!TargetCheck(endSquare, playerTurn, pieces))
            //     return false;

            // if (IsCheckAfterMove(move, fen))
            //     return false;

            // All checks passed, move is valid
            return true;
        }

}
