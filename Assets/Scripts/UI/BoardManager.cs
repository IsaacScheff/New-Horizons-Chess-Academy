using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public static BoardManager Instance { get; private set; }
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;
    private Dictionary<string, Tile> _board; 
    private string _activeTile;
    void Start() {
        Instance = this;
        GenerateBoard();
        UpdateBoardFromFen(ChessManager.Instance.CurrentFEN.Split(' ')[0]);
    }
    void GenerateBoard() {
        _board = new Dictionary<string, Tile>();
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8 ; j++) {
                var tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.name = $"{Convert.ToChar('a' + i)}{j + 1}";

                var isOffSet = i % 2 != j % 2;
                tile.SetColor(isOffSet);

                _board.Add(tile.name, tile);
            }
        }
        _camera.transform.position = new Vector3(3.5f, 3.5f, -10);
    }

    public Tile GetTile(string tileName) {
        return _board[tileName];
    }
    public void UpdateBoardFromFen(string fen) {
        char[][] boardArray = Converters.FenToBoard(fen);

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                char fenChar = boardArray[i][j];
                Tile.PieceType pieceType = Converters.FenCharToPieceType(fenChar);

                string tileName = $"{Convert.ToChar('a' + j)}{8 - i}";
                if (_board.ContainsKey(tileName)) {
                    _board[tileName].SetPiece(pieceType);
                }
            }
        }
    }

    public void TileClicked(string tileName) {
        Debug.Log("Clicked on tile: " + tileName);
        if((ChessManager.Instance.IsPlayerWhite && ChessManager.Instance.CurrentFEN.Split(' ')[1] != "w") ||
            (!ChessManager.Instance.IsPlayerWhite && ChessManager.Instance.CurrentFEN.Split(' ')[1] != "b")
        ) {
            Debug.Log("Not your turn");
            return;
        }
        //check here for tile having possible move set to active, if so execute move
        if(_board[tileName].IsPossibleMove) {
            string move = _activeTile + tileName;
            ChessManager.Instance.OnMoveUISubmit(move);
            ClearPossibleMoves();
            return;
        } else if (_activeTile != null) {
            ClearPossibleMoves();
        } else if(ChessManager.Instance.IsPlayerWhite == GetTile(tileName).IsPieceWhite()) {
            Debug.Log($"Piece on tile {GetTile(tileName).CurrentPiece}");
            ShowPossibleMoves(tileName, ChessManager.Instance.CurrentFEN); 
            _activeTile = tileName;
        } else {
            Debug.Log("Not your piece");
        }
    }

    private void ShowPossibleMoves(string startTile, string fen) {
        foreach (KeyValuePair<string, Tile> tileEntry in _board) {
            string targetTileName = tileEntry.Key; // Name of the target tile
            string move = startTile + targetTileName; // Concatenate to form a move string

            if (MoveValidator.ValidateMove(move, fen)) {
                // This is a valid move - you can do something with this information
                Debug.Log("Valid move: " + move);
                tileEntry.Value.HighlightPossibleMove();
            }
        }
    }
    private void ClearPossibleMoves() {
        foreach (KeyValuePair<string, Tile> tileEntry in _board) {
            tileEntry.Value.ClearPossibleMove();
        }
        _activeTile = null;
    }
}
