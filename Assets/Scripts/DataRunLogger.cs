using System;
using System.IO;
using UnityEngine;

public class RunDataLogger : MonoBehaviour
{
    private static string filePath;

    private void Awake()
    {
        // Ensure file path is initialized once
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Path.Combine(Application.persistentDataPath, "run_data.csv");

            // If file does not exist yet, create it with a header
            if (!File.Exists(filePath))
            {
                string header = "timestamp,levelName,playerTiles,optimalTiles,efficiencyPercent";
                File.WriteAllText(filePath, header + Environment.NewLine);
                Debug.Log("RunDataLogger: Created dataset file at " + filePath);
            }
        }
    }

    public static void LogRun(string levelName, int playerTiles, int optimalTiles, int efficiencyPercent)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Path.Combine(Application.persistentDataPath, "run_data.csv");

            if (!File.Exists(filePath))
            {
                string header = "timestamp,levelName,playerTiles,optimalTiles,efficiencyPercent";
                File.WriteAllText(filePath, header + Environment.NewLine);
            }
        }

        string timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601
        string line = $"{timestamp},{levelName},{playerTiles},{optimalTiles},{efficiencyPercent}";

        File.AppendAllText(filePath, line + Environment.NewLine);
        Debug.Log("RunDataLogger: Logged run -> " + line);
        Debug.Log("Dataset location: " + filePath);
    }
}

