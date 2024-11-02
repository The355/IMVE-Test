using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonstrate : MonoBehaviour
{
    void Start()
    {
        Tool Hammer = new Tool(3, 1.5f);
        Weapon Spear = new Weapon(5, 1);

        Hammer.Use(null);
        Spear.Use(null);
    }
}
