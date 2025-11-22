using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private List<Vector2> path = new List<Vector2>();
    private Vector2 lastCell;
    private int score = 0;

    private void Start()
    {
        // Find ANY TextMeshProUGUI named ScoreText anywhere in the scene
        TextMeshProUGUI[] allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (var t in allTexts)
        {
            if (t.name == "ScoreText")
            {
                scoreText = t;
                break;
            }
        }

        if (scoreText == null)
        {
            Debug.LogError("PathScore: Could not find TMP object named 'ScoreText' in the scene!");
            return;
        }

        lastCell = GetCell(transform.position);
        path.Add(lastCell);
        UpdateScoreDisplay();
    }

    private void Update()
    {
        Vector2 currentCell = GetCell(transform.position);

        if (currentCell == lastCell)
            return;

        // Backtracking
        if (path.Count > 1 && currentCell == path[path.Count - 2])
        {
            path.RemoveAt(path.Count - 1);
            score--;
        }
        else
        {
            // Forward
            path.Add(currentCell);
            score++;
        }

        lastCell = currentCell;
        UpdateScoreDisplay();
    }

    private Vector2 GetCell(Vector2 pos)
    {
        float cellX = Mathf.Round(pos.x - 0.5f);
        float cellY = Mathf.Round(pos.y - 0.5f);
        return new Vector2(cellX, cellY);
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }
}



