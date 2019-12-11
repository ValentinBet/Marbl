﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : MonoBehaviour
{
    public GameObject explosionFx;

    public float detonationTime = 0.5f;
    public float radius = 7;
    public float upForce = 3;
    public float power = 0.7f;
    public float baseScreenShakePower = 50;
    public float screenShakeDuration = 0.2f;

    public Animator animator;

    private bool isExploding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isExploding && other.CompareTag("Ball") && other.GetComponent<Rigidbody>() != null)
        {
            StartCoroutine(Explode(other));
        }
    }

    private IEnumerator Explode(Collider other)
    {
        isExploding = true;
        animator.SetTrigger("Active");

        yield return new WaitForSeconds(detonationTime);

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider co in colliders)
        {
            if (co.CompareTag("Ball") && co.GetComponent<Rigidbody>() != null)
            {
                if (co.GetComponent<PhotonView>().IsOwnerActive)
                {
                    co.GetComponent<Rigidbody>().AddExplosionForce(power, this.transform.position, radius, upForce, ForceMode.Impulse);
                }
            }
        }


        GameModeManager.Instance.localPlayerObj.GetComponent<CameraPlayer>().InitShakeScreen(baseScreenShakePower * power, screenShakeDuration);

        if (explosionFx != null)
        {
            Destroy(Instantiate(explosionFx, this.transform.position, this.transform.rotation), 2);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

