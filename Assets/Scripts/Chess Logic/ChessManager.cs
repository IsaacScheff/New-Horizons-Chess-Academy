using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessManager : MonoBehaviour {
    public static ChessManager Instance { get; private set; }
    [SerializeField] private string _currentFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    [SerializeField] private GameObject _textBoard;
    private TextMeshProUGUI _boardText;
    void Awake() {
        Instance = this;

        _boardText = _textBoard.GetComponent<TextMeshProUGUI>();
        _boardText.text = Converters.BoardToString(Converters.FenToBoard(_currentFEN));
        Debug.Log("_boardText: " + _boardText.text);
    }
  
}
