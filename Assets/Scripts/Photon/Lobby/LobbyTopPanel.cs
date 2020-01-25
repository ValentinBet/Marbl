using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Start()
    {
        if (InputManager.Instance.Inputs != null)
        {
            AudioListener.volume = InputManager.Instance.Inputs.inputs.GeneralVolume;

            AudioManager.Instance.SetPlayingSong(false);
            AudioManager.Instance.SetBackSong(true);
            AudioManager.Instance.backSong.Play();
        }
    }

    #endregion

    public void DisplaySettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }


    public void QuitGame() {
        Application.Quit();
    }
}
