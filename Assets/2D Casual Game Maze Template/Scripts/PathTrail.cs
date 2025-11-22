using System.Collections.Generic;
using UnityEngine;

public class PathTrail : MonoBehaviour
{
    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;

        // Find PathCost and subscribe
        PathCost cost = FindObjectOfType<PathCost>();
        cost.OnPathChanged += UpdatePath;
    }

    private void UpdatePath(List<Vector2> visitedCells)
    {
        line.positionCount = visitedCells.Count;

        for (int i = 0; i < visitedCells.Count; i++)
        {
            Vector2 c = visitedCells[i];
            line.SetPosition(i, new Vector3(c.x + 0.5f, c.y + 0.5f, 0));
        }
    }
}
