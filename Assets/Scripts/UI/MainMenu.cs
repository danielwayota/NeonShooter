using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        LevelLoader.Load(LevelNames.GAME);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
