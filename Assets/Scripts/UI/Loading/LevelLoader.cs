using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static string nextLevel;

    public static void Load(string levelName)
    {
        nextLevel = levelName;

        SceneManager.LoadScene(LevelNames.LOADING);
    }

    void Start()
    {
        StartCoroutine(this.DoTheLoad(nextLevel));
    }

    IEnumerator DoTheLoad(string levelName)
    {
        yield return new WaitForSeconds(0.5f);

        var operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}