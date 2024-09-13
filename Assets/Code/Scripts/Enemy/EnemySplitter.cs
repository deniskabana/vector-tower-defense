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

    private bool hasPerformed = false;

    public void SplitIfPossible()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (hasPerformed)
            return;

        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject newEnemy = Instantiate(prefabs[i], transform.position, Quaternion.identity);
            EnemyMovement movementScript = GetComponent<EnemyMovement>();
            Transform target = movementScript.targetPoint;
            int pathIndex = movementScript.pathIndex;
            newEnemy.GetComponent<EnemyMovement>().onMapScore = movementScript.onMapScore;
            newEnemy.GetComponent<EnemyMovement>().SetTargetPoint(target, pathIndex);
            EnemySpawner.main.AddCustomEnemy();
        }
        hasPerformed = true;
    }
}
