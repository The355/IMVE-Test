using UnityEngine;

public class Weapon : Equipable
{
    float damage = 10;
    int attribute = 0; // can be replaced with struct
    public Weapon(int _damage, int _attribute) // In reality the parameter something like itemId
    {
        damage = _damage;
        attribute = _attribute;

        durability = 50;
    }

    public /*override*/ void Use(GameObject enemy) // Bila parameter serupa, pakai keyword override
    {
        base.Use();

        Debug.Log("Attacking!");
        // float totalDamage = damage;
        // if(enemy.weakness <= attribute)
        //     totalDamage *= 1.5;
        // enemy.getDamage(totalDamage)
    }
}
