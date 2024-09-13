using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Animation")]
    [SerializeField]
    private float rotationSpeed = 8f;

    [SerializeField]
    private float scaleSpeed = 1f;

    [SerializeField]
    private float minScale = 0.15f;

    private float maxScale;

    void Start()
    {
        maxScale = transform.localScale.x;
        transform.localScale = new Vector3(maxScale, maxScale, 1);
    }

    void Update()
    {
        Rotate();
        Scale();
    }

    void Scale()
    {
        int hp = gameObject.GetComponent<EnemyHealth>().hitpoints;
        int maxHp = gameObject.GetComponent<EnemyHealth>().maxHitpoints;

        // Calculate the target scale based on hp change
        float targetScale = Mathf.Lerp(minScale, maxScale, (float)hp / maxHp);

        // Smoothly scale down to the target scale using Time.deltaTime
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            new Vector3(targetScale, targetScale, 1),
            Time.deltaTime * 2 * scaleSpeed
        );
    }

    void Rotate()
    {
        // Rotate non-stop
        float velocityBonus = 1 + rb.velocity.magnitude * 0.05f;
        rb.transform.Rotate(0, 0, velocityBonus * rotationSpeed * Time.deltaTime);
    }
}
