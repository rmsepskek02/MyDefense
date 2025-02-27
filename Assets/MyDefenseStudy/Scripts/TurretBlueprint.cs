using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public GameObject turretPrefab;
    public float cost;
    public GameObject upgradeTurretPrefab;
    public int upgradeCost;
    public Vector3 offset;

    public float GetSellCost()
    {
        return cost / 2;
    }
}
