using UnityEngine;

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
        this.gameOverScreen.SetActive(true);
    }
}