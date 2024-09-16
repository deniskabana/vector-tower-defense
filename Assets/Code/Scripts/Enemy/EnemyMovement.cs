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
    public float moveSpeed = 10f;

    public Transform targetPoint;
    public int pathIndex = 0;

    public float onMapScore = 0;

    void Start()
    {
        if (targetPoint == null)
            targetPoint = LevelManager.main.path[0];
    }

    public void SetTargetPoint(Transform newTargetPoint, int newPathIndex)
    {
        targetPoint = newTargetPoint;
        pathIndex = newPathIndex;
    }

    void Update()
    {
        onMapScore += moveSpeed;

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

    public float GetOnMapScore()
    {
        return onMapScore;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed / 10;
    }
}
