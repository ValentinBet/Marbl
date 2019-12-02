using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CatapultShoot : MonoBehaviour {

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileHelper;
    [SerializeField] private GameObject chargeBar;
    [SerializeField] private float timeToMaxForce;
    [SerializeField] private float maxForce;
    [SerializeField] private TrajectoryRenderer trajRender;
    [SerializeField] private CinemachineVirtualCamera cineCam;

    private float forceTimer = 0.0f;
    private float pongMultiplier = 1.0f;
    private bool isShooting = true;
    private Vector3 forceVector;
    

    void Update () {
        if (!isShooting)
        {
            if (Input.GetButton("Jump"))
            {
                UpdateForce();
            }
        }
	}

    void UpdateForce()
    {
        if (!chargeBar.activeSelf)
        {
            chargeBar.SetActive(true);
        }
        forceTimer += Time.deltaTime * pongMultiplier;
        Mathf.Clamp(forceTimer, -0.1f, timeToMaxForce + 0.1f);
        if (forceTimer > timeToMaxForce)
        {
            pongMultiplier = -1.0f;
        }
        if (forceTimer < 0.0f)
        {
            pongMultiplier = 1.0f;
        }
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        trajRender.SetVelocity(Mathf.Lerp(0, maxForce, forceTimer / timeToMaxForce));
    }

    void Shoot()
    {
        forceVector = new Vector3(0, Mathf.Cos(projectileHelper.parent.localRotation.x), Mathf.Sin(projectileHelper.parent.localRotation.x));
        GameObject actualProj = Instantiate(projectile, projectileHelper.position, Quaternion.identity);
        actualProj.GetComponent<ProjectileBehavior>().SetForce(forceVector.normalized * Mathf.Lerp(0,maxForce,forceTimer/timeToMaxForce),gameObject.GetComponent<CatapultShoot>());
    }
}
