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

    void Update()
    {
        if (!initialized && BuildManager.main != null)
        {
            SetActive();
        }
    }

    private void SetActive()
    {
        Tower[] towers = BuildManager.main.towers;
        previewImage = towers[towerIndex].towerPreview;
        hoverImage.GetComponent<UnityEngine.UI.Image>().sprite = previewImage;
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
