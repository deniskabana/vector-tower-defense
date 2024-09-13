using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayButton : MonoBehaviour, IPointerClickHandler
{
    public static UIPlayButton main;

    [Header("References")]
    [SerializeField]
    private Sprite spriteStartWave;

    [SerializeField]
    private Sprite spritePlay;

    [SerializeField]
    private Sprite spritePlayDouble;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        if (main == null)
            main = this;

        int speed = LevelManager.main.gameSpeed;

        if (speed == 1)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = spritePlay;
        }
        if (speed == 2)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = spritePlayDouble;
        }

        GetComponent<UnityEngine.UI.Image>().sprite = spriteStartWave;
    }

    public void OnEndWave()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = spriteStartWave;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!LevelManager.main.isWaveInProgress)
        {
            LevelManager.main.StartWave();
        }
        else
        {
            int speed = LevelManager.main.gameSpeed;

            if (speed == 1)
            {
                LevelManager.main.SetGameSpeed(2);
                GetComponent<UnityEngine.UI.Image>().sprite = spritePlayDouble;
            }
            else if (speed == 2)
            {
                LevelManager.main.SetGameSpeed(1);
                GetComponent<UnityEngine.UI.Image>().sprite = spritePlay;
            }
        }

        RefreshSprite();
    }

    public void RefreshSprite()
    {
        if (!LevelManager.main.isWaveInProgress)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = spriteStartWave;
            return;
        }

        int speed = LevelManager.main.gameSpeed;

        if (speed == 1)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = spritePlayDouble;
        }
        else if (speed == 2)
        {
            GetComponent<UnityEngine.UI.Image>().sprite = spritePlay;
        }
    }
}
