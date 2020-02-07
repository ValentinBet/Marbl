using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjManager : MonoBehaviourPunCallbacks
{
    public bool IsPossing = false;
    public bool canPos = false;
    public ObjType currentObj;

    public Animator myAnimator;

    public ButtonObj MineButton;
    public ButtonObj ConeButton;

    public int Mine = 0;
    public int Cone = 0;

    public GameObject prefabPosObj;
    GameObject objPosInGame;

    public enum ObjType
    {
        Mine,
        Cone
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
        if (IsPossing)
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

                    objPosInGame.transform.position = hit.point;
                    Quaternion rotaPoint = Quaternion.LookRotation(hit.normal);
                    rotaPoint *= Quaternion.Euler(90, 0, 0);
                    objPosInGame.transform.rotation = rotaPoint;



                    if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1))
                    {
                        switch (currentObj)
                        {
                            case ObjType.Mine:
                                if(Mine > 0)
                                {
                                    Mine--;

                                    Quaternion rota = Quaternion.LookRotation(hit.normal);
                                    rota *= Quaternion.Euler(90, 0, 0);

                                    PhotonNetwork.Instantiate("Mine", hit.point, rota);

                                    if(Mine == 0)
                                    {
                                        SetMine();
                                    }
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
            }
        }
        else
        {
            objPosInGame.SetActive(false);
        }
    }

    public void GiveRandomObj()
    {
        int rand = Random.Range(0, 1);

        switch (rand)
        {
            case 0:
                AddObj(ObjType.Mine, 3);
                break;

            case 1:
                AddObj(ObjType.Cone, 1);
                break;
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

            case ObjType.Cone:
                Cone += Cone;
                myAnimator.SetTrigger("Cone");
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

        if (Cone > 0)
        {
            ConeButton.SetValue(Cone);
            ConeButton.gameObject.SetActive(true);
        }
        else
        {
            ConeButton.gameObject.SetActive(false);
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

    public void SetCone()
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
            currentObj = ObjType.Cone;
            ConeButton.GetComponent<Image>().color = Color.red;
        }
    }

    void SetColorWhite()
    {
        MineButton.GetComponent<Image>().color = Color.white;
        ConeButton.GetComponent<Image>().color = Color.white;
    }
}
