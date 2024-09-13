using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField]
    public Tower[] towers;

    public int selectedTower = -1; // -1 = no tower selected

    void Start()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[Mathf.Clamp(selectedTower, 0, towers.Length - 1)];
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
