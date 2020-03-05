using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyGrid : MonoBehaviour
{
    private static EnemyGrid _current;
    public static EnemyGrid current
    {
        get {
            if (_current == null)
            {
                _current = GameObject.FindObjectOfType<EnemyGrid>();
            }

            return _current;
        }
    }

    public Transform rightCorner;
    public Transform leftCorner;

    [Range(1, 100)]
    public int columns = 1;
    [Range(1, 100)]
    public int rows = 1;

    private Vector3[,] positions;
    private bool[,] occupationBuffer;

    /// ==========================================
    private void Awake()
    {
        this.positions = new Vector3[this.columns, this.rows];
        this.occupationBuffer = new bool[this.columns, this.rows];

        StartCoroutine(this.GeneratePoints());
    }

    /// ==========================================
    IEnumerator GeneratePoints()
    {
        float width = this.rightCorner.position.x - this.leftCorner.position.x;
        float height = this.rightCorner.position.y - this.leftCorner.position.y;

        float horizontalStep = width / this.columns;
        float verticalStep = height / this.rows;

        for (int j = 0; j < this.rows; j++)
        {
            for (int i = 0; i < this.columns; i++)
            {
                var pos = new Vector3(
                    i * horizontalStep,
                    j * verticalStep,
                    0
                );

                pos += this.leftCorner.position;

                this.positions[i,j] = pos;
                this.occupationBuffer[i,j] = false;
            }

            yield return null;
        }
    }

    /// ==========================================
    public Vector2Int[] GetRandomPointLine(int size)
    {
        Vector2Int[] points = new Vector2Int[size];

        bool done = false;

        while (!done)
        {
            var pivot = this.GetRandomFreeCoordinate();

            int horizontalDirection = 0;
            int verticalDirection = 0;

            float dice = Random.Range(0f, 1f);

            if (dice < 0.5f)
            {
                // Can go right? Go right. Else, go left;
                horizontalDirection = (pivot.x + size < this.columns) ? 1 : -1;
            }
            else
            {
                // Can go down? Go down. Else, go up;
                verticalDirection = (pivot.y + size < this.rows) ? 1 : -1;
            }

            done = true;
            for (int i = 0; i < size; i++)
            {
                if (this.isCoordinateHeld(pivot.x, pivot.y))
                {
                    done = false;
                    break;
                }

                points[i] = new Vector2Int(pivot.x, pivot.y);

                pivot.x += horizontalDirection;
                pivot.y += verticalDirection;
            }
        }

        return points;
    }

    /// ==========================================
    public Vector3 GetPosition(Vector2Int coordinate)
    {
        return this.positions[coordinate.x, coordinate.y];
    }

    /// ==========================================
    public Vector2Int GetRandomFreeCoordinate()
    {
        bool coordinateHeld = true;
        int x = 0;
        int y = 0;

        while (coordinateHeld)
        {
            x = Random.Range(0, this.columns);
            y = Random.Range(0, this.rows);

            coordinateHeld = this.occupationBuffer[x, y];
        }

        return new Vector2Int(x, y);
    }

    /// ==========================================
    public Vector2Int GetFarPointCoordinates(Vector2Int current)
    {
        bool found = false;

        Vector2Int newTarget = current;

        while (found == false)
        {
            newTarget = EnemyGrid.current.GetRandomFreeCoordinate();

            if (current != newTarget)
            {
                found = true;
            }
        }

        return newTarget;
    }

    /// ==========================================
    public bool isCoordinateHeld(int x, int y)
    {
        return this.occupationBuffer[x, y];
    }

    /// ==========================================
    public void HoldCoordinate(Vector2Int coordinate)
    {
        this.occupationBuffer[coordinate.x, coordinate.y] = true;
    }

    /// ==========================================
    public void LeaveCoordinate(Vector2Int coordinate)
    {
        this.occupationBuffer[coordinate.x, coordinate.y] = false;
    }

    /// ==========================================
    private void OnDrawGizmos()
    {
        if (this.positions != null)
        {
            Gizmos.color = Color.red;
            foreach (var pos in this.positions)
            {
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }

        if (this.rightCorner != null && this.leftCorner != null)
        {
            Gizmos.color = Color.white;
            Vector3 center = Vector3.Lerp(this.rightCorner.position, this.leftCorner.position, 0.5f);
            Vector3 size = this.rightCorner.position - this.leftCorner.position;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
