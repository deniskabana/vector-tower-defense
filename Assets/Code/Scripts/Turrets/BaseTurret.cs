using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum BulletTargetStyle
{
    AngularTowardsEnemy,
    AngularFromCenter,
}
public class BaseTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] firingPoints;

    [Header("Attributes")]
    [SerializeField] public float targetRange = 5f;
    [SerializeField] public float rotationSpeed = 400f;
    [SerializeField] public float fireRatePerSecond = 1f;
    [SerializeField] public int towerPrice = 65;
    [SerializeField] public float buildTime = 1;
    [SerializeField] public int damage = 1;
    [SerializeField] public BulletTargetStyle bulletTargetStyle = BulletTargetStyle.AngularTowardsEnemy;

    [Header("SFX")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip bulletSound;

    private Transform target;
    private float timeUntilFire;

    public delegate void ShotAction();
    public event ShotAction OnShot;

    void Update()
    {
        RotateTowardsTarget();

        if (target == null)
        {
            FindTarget();
        }

        if (!CheckTargetInRange())
        {
            target = null;
        }

        if (timeUntilFire <= 0f)
        {
            if (target == null)
                return;
            Shoot();
            FindTarget();
            timeUntilFire = 1f / fireRatePerSecond;
        }
        else
        {
            timeUntilFire -= Time.deltaTime;
        }
    }

    private void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            targetRange,
            enemyLayerMask
        );

        if (colliders.Length > 0)
        {
            float highestScore = float.MinValue;
            Transform bestTarget = null;

            foreach (Collider2D collider in colliders)
            {
                EnemyMovement enemyMovement = collider.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    float score = enemyMovement.GetOnMapScore();
                    if (score > highestScore)
                    {
                        highestScore = score;
                        bestTarget = collider.transform;
                    }
                }
            }

            target = bestTarget;
        }
    }

    private bool CheckTargetInRange()
    {
        if (target == null)
            return false;

        return Vector2.Distance(target.position, transform.position) <= targetRange;
    }

    private void RotateTowardsTarget()
    {
        if (turretRotationPoint == null || target == null)
            return;

        float angle =
            Mathf.Atan2(
                target.position.y - transform.position.y,
                target.position.x - transform.position.x
            ) * Mathf.Rad2Deg
            - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Shoot()
    {
        foreach (Transform firingPoint in firingPoints)
        {
            GameObject bulletObj = Instantiate(
                bulletPrefab,
                firingPoint.position,
                Quaternion.identity
            );
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();

            switch (bulletTargetStyle)
            {
                case BulletTargetStyle.AngularTowardsEnemy:
                    bulletScript.SetTarget(target);
                    break;
                case BulletTargetStyle.AngularFromCenter:
                    Vector2 direction = (
                        firingPoint.position - turretRotationPoint.position
                    ).normalized;
                    Vector2 farAwayTarget =
                        (Vector2)turretRotationPoint.position + direction * 100f; // away in the same direction
                    bulletScript.SetTargetPosition(farAwayTarget);
                    break;
            }
            bulletScript.SetDamage(damage);
            bulletScript.bulletSound = bulletSound;
        }

        OnShot?.Invoke();
        if (shootSound != null)
            SoundManager.PlayAudioClip(shootSound);
    }
}
