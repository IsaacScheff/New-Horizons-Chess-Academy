using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameSelectManager : MonoBehaviour {
    public Button startGameButton;
    public GameData gameData;

    void Start() {
        startGameButton.onClick.AddListener(StartGame);
    }

    void StartGame() {
        gameData.currentFEN = "Q7/8/8/4k3/8/8/8/5K2 w - - 0 1";
        SceneManager.LoadScene("EndGameScene");
    }
}
