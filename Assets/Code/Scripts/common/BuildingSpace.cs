using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSpace : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject spriteObj;

    [SerializeField]
    private GameObject turretPreviewSprite;

    [SerializeField]
    private Sprite hoverSprite;

    [SerializeField]
    private Color hoverColor;

    private GameObject tower;

    private Sprite originalSprite;

    private int currentTowerWorth = 0;
    private LineRenderer lineRenderer;

    void Start()
    {
        originalSprite = spriteObj.GetComponent<SpriteRenderer>().sprite;
        turretPreviewSprite.SetActive(false);
        SetupLineRenderer();
    }

    void OnMouseDown()
    {
        if (tower != null)
        {
            // TODO: Selling, upgrading, etc.
            return;
        }

        if (ShopManager.main.IsShopOpen())
        {
            // TODO: ShopManager.main.CloseShop(); - currently registers clicks under UI
            return;
        }

        ShopManager.main.OpenShop();
        ShopManager.main.SetBuildingSpace(this);
        spriteObj.GetComponent<SpriteRenderer>().sprite = hoverSprite;
    }

    private void SetupLineRenderer()
    {
        if (lineRenderer != null)
            return;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 50;
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 1, 1, 0.1f);
        lineRenderer.endColor = new Color(1, 1, 1, 0.1f);
    }

    public void ShowTowerPreview(Sprite preview)
    {
        turretPreviewSprite.GetComponent<SpriteRenderer>().sprite = preview;
        turretPreviewSprite.SetActive(true);
    }

    public void BuildTower(GameObject towerPrefab)
    {
        if (tower != null)
            return;

        tower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        spriteObj.GetComponent<SpriteRenderer>().sprite = originalSprite;
        currentTowerWorth = towerPrefab.GetComponent<BaseTurret>().towerPrice;
    }

    public void Reset()
    {
        spriteObj.GetComponent<SpriteRenderer>().sprite = originalSprite;
        turretPreviewSprite.GetComponent<SpriteRenderer>().sprite = null;
        turretPreviewSprite.SetActive(false);
        ClearRange();
    }

    public void DeleteTower()
    {
        Destroy(tower);
        tower = null;
    }

    public void DrawRange(float targetRange)
    {
        float deltaTheta = (2f * Mathf.PI) / 50;
        float theta = 0f;

        lineRenderer.positionCount = 50 + 1;
        for (int i = 0; i < 50 + 1; i++)
        {
            float x = targetRange * Mathf.Cos(theta);
            float y = targetRange * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    public void ClearRange()
    {
        lineRenderer.positionCount = 0;
    }
}
