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
        SoundManager.PlayPredefinedSound(SoundType.UI_CLICK);
        SoundManager.PlayPredefinedSound(SoundType.SHOP_PURCHASE_FINISHED);
    }
}
