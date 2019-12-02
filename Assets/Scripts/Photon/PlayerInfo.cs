using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    PhotonView PV;

    public float score = 0;
    public team myTeam;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            int index = 0;
            switch (myTeam)
            {
                case team.Team1:
                    index = 0;
                    break;

                case team.Team2:
                    index = 1;
                    break;

                case team.Team3:
                    index = 2;
                    break;

                case team.Team4:
                    index = 3;
                    break;
            }

            /*
            Transform parentPos = GameModeManager.Instance.listPos[index];

            foreach(Transform element in parentPos)
            {
                print("test");
                GameObject c = PhotonNetwork.Instantiate("Ball", element.position, Quaternion.identity, 0);
            }
            */
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

    }

    public enum team {
        Team1,
        Team2,
        Team3,
        Team4
    };
}
