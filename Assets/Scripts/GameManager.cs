using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public bool SinglePlayerGame = false; // Whether the game is single player or not, for testing crrently always not
    void Awake() {
        Instance = this;
    }
}
