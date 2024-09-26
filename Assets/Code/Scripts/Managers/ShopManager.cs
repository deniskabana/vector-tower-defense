using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public GameObject shopPanel;
    [SerializeField] public TMP_Text nameText;
    [SerializeField] public TMP_Text costText;
    [SerializeField] public TMP_Text fireRateText;
    [SerializeField] public TMP_Text damageText;
    [SerializeField] public TMP_Text buildTimeText;
    [SerializeField] public GameObject noTowerSelected;
    [SerializeField] public GameObject towerDetails;
    [SerializeField] public GameObject purchaseButton;
    [SerializeField] public GameObject notEnoughCashButton;
    [SerializeField] public GameObject previewImage;

    public static ShopManager main;
    private string originalFireRateText;
    private string originalDamageText;
    private string originalBuildTimeText;
    private bool isShopOpen = false;
    private BuildingSpace buildingSpace;
    private BuildingSpace previousBuildingSpace;

    public delegate void OnTowerSelectedAction(int index);
    public event OnTowerSelectedAction OnTowerSelected;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        if (main == null)
            main = this;

        Initialize();
    }

    void Initialize()
    {
        shopPanel.SetActive(false);
        noTowerSelected.SetActive(true);
        towerDetails.SetActive(false);
        LevelManager.main.OnCurrencyChanged += HandleCurrencyChange;

        originalFireRateText = fireRateText.text;
        originalDamageText = damageText.text;
        originalBuildTimeText = buildTimeText.text;
    }

    public void PurchaseTower()
    {
        if (buildingSpace == null)
            return;

        if (LevelManager.main.SpendCurrency(BuildManager.main.GetSelectedTowerPrice()))
        {
            buildingSpace.BuildTower(BuildManager.main.GetSelectedTower().prefab);
            CloseShop();
        }
    }

    public bool IsShopOpen()
    {
        return isShopOpen;
    }

    public void OpenShop()
    {
        if (isShopOpen)
            return;

        HandleCurrencyChange(LevelManager.main.currency);
        SoundManager.PlaySound(SoundType.MENU_SHOW);
        isShopOpen = true;
        shopPanel.SetActive(true);
        SetSelectedTower(0);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        isShopOpen = false;
        SoundManager.PlaySound(SoundType.MENU_CLOSE);
        previousBuildingSpace?.Reset();
        buildingSpace?.Reset();
        previousBuildingSpace = null;
        buildingSpace = null;
        SetSelectedTower(-1);
        HandleCurrencyChange(LevelManager.main.currency);
    }

    public void SetBuildingSpace(BuildingSpace newBuildingSpace)
    {

        if (newBuildingSpace == null)
        {
            previousBuildingSpace = buildingSpace;
            buildingSpace = null;
            previousBuildingSpace?.Reset();
            return;
        }

        previousBuildingSpace = buildingSpace;
        buildingSpace = newBuildingSpace;
        previousBuildingSpace?.Reset();
        RenderBuildingSpacePreview();
    }

    public void SetSelectedTower(int index)
    {
        BuildManager.main.SelectTower(index);
        OnTowerSelected?.Invoke(index);

        if (index == -1)
        {
            noTowerSelected.SetActive(true);
            towerDetails.SetActive(false);
        }
        else
        {
            // Update texts
            Tower selectedTower = BuildManager.main.GetSelectedTower();
            int towerWorth = BuildManager.main.GetSelectedTowerPrice();
            BaseTurret turretScript = selectedTower.prefab.GetComponent<BaseTurret>();

            // UI layout switches
            noTowerSelected.SetActive(false);
            towerDetails.SetActive(true);
            previewImage.GetComponent<UnityEngine.UI.Image>().sprite = selectedTower.towerPreview;
            RenderBuildingSpacePreview();
            // UI text mutations
            nameText.text = selectedTower.name;
            nameText.color = selectedTower.color;
            costText.text = "$ " + towerWorth;
            fireRateText.text = originalFireRateText + " " + turretScript.fireRatePerSecond;
            damageText.text = originalDamageText + " " + turretScript.damage;
            buildTimeText.text = originalBuildTimeText + " " + turretScript.buildTime + "s";
        }

        HandleCurrencyChange(LevelManager.main.currency);
    }

    public void RenderBuildingSpacePreview()
    {
        if (buildingSpace == null)
            return;

        if (BuildManager.main.selectedTower < 0)
            return;

        Tower selectedTower = BuildManager.main.GetSelectedTower();
        BaseTurret turretScript = selectedTower.prefab.GetComponent<BaseTurret>();
        buildingSpace.ShowTowerPreview(selectedTower.towerPreview);
        // UI draw tower range preview
        buildingSpace?.ClearRange();
        buildingSpace?.DrawRange(turretScript.targetRange);
    }

    private void HandleCurrencyChange(int newCurrency)
    {
        if (!isShopOpen)
            return;

        if (newCurrency < BuildManager.main.GetSelectedTowerPrice())
        {
            purchaseButton.SetActive(false);
            notEnoughCashButton.SetActive(true);
        }
        else
        {
            purchaseButton.SetActive(true);
            notEnoughCashButton.SetActive(false);
        }
    }
}
