using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    public AudioClip bulletSound;
    private int bulletDamage = 0;
    private Transform target = null;
    private float lifecycleTimer;

    void Update()
    {
        // Make the bullet face it's direction
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity);

        // Delete if surpassed expected lifetime
        lifecycleTimer += Time.deltaTime;
        if (lifecycleTimer >= lifeTime || target == null)
            Destroy(gameObject);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetTargetPosition(Vector2 _targetPosition)
    {
        target = new GameObject().transform;
        target.position = _targetPosition;
    }

    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
        UIManager.main.UIUpdateCurrency(LevelManager.main.currency);
        if (bulletSound != null)
            SoundManager.PlaySoundEffect(bulletSound);
        Destroy(gameObject);
    }
}
