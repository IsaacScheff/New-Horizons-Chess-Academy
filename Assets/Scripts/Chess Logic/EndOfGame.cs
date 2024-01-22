using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EndOfGame : MonoBehaviour {
    public static string IsGameOver(string fen, List<string> prevBoards) {
        var pieces = GetPieces(fen);
        var soloMates = new List<char> { 'Q', 'R', 'P' };

        // Check for insufficient material
        if (pieces.BlackPieces.Count < 3 && pieces.WhitePieces.Count < 3) {
            if (!pieces.WhitePieces.Any(piece => soloMates.Contains(piece))) {
                if (!pieces.BlackPieces.Any(piece => soloMates.Contains(char.ToUpper(piece)))) {
                    Debug.Log("Insufficient mating material!");
                    return "draw";
                }
            }
        }

        if (int.Parse(fen.Split(' ')[4]) >= 100) {
            Debug.Log("Fifty Move rule");
            return "draw";
        }

        if (ThreefoldRepetition(prevBoards)) {
            Debug.Log("Threefold repetition.");
            return "draw";
        }

        if (!LegalMove(fen)) {
            // If in check, then checkmate, otherwise stalemate
            int[] kingPosition = null;
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++) {
                    var board = Converters.FenToBoard(fen.Split(' ')[0]);
                    if (fen.Split(' ')[1] == "w" && board[y][x] == 'K') {
                        kingPosition = new int[] { y, x };
                        break;
                    } else if (fen.Split(' ')[1] == "b" && board[y][x] == 'k') {
                        kingPosition = new int[] { y, x };
                        break;
                    }
                }
            }
            var kingSquare = ((char)(kingPosition[1] + 'a')).ToString() + (8 - kingPosition[0]).ToString();
            var move = kingSquare + kingSquare;
            if (CheckValidator.IsCheckAfterMove(move, fen)) {
                Debug.Log("Checkmate");
                return fen.Split(' ')[1] == "w" ? "black" : "white";
            }
            Debug.Log("Stalemate");
            return "draw";
        }
        return "false";
    }
    public static (List<char> WhitePieces, List<char> BlackPieces) GetPieces(string fen) {
        List<char> blackPieces = new List<char>();
        List<char> whitePieces = new List<char>();
        string board = fen.Split(' ')[0];

        foreach (char c in board) {
            if (char.IsLetter(c)) {
                if (char.IsUpper(c)) {
                    whitePieces.Add(c);
                } else {
                    blackPieces.Add(c);
                }
            }
        }

        return (whitePieces, blackPieces);
    }
    public static bool ThreefoldRepetition(List<string> prevBoards) {
        Dictionary<string, int> occurrences = new Dictionary<string, int>();

        foreach (var board in prevBoards) {
            if (occurrences.ContainsKey(board)) {
                occurrences[board]++;
                if (occurrences[board] == 3) {
                    return true;
                }
            } else {
                occurrences[board] = 1;
            }
        }

        return false;
    }
    public static bool LegalMove(string fen) {
        var parts = fen.Split(' ');
        var boardState = Converters.FenToBoard(parts[0]);
        var color = parts[1];

        for (int i = 0; i < boardState.GetLength(0); i++) {
            for (int j = 0; j < boardState[i].Length; j++) {
                char piece = boardState[i][j];
                if ((color == "w" && char.IsUpper(piece)) || (color == "b" && char.IsLower(piece))) {
                    switch (piece.ToString().ToLower()) {
                        case "k":
                            if (PossibleMoves.ValidKingMove(new int[] { i, j }, fen))
                                return true;
                            break;
                        case "q":
                            if (PossibleMoves.ValidRookMove(new int[] { i, j }, fen) || PossibleMoves.ValidBishopMove(new int[] { i, j }, fen))
                                return true;
                            break;
                        case "r":
                            if (PossibleMoves.ValidRookMove(new int[] { i, j }, fen))
                                return true;
                            break;
                        case "n":
                            if (PossibleMoves.ValidKnightMove(new int[] { i, j }, fen))
                                return true;
                            break;
                        case "b":
                            if (PossibleMoves.ValidBishopMove(new int[] { i, j }, fen))
                                return true;
                            break;
                        case "p":
                            if (PossibleMoves.ValidPawnMove(new int[] { i, j }, fen))
                                return true;
                            break;
                    }
                }
            }
        }
        return false;
    }   
}
