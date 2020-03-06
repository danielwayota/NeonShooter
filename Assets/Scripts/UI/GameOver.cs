using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void RePlay()
    {
        LevelLoader.Load(LevelNames.GAME);
    }

    public void Exit()
    {
        LevelLoader.Load(LevelNames.MAIN_MENU);
    }
}
