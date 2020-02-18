using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class ObjManager : MonoBehaviourPunCallbacks
{
    public bool IsPossing = false;
    public bool canPos = false;
    public ObjType currentObj;

    public Animator myAnimator;

    public Transform parentButton;
    public ButtonObj MineButton;
    public ButtonObj HoloButton;
    public ButtonObj ShockButton;

    public int Mine = 0;
    public int Holo = 0;
    public int Shockwave = 0;

    public GameObject checkPingPrefab;
    public GameObject prefabPosObj;
    GameObject objPosInGame;

    LocalPlayerManager myLocalPlayerManager;

    public enum ObjType
    {
        Mine,
        Holo,
        Shock
    }

    private static ObjManager _instance;
    public static ObjManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        objPosInGame = Instantiate(prefabPosObj);
        objPosInGame.SetActive(false);

        Refresh();
    }

    private void Update()
    {
        if (myLocalPlayerManager == null)
        {
            myLocalPlayerManager = GameModeManager.Instance.localPlayerObj.GetComponent<LocalPlayerManager>();
        }

        bool forceCam = PhotonNetwork.CurrentRoom.GetForceCam();

        parentButton.gameObject.SetActive(!forceCam);


        if (IsPossing && !forceCam)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.layer == 15)
                {
                    canPos = false;

                    if (objPosInGame.activeSelf)
                    {
                        objPosInGame.SetActive(false);
                    }
                }
                else
                {
                    canPos = true;
                    if (!objPosInGame.activeSelf)
                    {
                        objPosInGame.SetActive(true);
                    }

                    objPosInGame.transform.position = hit.point + objPosInGame.transform.forward * 0.02f;
                    Quaternion rotaPoint = Quaternion.LookRotation(hit.normal);
                    objPosInGame.transform.rotation = rotaPoint;



                    if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1))
                    {
                        switch (currentObj)
                        {
                            case ObjType.Mine:
                                if (Mine > 0)
                                {
                                    Mine--;

                                    Quaternion rota = Quaternion.LookRotation(hit.normal);
                                    rota *= Quaternion.Euler(90, 0, 0);

                                    myLocalPlayerManager.SendObj(PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.GetTeam(), hit.point, rota, "Mine");

                                    if (Mine == 0)
                                    {
                                        SetMine();
                                    }

                                    SpawnCheckPing(hit.point + Vector3.up * 0.04f, rota);
                                }
                                else
                                {
                                    IsPossing = false;
                                }
                                Refresh();
                                break;

                            case ObjType.Holo:
                                if (Holo > 0)
                                {
                                    Holo--;

                                    Quaternion rota = Quaternion.LookRotation(hit.normal);
                                    rota *= Quaternion.Euler(90, 0, 0);

                                    string objName = "HoloBlue";

                                    switch (GameModeManager.Instance.localPlayerTeam)
                                    {
                                        case Team.blue:
                                            objName = "HoloBlue";
                                            break;

                                        case Team.green:
                                            objName = "HoloGreen";
                                            break;

                                        case Team.red:
                                            objName = "HoloRed";
                                            break;

                                        case Team.yellow:
                                            //PhotonNetwork.Instantiate("HoloYellow", hit.point + Vector3.up * 0.09f, rota);
                                            objName = "HoloYellow";
                                            break;
                                    }

                                    myLocalPlayerManager.SendObj(PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.GetTeam(), hit.point + Vector3.up * 0.09f, rota, objName);

                                    if (Holo == 0)
                                    {
                                        SetHolo();
                                    }

                                    SpawnCheckPing(hit.point + Vector3.up * 0.04f, rota);
                                }
                                else
                                {
                                    IsPossing = false;
                                }
                                Refresh();
                                break;

                            case ObjType.Shock:
                                if (Shockwave > 0)
                                {
                                    Shockwave--;

                                    Quaternion rota = Quaternion.LookRotation(hit.normal);

                                    myLocalPlayerManager.SendObj(PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.GetTeam(), hit.point, rota, "ShockWave");

                                    if (Shockwave == 0)
                                    {
                                        SetShock();
                                    }

                                    rota *= Quaternion.Euler(90, 0, 0);
                                    SpawnCheckPing(hit.point + Vector3.up * 0.04f, rota);
                                }
                                else
                                {
                                    IsPossing = false;
                                }
                                Refresh();
                                break;
                        }
                    }
                }

                if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton2))
                {
                    switch (currentObj)
                    {
                        case ObjType.Mine:
                            SetMine();
                            break;

                        case ObjType.Holo:
                            SetHolo();
                            break;

                        case ObjType.Shock:
                            SetShock();
                            break;
                    }
                }
            }
            else
            {
                objPosInGame.SetActive(false);
            }
        }
        else
        {
            objPosInGame.SetActive(false);
        }
    }


    void SpawnCheckPing(Vector3 _pos, Quaternion _direction)
    {
        Instantiate(checkPingPrefab, _pos, _direction);
    }

    public void GiveRandomObj()
    {
        int rand = Random.Range(0, 21);

        if (rand < 10)
        {
            AddObj(ObjType.Mine, 3);
            return;
        }

        if (rand < 15)
        {
            AddObj(ObjType.Holo, 1);
            return;
        }

        if (rand > 15)
        {
            AddObj(ObjType.Shock, 1);
            return;
        }
    }

    public void AddObj(ObjType obj, int value)
    {
        switch (obj)
        {
            case ObjType.Mine:
                Mine += value;
                myAnimator.SetTrigger("Mine");
                break;

            case ObjType.Holo:
                Holo += value;
                myAnimator.SetTrigger("Holo");
                break;

            case ObjType.Shock:
                Shockwave += value;
                myAnimator.SetTrigger("Shockwave");
                break;
        }

        Refresh();
    }


    public void Refresh()
    {
        if(Mine > 0)
        {
            MineButton.SetValue(Mine);
            MineButton.gameObject.SetActive(true);
        }
        else
        {
            MineButton.gameObject.SetActive(false);
        }

        if (Holo > 0)
        {
            HoloButton.SetValue(Holo);
            HoloButton.gameObject.SetActive(true);
        }
        else
        {
            HoloButton.gameObject.SetActive(false);
        }

        if (Shockwave > 0)
        {
            ShockButton.SetValue(Shockwave);
            ShockButton.gameObject.SetActive(true);
        }
        else
        {
            ShockButton.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (PhotonNetwork.LocalPlayer.GetPlayerGift())
            {
                PhotonNetwork.LocalPlayer.SetPlayerGetGift(false);
                GiveRandomObj();
            }
        }
    }

    public void SetMine()
    {
        SetColorWhite();
        if (IsPossing)
        {
            IsPossing = false;
            return;
        }
        else
        {
            IsPossing = true;
            currentObj = ObjType.Mine;
            MineButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void SetHolo()
    {
        SetColorWhite();
        if (IsPossing)
        {
            IsPossing = false;
            return;
        }
        else
        {
            IsPossing = true;
            currentObj = ObjType.Holo;
            HoloButton.GetComponent<Image>().color = Color.red;
        }
    }

    public void SetShock()
    {
        SetColorWhite();
        if (IsPossing)
        {
            IsPossing = false;
            return;
        }
        else
        {
            IsPossing = true;
            currentObj = ObjType.Shock;
            ShockButton.GetComponent<Image>().color = Color.red;
        }
    }

    void SetColorWhite()
    {
        MineButton.GetComponent<Image>().color = Color.white;
        HoloButton.GetComponent<Image>().color = Color.white;
        ShockButton.GetComponent<Image>().color = Color.white;
    }
}
