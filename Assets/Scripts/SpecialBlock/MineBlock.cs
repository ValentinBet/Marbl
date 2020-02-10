using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBlock : MonoBehaviour
{
    public GameObject explosionFx;
    public AudioSource audioSource;

    public Transform posMark;
    public GameObject prefabMark;

    public AudioClip explosion;

    public float radius = 4;
    public float upForce = 1;
    public float power = 0.4f;
    public float baseScreenShakePower = 20;
    public float screenShakeDuration = 0.2f;

    private bool isExploding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isExploding && other.GetComponent<Rigidbody>() != null)
        {
            Explode(other);
        }
    }

    private void Explode(Collider other)
    {
        isExploding = true;

        if (explosion != null)
        {
            audioSource.PlayOneShot(explosion);
        }
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);

        foreach (Collider co in colliders)
        {
            if (co.CompareTag("Ball") && co.GetComponent<Rigidbody>() != null)
            {
                if (co.GetComponent<PhotonView>().IsMine)
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

        GameObject mark = Instantiate(prefabMark);
        mark.transform.position = posMark.position;
        mark.transform.eulerAngles = new Vector3(90, Random.Range(0, 360), 0);

        gameObject.SetActive(false);
        Destroy(transform.parent.gameObject,2);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

