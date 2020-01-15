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
        CameraManager.Instance.CameraSpec = transform;
    }

    //public void FixedUpdate()
    //{
    //    if (!pv.IsMine)
    //    {
    //        Debug.Log(networkPosition + " - POS ");
    //        Debug.Log(networkRotation + " - Rot ");
    //        transform.position = networkPosition;
    //        transform.rotation = networkRotation;
    //    }
    //}
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(this.transform.position);
    //        stream.SendNext(this.transform.rotation);
    //    }
    //    else
    //    {
    //        networkPosition = (Vector3)stream.ReceiveNext();
    //        networkRotation = (Quaternion)stream.ReceiveNext();
    //    }
    //}
}
