using System.Collections.Generic;
using UnityEngine;

public class GridScanner : MonoBehaviour
{
    public HashSet<Vector2Int> wallCells = new HashSet<Vector2Int>();

    private void Awake()
    {
        BuildWallGrid();
    }

    private void BuildWallGrid()
    {
        wallCells.Clear();

        // Find every object named "Square" in the scene
        SquareMarker[] squares = FindObjectsOfType<SquareMarker>();

        foreach (var sq in squares)
        {
            Vector3 wp = sq.transform.position;

            int gx = Mathf.RoundToInt(wp.x - 0.5f);
            int gy = Mathf.RoundToInt(wp.y - 0.5f);

            wallCells.Add(new Vector2Int(gx, gy));
        }

        Debug.Log("CustomGridScanner: Walls found = " + wallCells.Count);
    }

    public bool IsWalkable(Vector2Int cell)
    {
        return !wallCells.Contains(cell);
    }

    public List<Vector2Int> GetNeighbors(Vector2Int c)
    {
        List<Vector2Int> kids = new List<Vector2Int>();

        Vector2Int[] dirs =
        {
            new Vector2Int( 1,  0 ),
            new Vector2Int(-1,  0 ),
            new Vector2Int( 0,  1 ),
            new Vector2Int( 0, -1 ),
        };

        foreach (var d in dirs)
        {
            Vector2Int next = c + d;
            if (IsWalkable(next))
                kids.Add(next);
        }

        return kids;
    }
}

