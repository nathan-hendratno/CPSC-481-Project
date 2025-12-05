using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDrawer : MonoBehaviour
{
    private LineRenderer line;
    private AStarPathfinder aStar;
    private PathCost pathCost;
    private PathTrail trail;

    public Transform player;
    public Transform win;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        aStar = FindObjectOfType<AStarPathfinder>();
        pathCost = FindObjectOfType<PathCost>();
        trail = FindObjectOfType<PathTrail>();

        StartCoroutine(FindRefsThenDraw());
    }

    private IEnumerator FindRefsThenDraw()
    {
        while (player == null || win == null)
        {
            yield return null;
        }
    }

    public void DrawOptimalPath()
    {
        StartCoroutine(DrawPathAnimated());
    }

    private IEnumerator DrawPathAnimated()
    {
        Vector2Int startCell = pathCost.GetCellHistory()[0];
        Vector2Int goalCell = WorldToCell(win.position);

        Debug.Log("StartCell: " + startCell + "   GoalCell: " + goalCell);

        var path = aStar.FindPath(startCell, goalCell);
        Debug.Log("A* Path length = " + path.Count);

        if (trail != null)
            trail.gameObject.SetActive(false);
        if (player != null)
            player.gameObject.SetActive(false);

        GameObject ghost = new GameObject("Ghost");
        ghost.transform.position = CellToWorld(path[0]);

        line.positionCount = 1;
        line.SetPosition(0, ghost.transform.position);

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 worldPos = CellToWorld(path[i]);
            ghost.transform.position = worldPos;

            line.positionCount = i + 1;
            line.SetPosition(i, worldPos);

            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("Replay Finished!");
    }

    private Vector2Int WorldToCell(Vector3 wp)
    {
        return new Vector2Int(Mathf.RoundToInt(wp.x - 0.5f), Mathf.RoundToInt(wp.y - 0.5f));
    }

    private Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0);
    }
}





