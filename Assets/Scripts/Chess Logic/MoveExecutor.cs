using UnityEngine;

public class MoveExecutor : MonoBehaviour {
    public static string ExecuteMove(string move, string fen) {
        int startFile = move[0] - 'a';
        int startRank = 8 - int.Parse(move[1].ToString());
        int endFile = move[2] - 'a';
        int endRank = 8 - int.Parse(move[3].ToString());

        var fenParts = fen.Split(' ');
        var boardState = fenParts[0];
        var playerTurn = fenParts[1];
        var castlingRights = fenParts[2];
        var enPassantSquare = fenParts[3];
        int halfMoveClock = int.Parse(fenParts[4]);
        int fullMoveNumber = int.Parse(fenParts[5]);

        var board = Converters.FenToBoard(boardState);

        halfMoveClock++;

        var piece = board[startRank][startFile];
        if (piece != 'p' && piece != 'P')
            enPassantSquare = "-";

        if (piece == 'p' || piece == 'P')
            halfMoveClock = 0;
        else if (board[endRank][endFile] != '-')
            halfMoveClock = 0;

        // Remove the piece from its starting position
        board[startRank][startFile] = '-';

        // Check for castling
        switch (move) {
            case "e1c1":
                board[7][2] = 'K';
                board[7][3] = 'R';
                board[7][0] = '-';
                break;
            case "e1g1":
                board[7][6] = 'K';
                board[7][5] = 'R';
                board[7][7] = '-';
                break;
            case "e8c8":
                board[0][2] = 'k';
                board[0][3] = 'r';
                board[0][0] = '-';
                break;
            case "e8g8":
                board[0][6] = 'k';
                board[0][5] = 'r';
                board[0][7] = '-';
                break;
            default: // Every move that's not castling move the piece to its new position
                board[endRank][endFile] = piece;
                break;
        }

        if (playerTurn == "b") {
            fullMoveNumber++;
            playerTurn = "w";
        } else {
            playerTurn = "b";
        }

        // Special moves; castling, en passant, promotion
        switch (piece) { 
            case 'P':
                if (endRank == 0) {
                    board[endRank][endFile] = move.Length > 4 ? move[4].ToString().ToUpper()[0] : 'Q'; // Default to Queen if no promotion piece is specified
                } else if (enPassantSquare == move.Substring(move.Length - 2)) {
                    board[endRank + 1][endFile] = '-';
                }
                if (startRank == 6 && endRank == 4) {
                    enPassantSquare = ((char)(97 + startFile)).ToString() + "3";
                } else {
                    enPassantSquare = "-";
                }
                break;
            case 'p':
                if (endRank == 7) {
                    board[endRank][endFile] = move.Length > 4 ? move[4].ToString().ToLower()[0] : 'q'; // Default to queen if no promotion piece is specified
                } else if (enPassantSquare == move.Substring(move.Length - 2)) {
                    board[endRank - 1][endFile] = '-';
                }
                if (startRank == 1 && endRank == 3) {
                    enPassantSquare = ((char)(97 + startFile)).ToString() + "6";
                } else {
                    enPassantSquare = "-";
                }
                break;
            case 'K':
                if (castlingRights.Contains('K') || castlingRights.Contains('Q')) {
                    castlingRights = castlingRights.Replace("K", "").Replace("Q", ""); 
                }
                break;
            case 'k':
                if (castlingRights.Contains('k') || castlingRights.Contains('q')) {
                    castlingRights = castlingRights.Replace("k", "").Replace("q", ""); 
                }
                break;
            case 'R':
                if (startRank == 7) {
                    if (startFile == 0 && castlingRights.Contains('Q'))
                        castlingRights = castlingRights.Replace("Q", ""); 
                    else if (startFile == 7 && castlingRights.Contains('K'))
                        castlingRights = castlingRights.Replace("K", "");
                } 
                break;
            case 'r':
                if (startRank == 0) {
                    if (startFile == 0 && castlingRights.Contains('q'))
                        castlingRights = castlingRights.Replace("q", ""); 
                    else if (startFile == 7 && castlingRights.Contains('k'))
                        castlingRights = castlingRights.Replace("k", "");
                } 
                break;
            default:
                break;
        }

        boardState = Converters.BoardToFen(board);

        return string.Join(" ", boardState, playerTurn, castlingRights, enPassantSquare, halfMoveClock, fullMoveNumber);
    }
}
