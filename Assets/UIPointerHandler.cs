using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnMouseEnter()
    {

        UIManager.main?.SetUIHoverState(true);
    }

    public void OnMouseExit()
    {
        UIManager.main?.SetUIHoverState(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.main?.SetUIHoverState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.main?.SetUIHoverState(false);
    }
}
