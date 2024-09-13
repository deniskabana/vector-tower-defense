using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBombAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public BaseTurret baseTurret;

    [SerializeField]
    private GameObject rotatingPoint;

    [Header("Attributes")]
    [SerializeField]
    private float shotForce = 2f;

    [SerializeField]
    private float animationFriction = 5f;

    private Vector3 originalPosition;
    private float currentShotForce;

    void Start()
    {
        originalPosition = rotatingPoint.transform.localPosition;

        if (baseTurret == null)
            return;

        baseTurret.OnShot += HandleShot;
    }

    void OnDestroy()
    {
        if (baseTurret == null)
            return;

        baseTurret.OnShot -= HandleShot;
    }

    void Update()
    {
        // Apply the kickback effect
        rotatingPoint.transform.localPosition =
            originalPosition - rotatingPoint.transform.up * currentShotForce;

        // Gradually reduce the current shot force to simulate friction
        currentShotForce = Mathf.Max(0, currentShotForce - Time.deltaTime * animationFriction);
    }

    private void HandleShot()
    {
        currentShotForce = shotForce / 200;
    }
}
