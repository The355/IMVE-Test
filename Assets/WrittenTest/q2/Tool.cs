using UnityEngine;

public class Tool : Equipable
{
    int tier = 1;
    float efficiency = 1;

    public Tool(int _tier, float _efficiency) // In reality the parameter something like itemId
    {
        tier = _tier;
        efficiency = _efficiency;

        durability = 150;
    }

    public /*override*/ void Use(GameObject harvestable) // Bila parameter serupa, pakai keyword override
    {
        base.Use();

        Debug.Log("Harvesting!");
        // if(harvestable.tier <= tier)
        //     harvestable.AddProgress(efficiency)
    }
}
