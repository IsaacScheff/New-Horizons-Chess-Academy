using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChessManager : MonoBehaviour {
    public static ChessManager Instance { get; private set; }
    //[SerializeField] private string _currentFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    [SerializeField] private string _currentFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w KQkq - 0 1";
    public List<string> previousBoards; 
    public string CurrentFEN { get { return _currentFEN; } }
    [SerializeField] private GameObject _textBoard;
    [SerializeField] private TMP_InputField _moveInput;
    [SerializeField] private bool _playerIsWhite;
    public bool IsPlayerWhite { get { return _playerIsWhite; } }
    private TextMeshProUGUI _boardText;
    void Awake() {
        Instance = this;
        
        _playerIsWhite = true; //for testing
        //_boardText = _textBoard.GetComponent<TextMeshProUGUI>();
        //_boardText.text = Converters.BoardToString(Converters.FenToBoard(_currentFEN.Split(' ')[0]));
        //Debug.Log("_boardText: " + _boardText.text);
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
        
        if(MoveValidator.ValidateMove(enteredText, _currentFEN)) {
            _currentFEN = MoveExecutor.ExecuteMove(enteredText, _currentFEN);
            //_boardText.text = Converters.BoardToString(Converters.FenToBoard(_currentFEN.Split(' ')[0])); 
            BoardManager.Instance.UpdateBoardFromFen(_currentFEN.Split(' ')[0]);
        } else {
            Debug.Log("Invalid move");
        }
    }
    //these functions will be largely the same, will refactor later
    public void OnMoveUISubmit(string move) {
        if(GameManager.Instance.SinglePlayerGame) {
            _playerIsWhite = !_playerIsWhite;
        }
        if(MoveValidator.ValidateMove(move, _currentFEN)) { //should already be validated
            previousBoards.Add(_currentFEN);
            _currentFEN = MoveExecutor.ExecuteMove(move, _currentFEN);
            BoardManager.Instance.UpdateBoardFromFen(_currentFEN.Split(' ')[0]);
            //check for end of game
            HandleEndOfGame();
            if(GameManager.Instance.SinglePlayerGame) {
                MakeAIMove();
                //check for end of game
                HandleEndOfGame();
            }
        } else {
            Debug.Log("Invalid move");
        }
    }

    public void MakeAIMove() {
        string AIMove = AIController.Instance.GetBestMove(_currentFEN);
        previousBoards.Add(_currentFEN);
        _currentFEN = MoveExecutor.ExecuteMove(AIMove, _currentFEN);
        BoardManager.Instance.UpdateBoardFromFen(_currentFEN.Split(' ')[0]);
        _playerIsWhite = !_playerIsWhite;
    }

    public void HandleEndOfGame() {
        Debug.Log(EndOfGame.IsGameOver(_currentFEN, previousBoards)); 
    }
}
