using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public string mainMenu, play, stressTest;
    public void ChangeSceneTo(string sceneName)
    {
        if (PlayManager.instance)
        {
            PlayManager.instance.EndGame();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneToMainMenu()
    {
        ChangeSceneTo(mainMenu);
    }

    public void ChangeSceneToPlay()
    {
        ChangeSceneTo(play);
    }

    public void ChangeSceneToStressTest()
    {
        ChangeSceneTo(stressTest);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
