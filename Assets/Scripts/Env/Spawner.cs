using System.Collections;

using UnityEngine;

[System.Serializable]
public struct EnemyData
{
    public int size;
    public GameObject prefab;
}

public class Spawner : MonoBehaviour
{
    public HUD hud;

    [Header("Enemies")]
    public EnemyData[] enemyPrfbs;

    [HeaderAttribute("Spawn")]
    public Transform spawnLineLeft;
    public Transform spawnLineRight;

    private int level = 1;
    private int maxLevel = 8;

    private int currentBagSize = 1;
    private float waitBetweenSpawns = 5f;

    /// ==========================================
    private void Awake()
    {
        StartCoroutine(this.SpawnLoop());
        StartCoroutine(this.UpgradeSpawnerLoop());

        this.hud.UpdateDanger(this.level);
    }

    /// ==========================================
    IEnumerator UpgradeSpawnerLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            if (this.level >= this.maxLevel)
                break;

            this.level++;
            this.currentBagSize = Mathf.RoundToInt(this.currentBagSize * 1.5f);
            this.waitBetweenSpawns *= 0.9f;

            this.hud.UpdateDanger(this.level);
        }
    }

    /// ==========================================
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            this.SpawnEnemies();

            yield return new WaitForSeconds(this.waitBetweenSpawns);
        }
    }

    /// ==========================================
    protected void SpawnEnemies()
    {
        int bagSize = this.currentBagSize;

        int iterations = 0;
        int maxAttempts = 100;

        while (bagSize > 0 && iterations < maxAttempts)
        {
            int index = Random.Range(0, this.enemyPrfbs.Length);
            var data = this.enemyPrfbs[index];

            if (data.size <= bagSize)
            {
                bagSize -= data.size;

                float percentage = Random.Range(0f, 1f);
                Vector3 position = Vector3.Lerp(
                    this.spawnLineLeft.position,
                    this.spawnLineRight.position,
                    percentage
                );

                var go = Instantiate(data.prefab, position, Quaternion.identity);
            }

            iterations++;
        }
    }
}
