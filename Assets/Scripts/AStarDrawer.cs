using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AStarDrawer : MonoBehaviour
{
    private LineRenderer line;
    private AStarPathfinder aStar;
    private PathCost pathCost;
    private Transform player;
    private Transform win;
    private List<Vector2Int> lastPath;

    public void DrawOptimalPath(System.Action onReplayFinished)
    {
        StartCoroutine(DrawPathAnimated(onReplayFinished));
    }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        aStar = FindObjectOfType<AStarPathfinder>();
        pathCost = FindObjectOfType<PathCost>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        win = GameObject.FindGameObjectWithTag("Win").transform;
    }

    private IEnumerator DrawPathAnimated(System.Action onReplayFinished)
    {
        var startCell = pathCost.GetCellHistory()[0];
        var goalCell = WorldToCell(win.position);
        lastPath = aStar.FindPath(startCell, goalCell);

        // Hide player during replay
        player.gameObject.SetActive(false);

        GameObject ghost = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ghost.transform.localScale = Vector3.one * 0.6f;
        ghost.name = "ReplayGhost";
        ghost.GetComponent<Renderer>().material.color = Color.yellow;

        line.positionCount = 1;
        ghost.transform.position = CellToWorld(lastPath[0]);
        line.SetPosition(0, ghost.transform.position);

        for (int i = 1; i < lastPath.Count; i++)
        {
            Vector3 pos = CellToWorld(lastPath[i]);
            ghost.transform.position = pos;
            line.positionCount = i + 1;
            line.SetPosition(i, pos);

            yield return new WaitForSeconds(0.12f);
        }

        Destroy(ghost);

        onReplayFinished?.Invoke();
    }

    public void ShowScoresOnWinScreen()
    {
        // Win UI & Score UI are active now → safe to find it
        TextMeshProUGUI scoreText = GameObject.Find("ScoreCompareText")?.GetComponent<TextMeshProUGUI>();
        if (scoreText == null)
        {
            Debug.LogError("AStarDrawer: Could NOT find ScoreCompareText even after win screen active!");
            return;
        }

        int yourCost = pathCost.GetCost();
        int optimalCost = lastPath.Count;
        int efficiency = Mathf.RoundToInt((float)optimalCost / yourCost * 100f);

        scoreText.text =
            $"Your Path: {yourCost}\n" +
            $"Optimal Path: {optimalCost}\n" +
            $"Efficiency: {efficiency}%";

        scoreText.gameObject.SetActive(true);
    }

    private Vector2Int WorldToCell(Vector3 pos) =>
        new Vector2Int(Mathf.RoundToInt(pos.x - 0.5f), Mathf.RoundToInt(pos.y - 0.5f));

    private Vector3 CellToWorld(Vector2Int cell) =>
        new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);
}


