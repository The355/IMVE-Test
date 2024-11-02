using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenuCanvasController : MonoBehaviour
{
    public TMP_Text time, score;
    BinaryFormatter binaryFormatter;
    FileStream saveFile;
    void Awake()
    {
        binaryFormatter = new BinaryFormatter();
        LoadHighscore();
    }

    void LoadHighscore()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        if (!File.Exists(destination))
        {
            UpdateTime(0);
            UpdateScore(0);
            return;
        }

        saveFile = File.OpenRead(destination);
        GameData gameData = (GameData)binaryFormatter.Deserialize(saveFile);
        saveFile.Close();

        UpdateTime(gameData.time);
        UpdateScore(gameData.score);
    }

    void UpdateTime(float _time)
    {
        if (_time >= 3600)
        {
            time.text = System.TimeSpan.FromSeconds(_time).ToString("hh':'mm':'ss");
        }
        else if (_time >= 60)
        {
            time.text = System.TimeSpan.FromSeconds(_time).ToString("mm':'ss");
        }
        else if (_time >= 10)
        {
            time.text = System.TimeSpan.FromSeconds(_time).ToString("ss");
        }
        else
        {
            time.text = ((int)_time).ToString();
        }

    }

    void UpdateScore(int _score)
    {
        score.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", _score);
    }

    public void ResetHighscore()
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

        binaryFormatter.Serialize(saveFile, new GameData(0, 0));
        saveFile.Close();

        UpdateTime(0);
        UpdateScore(0);
    }
}
