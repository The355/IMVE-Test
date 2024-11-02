using UnityEngine;

public class Equipable
{
    // public string name, desc, tooltip; //etc etc
    public int durability = 100;
    // bool equipped = false;
    public virtual void Use()
    {
        durability--;
        Debug.Log("Durability: " + durability);
        if (durability <= 0)
        {
            Unequip();
        }
    }

    public virtual void Equip()
    {
        if (durability <= 0)
        {
            return;
        }
        // equipped = true;
    }
    public virtual void Unequip()
    {
        // equipped = false;
    }
}
