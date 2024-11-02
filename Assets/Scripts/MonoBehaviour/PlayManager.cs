using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    [SerializeField] bool trackingScore = false;
    [SerializeField] StarterAssetsInputs playerController;
    InputAction pauseAction;
    PlayCanvasController canvas;
    GameData gameData;
    BinaryFormatter binaryFormatter = new BinaryFormatter();
    FileStream saveFile;
    float startingTime;
    bool paused = false;
    int score = 0;
    int currentFps = 0, fpsSampleCount = 30, fpsSampleIndex = 0;
    int[] fpsSamples;

    void Awake()
    {
        instance = this;
        pauseAction = InputSystem.actions.FindActionMap("Player").FindAction("Pause");
        pauseAction.performed += PauseActionFunction;
        UnpauseGame();
    }
    void Start()
    {
        score = 0;
        startingTime = Time.time;
        fpsSamples = new int[fpsSampleCount];
        StartCoroutine(StatsUpdateCycle());
    }

    IEnumerator StatsUpdateCycle()
    {
        if (!canvas)
        {
            canvas = PlayCanvasController.instance;
        }

        while (true)
        {
            canvas.UpdateTime(Time.time - startingTime);
            canvas.UpdateScore(score);
            canvas.UpdateFpsCurrent(currentFps);

            int averageFps = 0;
            foreach (var fps in fpsSamples)
            {
                averageFps += fps;
            }
            canvas.UpdateFpsAverage(Mathf.RoundToInt(averageFps / fpsSampleCount));
            yield return new WaitForSeconds(1);
        }
    }

    public void AddScore(int _score)
    {
        if (paused)
        {
            return;
        }
        score += _score;
    }
    void Update()
    {
        currentFps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
        fpsSamples[fpsSampleIndex] = currentFps;
        fpsSampleIndex = (fpsSampleIndex + 1) % fpsSampleCount;
    }

    public void PauseGame()
    {
        paused = true;
        Time.timeScale = 0f;
        canvas.SetPausePanel(true);

        playerController.cursorLocked = false;
        Cursor.lockState = CursorLockMode.None;

        playerController.cursorInputForLook = false;
        playerController.LookInput(Vector2.zero);
    }
    public void UnpauseGame()
    {
        if (!canvas)
        {
            canvas = PlayCanvasController.instance;
        }

        paused = false;
        Time.timeScale = 1f;
        canvas.SetPausePanel(false);

        if (!playerController.GetComponent<PlayerInput>().enabled)
        {
            return;
        }
        playerController.cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;

        playerController.cursorInputForLook = true;
    }

    public void EndGame()
    {
        pauseAction.performed -= PauseActionFunction;

        if (trackingScore)
        {
            UpdateHighscores();
        }

        canvas.ChangePauseToGameOver();
        PauseGame();
    }

    void UpdateHighscores()
    {
        LoadData();
        gameData.score = Mathf.Max(gameData.score, score);
        gameData.time = Mathf.Max(gameData.time, Time.time - startingTime);
        SaveData();
    }

    void LoadData()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        if (!File.Exists(destination))
        {
            gameData = new GameData(0, 0);
            return;
        }

        saveFile = File.OpenRead(destination);
        gameData = (GameData)binaryFormatter.Deserialize(saveFile);
        saveFile.Close();
    }

    void SaveData()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        if (File.Exists(destination))
        {
            saveFile = File.OpenWrite(destination);
        }
        else
        {
            saveFile = File.Create(destination);
        }

        binaryFormatter.Serialize(saveFile, gameData);
        saveFile.Close();
    }

    void PauseActionFunction(InputAction.CallbackContext ctx)
    {
        if (paused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }
}
