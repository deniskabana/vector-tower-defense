using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] public Tower[] towers;

    public int selectedTower = -1; // -1 = no tower selected

    void Start()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        if (selectedTower < 0 || selectedTower >= towers.Length)
            return null;
        return towers[selectedTower];
    }

    public void SelectTower(int index)
    {
        if (selectedTower == index)
        {
            selectedTower = 0;
            return;
        }
        selectedTower = index;
    }

    public int GetSelectedTowerPrice()
    {
        BaseTurret turretScript = GetSelectedTower().prefab.GetComponent<BaseTurret>();
        int cost = turretScript.towerPrice;
        return cost;
    }
}
