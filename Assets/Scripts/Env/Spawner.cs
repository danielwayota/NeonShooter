using System.Collections;

using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject enemyPrfb;

    [HeaderAttribute("Spawn")]
    public int enemyMax = 1;
    public float spawnRate = 1f;

    public Transform spawnLineLeft;
    public Transform spawnLineRight;

    /// ==========================================
    private void Awake()
    {
        StartCoroutine(this.SpawnLoop());
    }

    /// ==========================================
    IEnumerator SpawnLoop()
    {
        int i = 0;

        while (i < this.enemyMax)
        {
            yield return new WaitForSeconds(this.spawnRate);

            float percentage = Random.Range(0f, 1f);
            Vector3 position = Vector3.Lerp(
                this.spawnLineLeft.position,
                this.spawnLineRight.position,
                percentage
            );

            var go = Instantiate(this.enemyPrfb, position, Quaternion.identity);

            i++;
        }
    }
}
