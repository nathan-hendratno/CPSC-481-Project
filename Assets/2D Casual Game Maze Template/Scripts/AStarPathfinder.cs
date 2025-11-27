using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* pathfinder working on integer grid cells (Vector2Int).
/// Expects a CustomGridScanner in the scene that provides walkability & neighbors.
/// </summary>
public class AStarPathfinder : MonoBehaviour
{
    private GridScanner grid;

    private void Awake()
    {
        grid = FindObjectOfType<GridScanner>();
        if (grid == null)
            Debug.LogError("AStarPathfinder: CustomGridScanner not found in scene!");
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        // Manhattan distance for 4-directional grid
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // Public API: find path from start to goal (both in integer grid coords)
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        if (grid == null)
        {
            Debug.LogError("AStarPathfinder: grid is null");
            return null;
        }

        // Early exit
        if (start == goal)
            return new List<Vector2Int> { start };

        var open = new List<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, int> { [start] = Heuristic(start, goal) };
        var closed = new HashSet<Vector2Int>();

        while (open.Count > 0)
        {
            // pick node in open with lowest fScore
            open.Sort((x, y) =>
            {
                int fx = fScore.ContainsKey(x) ? fScore[x] : int.MaxValue;
                int fy = fScore.ContainsKey(y) ? fScore[y] : int.MaxValue;
                return fx.CompareTo(fy);
            });

            Vector2Int current = open[0];

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            open.RemoveAt(0);
            closed.Add(current);

            foreach (var neighbor in grid.GetNeighbors(current))
            {
                if (closed.Contains(neighbor))
                    continue;

                int tentativeG = gScore[current] + 1; // each move costs 1

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

        // No path found
        return null;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        var path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}

