using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class PlayCanvasController : MonoBehaviour
{
    public static PlayCanvasController instance;
    [SerializeField] TMP_Text time, score, fpsCurrent, fpsAverage, pauseLabel;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject pausePanel;

    void Awake()
    {
        instance = this;
    }

    public void UpdateTime(float _time)
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

    public void UpdateScore(int _score)
    {
        score.text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", _score);
    }

    public void UpdateFpsCurrent(int _fpsCurrent)
    {
        fpsCurrent.text = _fpsCurrent.ToString();
    }

    public void UpdateFpsAverage(int _fpsAverage)
    {
        fpsAverage.text = _fpsAverage.ToString();
    }

    public void SetPausePanel(bool active)
    {
        pausePanel.SetActive(active);
    }
    public void ChangePauseToGameOver()
    {
        pauseLabel.text = "Game Over";
        resumeButton.SetActive(false);
    }
}
