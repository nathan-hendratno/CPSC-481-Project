using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDrawer : MonoBehaviour
{
    private LineRenderer line;
    private AStarPathfinder aStar;
    private PathCost pathCost;

    public Transform player;   // optional if assigned; will auto-find if null
    public Transform win;      // optional if assigned; will auto-find if null

    [SerializeField] private float drawDelay = 0.05f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        if (line == null) line = gameObject.AddComponent<LineRenderer>();
        line.positionCount = 0;

        aStar = FindObjectOfType<AStarPathfinder>();
        pathCost = FindObjectOfType<PathCost>();

        // Start-coroutine to ensure player/win exist
        StartCoroutine(WaitForReferences());
    }

    private IEnumerator WaitForReferences()
    {
        // Wait for player
        while (player == null)
        {
            var pgo = GameObject.FindGameObjectWithTag("Player");
            if (pgo != null) player = pgo.transform;
            yield return null;
        }

        // Wait for win
        while (win == null)
        {
            var wgo = GameObject.FindGameObjectWithTag("Win");
            if (wgo != null) win = wgo.transform;
            yield return null;
        }

        Debug.Log("AStarDrawer: Found PLAYER = " + player.name);
        Debug.Log("AStarDrawer: Found WIN = " + win.name);
    }

    public void DrawOptimalPath()
    {
        StopAllCoroutines();
        StartCoroutine(DrawPathAnimated());
    }

    private IEnumerator DrawPathAnimated()
    {
        // Get start cell from PathCost if available; otherwise fallback to current player position
        Vector2Int startCell = new Vector2Int(int.MinValue, int.MinValue);

        if (pathCost != null)
        {
            var hist = pathCost.GetCellHistory();
            if (hist != null && hist.Count > 0)
            {
                startCell = hist[0]; // first recorded cell = start
            }
        }

        if (startCell.x == int.MinValue)
        {
            // fallback
            startCell = WorldToCell(player.position);
        }

        Vector2Int goalCell = WorldToCell(win.position);

        Debug.Log("StartCell: " + startCell + "   GoalCell: " + goalCell);

        List<Vector2Int> path = aStar.FindPath(startCell, goalCell);

        Debug.Log("A* Path length = " + (path != null ? path.Count : 0));

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("A* found no path!");
            yield break;
        }

        // hide current player and player trail already handled elsewhere; clear line
        line.positionCount = 0;

        for (int i = 0; i < path.Count; i++)
        {
            line.positionCount = i + 1;
            Vector3 wp = CellToWorld(path[i]);
            line.SetPosition(i, wp);

            yield return new WaitForSeconds(drawDelay);
        }
    }

    // Convert world → integer grid cell (matching your PathCost/GetCell logic)
    private Vector2Int WorldToCell(Vector3 wp)
    {
        int gx = Mathf.RoundToInt(wp.x - 0.5f);
        int gy = Mathf.RoundToInt(wp.y - 0.5f);
        return new Vector2Int(gx, gy);
    }

    // grid cell → world center
    private Vector3 CellToWorld(Vector2Int c)
    {
        return new Vector3(c.x + 0.5f, c.y + 0.5f, 0f);
    }
}



