using System.Collections.Generic;
using UnityEngine;

public class PathTrail : MonoBehaviour
{
    private LineRenderer line;
    private PathCost pathCost;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;

        pathCost = FindObjectOfType<PathCost>();
        if (pathCost != null)
        {
            pathCost.OnPathChanged += UpdateTrail;
        }
    }

    private void UpdateTrail(List<Vector2> visitedCells)
    {
        // Match line exactly to the visited path, including rewinds
        int count = visitedCells.Count;
        line.positionCount = count;

        for (int i = 0; i < count; i++)
        {
            Vector2 c = visitedCells[i];
            line.SetPosition(i, new Vector3(c.x + 0.5f, c.y + 0.5f, 0f));
        }
    }

    private void OnDestroy()
    {
        if (pathCost != null)
        {
            pathCost.OnPathChanged -= UpdateTrail;
        }
    }
}
