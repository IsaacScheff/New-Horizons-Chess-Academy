// using System.Diagnostics;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class StockfishTest : MonoBehaviour {
//     private Process stockfishProcess;
//     [SerializeField] private GameObject _testText;

//     void Start() {
//         StartStockfish();
//         TestCommunicationWithStockfish();
//     }

//     void StartStockfish() {
//         UnityEngine.Debug.Log("Start Stockfish function");
//         stockfishProcess = new Process();
//         stockfishProcess.StartInfo.FileName = Application.dataPath + "/Executables/stockfish"; 
//         stockfishProcess.StartInfo.UseShellExecute = false;
//         stockfishProcess.StartInfo.RedirectStandardInput = true;
//         stockfishProcess.StartInfo.RedirectStandardOutput = true;
//         stockfishProcess.Start();
//     }

//     void TestCommunicationWithStockfish() {
//         stockfishProcess.StandardInput.WriteLine("uci");
//         stockfishProcess.StandardInput.WriteLine("isready");
//         stockfishProcess.StandardInput.WriteLine("position startpos moves e2e4 e7e5");
//         stockfishProcess.StandardInput.WriteLine("go depth 10");

//         while (!stockfishProcess.StandardOutput.EndOfStream) {
//             string outputLine = stockfishProcess.StandardOutput.ReadLine();
//             UnityEngine.Debug.Log(outputLine);
//             _testText.GetComponentInChildren<TextMeshProUGUI>().text = outputLine;
//             if (outputLine.StartsWith("bestmove")) {
//                 // Extract and process the best move
//                 break;
//             }
//         }
//     }

//     void OnApplicationQuit() {
//         if (stockfishProcess != null) {
//             stockfishProcess.Close();
//         }
//     }
// }
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; // Required for file logging

public class StockfishTest : MonoBehaviour {
    private Process stockfishProcess;
    [SerializeField] private GameObject _testText;

    void Start() {
        StartStockfish();
        TestCommunicationWithStockfish();
    }

    void StartStockfish() {
        try {
            UnityEngine.Debug.Log("Starting Stockfish process...");
            LogToFile("Starting Stockfish process...");

            stockfishProcess = new Process();
            stockfishProcess.StartInfo.FileName = Application.dataPath + "/Executables/stockfish"; 
            stockfishProcess.StartInfo.UseShellExecute = false;
            stockfishProcess.StartInfo.RedirectStandardInput = true;
            stockfishProcess.StartInfo.RedirectStandardOutput = true;
            stockfishProcess.Start();

            UnityEngine.Debug.Log("Stockfish process started successfully.");
            LogToFile("Stockfish process started successfully.");
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogError("Failed to start Stockfish: " + ex.Message);
            LogToFile("Failed to start Stockfish: " + ex.Message);
        }
    }

    void TestCommunicationWithStockfish() {
        try {
            stockfishProcess.StandardInput.WriteLine("uci");
            stockfishProcess.StandardInput.WriteLine("isready");
            stockfishProcess.StandardInput.WriteLine("position startpos moves e2e4 e7e5");
            stockfishProcess.StandardInput.WriteLine("go depth 10");

            while (!stockfishProcess.StandardOutput.EndOfStream) {
                string outputLine = stockfishProcess.StandardOutput.ReadLine();
                UnityEngine.Debug.Log(outputLine);
        UnityEngine.Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
                LogToFile(outputLine);
                _testText.GetComponentInChildren<TextMeshProUGUI>().text = outputLine;
                if (outputLine.StartsWith("bestmove")) {
                    // Extract and process the best move
                    break;
                }
            }
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogError("Error during Stockfish communication: " + ex.Message);
            LogToFile("Error during Stockfish communication: " + ex.Message);
        }
    }

    void OnApplicationQuit() {
        if (stockfishProcess != null) {
            stockfishProcess.Close();
        }
    }

    // Logging method to write messages to a file
    void LogToFile(string message) {
        string filePath = Application.persistentDataPath + "/game_log.txt";
        using (StreamWriter sw = File.AppendText(filePath)) {
            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + message);
        }
    }
}


