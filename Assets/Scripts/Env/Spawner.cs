using System.Collections;

using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject enemyPrfb;

    [HeaderAttribute("Spawn")]
    public float spawnRate = 1f;

    public Transform spawnLineLeft;
    public Transform spawnLineRight;

    private void Awake()
    {
        StartCoroutine(this.SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.spawnRate);

            float percentage = Random.Range(0f, 1f);
            Vector3 position = Vector3.Lerp(
                this.spawnLineLeft.position,
                this.spawnLineRight.position,
                percentage
            );

            var go = Instantiate(this.enemyPrfb, position, Quaternion.identity);

            // TODO: Make the enemies go to fixed position and avoid this
            Destroy(go, 10f);
        }
    }
}
