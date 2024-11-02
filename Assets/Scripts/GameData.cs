[System.Serializable]
public class GameData
{
    public int score;
    public float time;

    public GameData(int _score, float _time)
    {
        score = _score;
        time = _time;
    }
}
