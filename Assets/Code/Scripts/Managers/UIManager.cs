using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    [Header("References")]
    [SerializeField]
    public TMP_Text waveText;

    [SerializeField]
    public TMP_Text currencyText;

    [SerializeField]
    public TMP_Text livesText;

    private string originalWaveText;
    private string originalCurrencyText;
    private string originalLivesText;
    private bool initialized = false;

    public bool isUIHovered = false;

    public bool IsUIHovered()
    {
        return isUIHovered;
    }

    public void SetUIHoverState(bool state)
    {
        isUIHovered = state;
    }

    public void PlayUIClickSound()
    {
        SoundManager.PlaySound(SoundType.UI_CLICK);
    }

    public void ShowNotification(string message)
    {
        // TODO: implement notification system
        Debug.Log("Notification: " + message);
    }

    void Awake()
    {
        main = this;
    }

    private void Start()
    {
        if (main == null)
            main = this;

        if (!initialized)
        {
            originalWaveText = waveText.text;
            originalCurrencyText = currencyText.text;
            originalLivesText = livesText.text;
        }

        initialized = true;
    }

    public void UIInitialize(int wave, int currency, int lives)
    {
        if (!initialized)
            Start();

        UIUpdateWave(wave);
        UIUpdateCurrency(currency);
        UIUpdateLivesLeft(lives);
    }

    public void UIUpdateWave(int wave)
    {
        if (!initialized)
            Start();
        waveText.SetText(originalWaveText + " " + wave);
    }

    public void UIUpdateCurrency(int currency)
    {
        if (!initialized)
            Start();
        currencyText.SetText(originalCurrencyText + " $ " + currency);
    }

    public void UIUpdateLivesLeft(int lives)
    {
        if (!initialized)
            Start();
        livesText.SetText(originalLivesText + " " + lives);
    }
}
