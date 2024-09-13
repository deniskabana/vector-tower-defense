using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class YellowRapidAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform turretBase;

    [SerializeField]
    public BaseTurret baseTurret;

    [Header("Attributes")]
    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float rotationFriction = 10f;

    [SerializeField]
    private float shotSpinImpactForce = 11f;

    [SerializeField]
    private float maxRotationSpeed = 240f;

    private float currentRotationForce;

    void Start()
    {
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
        RotateBase();

        if (currentRotationForce > rotationSpeed)
        {
            currentRotationForce -= Time.deltaTime * rotationFriction;
        }
        else
        {
            currentRotationForce = rotationSpeed;
        }
    }

    private void RotateBase()
    {
        turretBase.transform.Rotate(
            0,
            0,
            (rotationSpeed + Mathf.Min(maxRotationSpeed, currentRotationForce)) * Time.deltaTime
        );
    }

    private void HandleShot()
    {
        currentRotationForce += shotSpinImpactForce;
    }
}
