using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    public int hitpoints = 2;

    [SerializeField]
    private int currencyReward = 50;

    private bool isDead = false;
    public int maxHitpoints;

    public void TakeDamage(int dmg)
    {
        hitpoints -= dmg;

        if (hitpoints <= 0 && !isDead)
        {
            EnemySplitter enemySplitter = GetComponent<EnemySplitter>();
            enemySplitter?.SplitIfPossible();

            EnemySpawner.onEnemyDestroy.Invoke();
            SoundManager.PlayPredefinedSound(SoundType.ENEMY_DEATH);
            isDead = true;
            LevelManager.main.IncreaseCurrency(currencyReward);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        maxHitpoints = hitpoints;
    }
}
