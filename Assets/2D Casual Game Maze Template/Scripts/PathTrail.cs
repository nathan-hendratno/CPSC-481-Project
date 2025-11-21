using System.Collections.Generic;
using UnityEngine;

namespace MazeTemplate
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathTrail : MonoBehaviour
    {
        private LineRenderer line;
        private List<Vector3> points = new List<Vector3>();
        private Vector3 lastPos;

        private void Start()
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 0;

            lastPos = transform.position;
            points.Add(lastPos);
            UpdateLine();
        }

        private void Update()
        {
            Vector3 currentPos = transform.position;

            // Only add a node if the player actually moved to a new world position
            if (Vector3.Distance(currentPos, lastPos) < 0.1f)
                return;

            // If backtracking (returning to previous node)
            if (points.Count > 1 && Vector3.Distance(currentPos, points[points.Count - 2]) < 0.1f)
            {
                // remove last point
                points.RemoveAt(points.Count - 1);
            }
            else
            {
                // add a new point
                points.Add(currentPos);
            }

            lastPos = currentPos;
            UpdateLine();
        }

        private void UpdateLine()
        {
            line.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                line.SetPosition(i, points[i]);
            }
        }
    }
}





