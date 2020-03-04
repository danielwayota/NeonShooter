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

    private List<Vector3> positions;

    /// ==========================================
    private void Awake()
    {
        this.positions = new List<Vector3>();

        StartCoroutine(this.GeneratePoints());
    }

    /// ==========================================
    IEnumerator GeneratePoints()
    {
        float width = this.rightCorner.position.x - this.leftCorner.position.x;
        float height = this.rightCorner.position.y - this.leftCorner.position.y;

        float horizontalStep = width / this.columns;
        float verticalStep = height / this.rows;

        for (int i = 0; i <= this.columns; i++)
        {
            for (int j = 0; j <= this.rows; j++)
            {
                var pos = new Vector3(
                    i * horizontalStep,
                    j * verticalStep,
                    0
                );

                pos += this.leftCorner.position;

                this.positions.Add(pos);
            }

            yield return null;
        }
    }

    /// ==========================================
    public Vector3 GetRandomPointLine(Vector3 current, float minDistanceToPoint)
    {
        return Vector3.zero;
    }

    /// ==========================================
    public Vector3 GetRandomPoint()
    {
        int index = Random.Range(0, this.positions.Count);

        return this.positions[index];
    }

    /// ==========================================
    public Vector3 GetFarPoint(Vector3 current, float minDistanceToPoint)
    {
        bool found = false;

        Vector3 newTarget = current;

        float minDistance = 2 * minDistanceToPoint;

        while (found == false)
        {
            newTarget = EnemyGrid.current.GetRandomPoint();

            float distance = (newTarget - current).sqrMagnitude;
            if (distance > minDistance)
            {
                found = true;
            }
        }

        return newTarget;
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
