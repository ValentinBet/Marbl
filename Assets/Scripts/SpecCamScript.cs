using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecCamScript : MonoBehaviourPunCallbacks/*, IPunObservable*/
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private PhotonView pv;

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraManager.Instance.CamSpecNetwork = transform;
    }
}
