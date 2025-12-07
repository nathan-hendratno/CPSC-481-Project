using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathCost : MonoBehaviour
{
    private TextMeshProUGUI costText;

    private struct PathNode
    {
        public Vector2 cell; // stored as grid cell (x,y) using your GetCell conversion
        public int cost;
    }

    private List<PathNode> history = new List<PathNode>();

    private int cost = 0;
    private Vector2 lastCell;

    public System.Action<List<Vector2>> OnPathChanged;

    private void Start()
    {
        costText = GameObject.Find("CostText")?.GetComponent<TextMeshProUGUI>();

        lastCell = GetCell(transform.position);

        // Add starting node
        history.Add(new PathNode
        {
            cell = lastCell,
            cost = 0
        });

        UpdateCostDisplay();
    }

    private void Update()
    {
        Vector2 current = GetCell(transform.position);

        if (current == lastCell)
            return;

        int index = history.FindIndex(n => n.cell == current);

        if (index != -1)
        {
            // rewind
            cost = history[index].cost;

            // remove everything after that point
            history.RemoveRange(index + 1, history.Count - (index + 1));
        }
        else
        {
            // Forward movement
            cost++;
            history.Add(new PathNode
            {
                cell = current,
                cost = cost
            });
        }

        lastCell = current;
        UpdateCostDisplay();

        // Notify any listeners with world-grid positions (as Vector2 grid coords)
        OnPathChanged?.Invoke(GetPositionList());
    }

    private List<Vector2> GetPositionList()
    {
        List<Vector2> list = new List<Vector2>();
        foreach (var node in history)
            list.Add(node.cell);
        return list;
    }

    private Vector2 GetCell(Vector2 pos)
    {
        float cellX = Mathf.Round(pos.x - 0.5f);
        float cellY = Mathf.Round(pos.y - 0.5f);
        return new Vector2(cellX, cellY);
    }

    private void UpdateCostDisplay()
    {
        if (costText != null)
            costText.text = "Cost: " + cost;
    }

    // -----------------------
    // Public helpers for A*
    // -----------------------

    // Returns the tracked history as integer grid cells (Vector2Int)
    // The first element is the starting cell.
    public List<Vector2Int> GetCellHistory()
    {
        var outList = new List<Vector2Int>(history.Count);
        foreach (var n in history)
        {
            int gx = Mathf.RoundToInt(n.cell.x);
            int gy = Mathf.RoundToInt(n.cell.y);
            outList.Add(new Vector2Int(gx, gy));
        }
        return outList;
    }

    // Return start cell (if any), or an "invalid" sentinel if empty
    public Vector2Int GetStartCell()
    {
        if (history.Count == 0)
            return new Vector2Int(int.MinValue, int.MinValue);
        var c = history[0].cell;
        return new Vector2Int(Mathf.RoundToInt(c.x), Mathf.RoundToInt(c.y));
    }

    public int GetCost()
    {
        return cost;
    }

}

