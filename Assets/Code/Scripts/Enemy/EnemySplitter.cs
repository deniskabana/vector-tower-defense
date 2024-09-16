using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySplitter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public GameObject[] prefabs;

    [SerializeField]
    private float delayBetweenSplits = 0.08f;

    private bool hasPerformed = false;

    public void SplitIfPossible()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        if (!hasPerformed)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                GameObject newEnemy = Instantiate(
                    prefabs[i],
                    transform.position,
                    Quaternion.identity
                );
                EnemyMovement newEnemyMovement = newEnemy.GetComponent<EnemyMovement>();
                EnemyMovement movementScript = GetComponent<EnemyMovement>();
                Transform target = movementScript.targetPoint;
                int pathIndex = movementScript.pathIndex;
                newEnemyMovement.onMapScore = movementScript.onMapScore;
                newEnemyMovement.SetTargetPoint(target, pathIndex);
                EnemySpawner.main.AddCustomEnemy();
                yield return new WaitForSeconds(delayBetweenSplits);
            }
            hasPerformed = true;
        }
    }
}
