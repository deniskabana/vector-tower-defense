using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSelectionTurret : MonoBehaviour, IPointerClickHandler
{
    [Header("Attributes")]
    [SerializeField] private int towerIndex;
    [SerializeField] private GameObject hoverImage;

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
            hoverImage.SetActive(true);
        }
    }

    private void Initialize()
    {
        hoverImage.SetActive(false);
        initialized = true;
        if (BuildManager.main.selectedTower == towerIndex) hoverImage.SetActive(true);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        // TODO: Make interaction much more intuitive and user-friendly

        if (BuildManager.main.selectedTower == towerIndex)
        {
            BuildManager.main.selectedTower = -1;
            return;
        }
        ShopManager.main.SetSelectedTower(towerIndex);
        UIManager.main?.PlayUIClickSound();
        hoverImage.SetActive(true);
    }
}
