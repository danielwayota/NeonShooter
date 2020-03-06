using UnityEngine;

using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    [Header("UI")]
    public GameObject gameOverScreen;

    void Awake()
    {
        current = this;

        this.gameOverScreen.SetActive(false);
    }

    public void OnPlayerDeath()
    {
        var spawner = GameObject.FindObjectOfType<Spawner>();

        Destroy(spawner.gameObject);

        StartCoroutine(this.WaitForGameOverScreen());
    }

    IEnumerator WaitForGameOverScreen()
    {
        yield return new WaitForSeconds(2f);

        this.gameOverScreen.SetActive(true);
    }
}