using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _camera;
    void Start() {
        GenerateBoard();
    }
    void GenerateBoard() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8 ; j++) {
                var tile = Instantiate(_tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.name = "Tile " + i + " " + j;

                var isOffSet = i % 2 != j % 2;
                tile.SetColor(isOffSet);
            }
        }
        _camera.transform.position = new Vector3(3.5f, 3.5f, -10);
    }
}
