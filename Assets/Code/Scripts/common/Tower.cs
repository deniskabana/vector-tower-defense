using System;
using UnityEngine;

[Serializable]
public class Tower
{
    public string name;
    public GameObject prefab;
    public Color32 color;
    public Sprite towerPreview;

    public Tower(string name, GameObject prefab, Color32 color, Sprite towerPreview)
    {
        this.name = name;
        this.prefab = prefab;
        this.color = color;
        this.towerPreview = towerPreview;
    }
}
