using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    public static BoardManager Instance { get; private set; }
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;
    private Dictionary<string, Tile> _board;
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
}
