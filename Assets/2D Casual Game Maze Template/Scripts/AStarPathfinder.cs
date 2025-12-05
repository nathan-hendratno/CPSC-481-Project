using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private GridScanner grid;

    private void Awake()
    {
        grid = FindObjectOfType<GridScanner>();
        if (grid == null)
        {
            Debug.LogError("AStarPathfinder: CustomGridScanner not found in scene!");
        }
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        if (grid == null)
        {
            Debug.LogError("AStarPathfinder: grid missing");
            return null;
        }

        var open = new List<Vector2Int> { start };
        var gScore = new Dictionary<Vector2Int, int> { { start, 0 } };
        var fScore = new Dictionary<Vector2Int, int> { { start, Heuristic(start, goal) } };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var closed = new HashSet<Vector2Int>();

        while (open.Count > 0)
        {
            open.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Vector2Int current = open[0];

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            open.RemoveAt(0);
            closed.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closed.Contains(neighbor)) continue;

                int tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }
        return null;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int[] dirs = new[]
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };

        foreach (var d in dirs)
        {
            Vector2Int next = cell + d;
            if (grid.IsWalkable(next))
                result.Add(next);
        }

        return result;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new() { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}
