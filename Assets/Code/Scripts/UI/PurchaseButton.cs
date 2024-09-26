using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PurchaseButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!this.isActiveAndEnabled)
            return;

        ShopManager.main.PurchaseTower();
        SoundManager.PlaySound(SoundType.UI_CLICK);
        SoundManager.PlaySound(SoundType.SHOP_PURCHASE_FINISHED);
    }
}
