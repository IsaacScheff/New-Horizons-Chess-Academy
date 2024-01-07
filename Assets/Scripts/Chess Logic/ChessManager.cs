using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour {
    public static ChessManager Instance { get; private set; }
    [SerializeField] private string _currentFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    [SerializeField] private GameObject _textBoard;
    void Awake() {
        Instance = this;
    }
  
}
