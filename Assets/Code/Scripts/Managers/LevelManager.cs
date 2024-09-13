using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform startPoint;
    public Transform[] path;

    public int currency;
    public int startingCurrency;
    public int lives;
    public int gameSpeed = 1;
    public int currentWave = 0;
    public bool isWaveInProgress = false;
    public delegate void OnCurrencyChangedAction(int newCurrency);
    public event OnCurrencyChangedAction OnCurrencyChanged;
    public delegate void OnLivesChangedAction(int newLives);
    public event OnLivesChangedAction OnLivesChanged;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        if (main == null)
            main = this;

        currency = startingCurrency;
        UIManager.main.UIInitialize(currentWave, currency, lives);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void StartWave()
    {
        currentWave++;
        EnemySpawner.main.StartSpawning();
        isWaveInProgress = true;

        UIManager.main.UIUpdateWave(currentWave);
    }

    public void EndWave()
    {
        isWaveInProgress = false;
        UIPlayButton.main.OnEndWave();
    }

    public int SetGameSpeed(int speed)
    {
        gameSpeed = speed;
        Time.timeScale = gameSpeed;
        return gameSpeed;
    }

    public void GainLife()
    {
        lives++;
        UIManager.main.UIUpdateLivesLeft(lives);
        OnLivesChanged?.Invoke(lives);
    }

    public void LoseLife()
    {
        lives--;
        UIManager.main.UIUpdateLivesLeft(lives);
        OnLivesChanged?.Invoke(lives);

        if (lives <= 0)
        {
            // TODO: GameManager.main.GameOver();
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        UIManager.main.UIUpdateCurrency(currency);
        OnCurrencyChanged?.Invoke(currency);
    }

    public bool SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            UIManager.main.UIUpdateCurrency(currency);
            OnCurrencyChanged?.Invoke(currency);
            return true;
        }
        else
        {
            return false;
        }
    }
}
