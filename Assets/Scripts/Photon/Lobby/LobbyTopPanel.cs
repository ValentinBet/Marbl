using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class LobbyTopPanel : MonoBehaviourPunCallbacks
{
    private readonly string connectionStatusMessage = "    Connection Status: ";

    [Header("UI References")]
    public Text ConnectionStatusText;

    public Text Region;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }

    public override void OnConnectedToMaster()
    {
        Region.text = "Region : " + PhotonNetwork.CloudRegion.ToUpper();
    }

    #endregion
}
