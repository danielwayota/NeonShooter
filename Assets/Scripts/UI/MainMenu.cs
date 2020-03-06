using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;

    public void Play()
    {
        LevelLoader.Load(LevelNames.GAME);
    }

    public void Controls()
    {
        this.mainMenuPanel.SetActive(false);
        this.controlsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        this.mainMenuPanel.SetActive(true);
        this.controlsPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
