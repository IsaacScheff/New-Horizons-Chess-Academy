using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessManager : MonoBehaviour {
    public static ChessManager Instance { get; private set; }
    [SerializeField] private string _currentFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    
    [SerializeField] private GameObject _textBoard;
    [SerializeField] private TMP_InputField _moveInput;
    private TextMeshProUGUI _boardText;
    void Awake() {
        Instance = this;

        _boardText = _textBoard.GetComponent<TextMeshProUGUI>();
        _boardText.text = Converters.BoardToString(Converters.FenToBoard(_currentFEN.Split(' ')[0]));
        Debug.Log("_boardText: " + _boardText.text);
    }
    void Update() {
        // Check if the input field is currently selected and the Enter key is pressed
        if (_moveInput.text != "" && Input.GetKeyDown(KeyCode.Return)) {
            OnMoveTextSubmit();
        }
    }
    public void OnMoveTextSubmit() {
        string enteredText = _moveInput.text;
        Debug.Log("Submitted text: " + enteredText);

        _moveInput.text = "";
        
        MoveValidator.ValidateMove(enteredText, _currentFEN);
    }
  
}
