using System.Diagnostics;
using UnityEngine;

public class StockfishManager : MonoBehaviour {
    private string stockfishPath;

    void Start() {
        stockfishPath = Application.dataPath + "/Stockfish/stockfish";
    }

}

