using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndgameSelectManager : MonoBehaviour {
    public Button startGameButton;

    void Start() {
        startGameButton.onClick.AddListener(StartGame);
    }

    void StartGame() {
        SceneManager.LoadScene("EndGameScene");
    }
}
