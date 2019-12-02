using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNBallControl : MonoBehaviour
{
    public bool isSelected;
    private PUNBallMovement ballMovement;
    private PhotonView photonView;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        ballMovement = GetComponent<PUNBallMovement>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine || !ballMovement.controllable)
        {
            return;
        }
    }
    [PunRPC]
    public void DestroyBalls()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        collider.enabled = false;
        renderer.enabled = false;
        rigidbody.useGravity = false;
        ballMovement.controllable = false;

        if (photonView.IsMine)
        {
            StartCoroutine("WaitForRespawn");
        }
    }

    public void SetOutline(bool value, Color? c = null)
    {
        Outline _outline = this.GetComponent<Outline>();

        _outline.enabled = value;
        _outline.OutlineColor = c ?? Color.white;

        isSelected = _outline.enabled;
    }
}
