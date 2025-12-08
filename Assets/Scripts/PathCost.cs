using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PathCost : MonoBehaviour
{
    private TextMeshProUGUI costText;

    private struct PathNode
    {
        public Vector2 cell;
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

        // No movement = no update
        if (current == lastCell)
            return;

        // Check if this tile was visited before (backtrack)
        int index = history.FindIndex(n => n.cell == current);

        if (index != -1)
        {
            // -------------------------
            // BACKTRACK REWIND RESTORE
            // -------------------------
            cost = history[index].cost;
            history.RemoveRange(index + 1, history.Count - (index + 1));
        }
        else
        {
            // -------------------------
            // SLIDE TILE COUNT FIX
            // (counts every tile moved through)
            // -------------------------

            int dx = Mathf.RoundToInt(current.x - lastCell.x);
            int dy = Mathf.RoundToInt(current.y - lastCell.y);

            int stepX = dx == 0 ? 0 : (dx > 0 ? 1 : -1);
            int stepY = dy == 0 ? 0 : (dy > 0 ? 1 : -1);

            // Only one axis moves at a time (slide logic)
            int steps = Mathf.Abs(dx) + Mathf.Abs(dy);

            for (int i = 1; i <= steps; i++)
            {
                Vector2 newCell = new Vector2(
                    lastCell.x + (stepX * i),
                    lastCell.y + (stepY * i)
                );

                cost++;
                history.Add(new PathNode { cell = newCell, cost = cost });
            }
        }

        lastCell = current;
        UpdateCostDisplay();
        OnPathChanged?.Invoke(GetPositionList());   // <--- TRIGGER TRAIL UPDATES
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

    public void AddCost(int amount)
    {
        if (amount <= 0)
            return;

        cost += amount;

        // Add this new position update to history
        history.Add(new PathNode
        {
            cell = lastCell,
            cost = cost
        });

        UpdateCostDisplay();
    }

}
