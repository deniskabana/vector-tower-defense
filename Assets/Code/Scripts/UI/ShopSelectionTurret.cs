using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSelectionTurret : MonoBehaviour, IPointerClickHandler
{
    [Header("Attributes")]
    [SerializeField]
    private int towerIndex;

    [SerializeField]
    private GameObject hoverImage;

    private Sprite previewImage;

    private bool initialized = false;

    void Start()
    {
        ShopManager.main.OnTowerSelected += HandleTowerSelectHoverStatus;
    }

    void Update()
    {
        if (!initialized && BuildManager.main != null)
        {
            Initialize();
        }
    }

    private void HandleTowerSelectHoverStatus(int index)
    {
        if (index != towerIndex)
        {
            hoverImage.SetActive(false);
        }
        else
        {
            hoverImage.SetActive(false);
        }
    }

    private void Initialize()
    {
        Tower[] towers = BuildManager.main.towers;
        previewImage = towers[towerIndex].towerPreview;
        hoverImage.SetActive(false);
        initialized = true;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (BuildManager.main.selectedTower == towerIndex)
        {
            BuildManager.main.selectedTower = -1;
            return;
        }
        ShopManager.main.SetSelectedTower(towerIndex);
        hoverImage.SetActive(true);
    }
}
