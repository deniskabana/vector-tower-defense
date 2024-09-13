using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField]
    private float moveSpeed = 10f;

    private Transform targetPoint;
    private int pathIndex = 0;

    private int onMapScore = 0;

    void Start()
    {
        targetPoint = LevelManager.main.path[0];
    }

    void Update()
    {
        onMapScore++;

        if (Vector2.Distance(targetPoint.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= LevelManager.main.path.Length)
            {
                LevelManager.main.LoseLife();
                EnemySpawner.onEnemyDestroy.Invoke();
                if (gameObject != null)
                    Destroy(gameObject);
                return;
            }
            else
            {
                targetPoint = LevelManager.main.path[pathIndex];
            }
        }
    }

    public int GetOnMapScore()
    {
        return onMapScore;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed / 10;
    }
}
