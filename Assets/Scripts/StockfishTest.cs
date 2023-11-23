using System.Diagnostics;
using UnityEngine;

public class StockfishTest : MonoBehaviour {
    private Process stockfishProcess;

    void Start() {
        StartStockfish();
        TestCommunicationWithStockfish();
    }

    void StartStockfish() {
        UnityEngine.Debug.Log("Start Stockfish function");
        stockfishProcess = new Process();
        stockfishProcess.StartInfo.FileName = Application.dataPath + "/Executables/stockfish"; // Replace with actual path
        stockfishProcess.StartInfo.UseShellExecute = false;
        stockfishProcess.StartInfo.RedirectStandardInput = true;
        stockfishProcess.StartInfo.RedirectStandardOutput = true;
        stockfishProcess.Start();
    }

    void TestCommunicationWithStockfish() {
        stockfishProcess.StandardInput.WriteLine("uci");
        stockfishProcess.StandardInput.WriteLine("isready");
        stockfishProcess.StandardInput.WriteLine("position startpos moves e2e4 e7e5");
        stockfishProcess.StandardInput.WriteLine("go depth 10");

        while (!stockfishProcess.StandardOutput.EndOfStream) {
            string outputLine = stockfishProcess.StandardOutput.ReadLine();
            UnityEngine.Debug.Log(outputLine);
            if (outputLine.StartsWith("bestmove")) {
                // Extract and process the best move
                break;
            }
        }
    }

    void OnApplicationQuit() {
        if (stockfishProcess != null) {
            stockfishProcess.Close();
        }
    }
}

