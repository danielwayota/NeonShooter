using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyHorde : MonoBehaviour
{
    public GameObject hordeEnemyPrfb;

    public int minEnemies = 1;
    public int maxEnemies = 2;

    private List<Enemy> enemies;

    /// =============================================
    void Awake()
    {
        this.enemies = new List<Enemy>();

        int enemyCount = Random.Range(this.minEnemies, this.maxEnemies);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0
            );

            var go = Instantiate(
                this.hordeEnemyPrfb,
                this.transform.position + offset,
                Quaternion.identity
            );

            this.enemies.Add(go.GetComponent<Enemy>());
        }

        StartCoroutine(this.MoveEnemiesRutine());
    }

    /// =============================================
    IEnumerator MoveEnemiesRutine()
    {
        var wait = new WaitForSeconds(1f);

        while (this.enemies.Count != 0)
        {
            bool doTheNextMove = true;

            foreach (var enemy in this.enemies)
            {
                if (enemy.isMoving)
                {
                    doTheNextMove = false;
                    break;
                }
            }

            if (doTheNextMove)
            {
                // TODO: Search for a line in the grid and move the enemies
                var positions = EnemyGrid.current.GetRandomFreeCoordinateLine(this.enemies.Count);

                for (int i = 0; i < this.enemies.Count; i++)
                {
                    this.enemies[i].targetGridCoordinates = positions[i];
                }
            }

            yield return wait;
        }

        Destroy(this.gameObject);
    }

    /// =============================================
    public void OnEnemyDestroyed(Enemy e)
    {
        this.enemies.Remove(e);
    }
}
