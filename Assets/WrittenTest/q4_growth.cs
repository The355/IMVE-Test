using UnityEngine;

public class q4_growth : MonoBehaviour
{
    class Tree
    {
        double height;
        public Tree()
        {
            height = 100;
        }

        public void Grow(float strength)
        {
            height *= strength;
            Cetak(height + " cm");
        }

        void Cetak(string content)
        {
            Debug.Log(content);
        }
    }
    void Start()
    {
        Tree theTree = new Tree();

        // Nugraha action
        float fertilizerFactor = 1.1f;
        for (int a = 0; a < 50; a++)
        {
            theTree.Grow(fertilizerFactor);
        }

    }

}
