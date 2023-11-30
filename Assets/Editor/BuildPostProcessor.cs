using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class BuildPostProcessor {
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        if (target == BuildTarget.StandaloneOSX) {
            string sourcePath = Path.Combine(Application.dataPath, "Executables/stockfish");
            string targetPath = Path.Combine(pathToBuiltProject, "Contents/MacOS/stockfish");

            if (File.Exists(sourcePath)) {
                File.Copy(sourcePath, targetPath, true);
                Debug.Log("Stockfish executable copied to build folder.");
            }
            else {
                Debug.LogError("Stockfish executable not found at: " + sourcePath);
            }
        }
    }
}

