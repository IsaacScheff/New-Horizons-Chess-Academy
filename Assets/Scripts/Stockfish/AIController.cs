using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO; // Required for file logging

public class AIController : MonoBehaviour {
    public static AIController Instance { get; private set; }
    private Process stockfishProcess;

    void Start() {
        Instance = this;
        StartStockfish();
        //TestCommunicationWithStockfish();
    }

    void StartStockfish() {
        try {
            UnityEngine.Debug.Log("Starting Stockfish process...");
            LogToFile("Starting Stockfish process...");

            stockfishProcess = new Process();
            stockfishProcess.StartInfo.FileName = Application.dataPath + "/MacOS/stockfish";
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
    public string GetBestMove(string fen) {
        try {
            stockfishProcess.StandardInput.WriteLine("uci");
            stockfishProcess.StandardInput.WriteLine("isready");
            stockfishProcess.StandardInput.WriteLine("position fen " + fen);
            stockfishProcess.StandardInput.WriteLine("go depth 10");

            while (!stockfishProcess.StandardOutput.EndOfStream) {
                string outputLine = stockfishProcess.StandardOutput.ReadLine();
                UnityEngine.Debug.Log(outputLine);
        UnityEngine.Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
                LogToFile(outputLine);
                if (outputLine.StartsWith("bestmove")) {
                    // Extract and process the best move
                    return outputLine.Split(' ')[1];
                }
            }
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogError("Error during Stockfish communication: " + ex.Message);
            LogToFile("Error during Stockfish communication: " + ex.Message);
        }
        return "";
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


