using UnityEngine;

public class q3_FunRun : MonoBehaviour
{
    void Start()
    {
        for (uint a = 1; a <= 100; a++)
        {
            Transmute(a);
        }
    }

    void Transmute(uint num)
    {
        string result = "";
        if (num % 2 == 0)
        {
            result += "Fun";
        }
        if (num % 7 == 0)
        {
            result += "Run";
        }

        if (result.Length == 0)
        {
            Cetak(num.ToString());
        }
        else
        {
            Cetak(result);
        }
        return;
    }

    void Cetak(string content)
    {
        Debug.Log(content);
    }
}
