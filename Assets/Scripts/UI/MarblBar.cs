using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class MarblBar : MonoBehaviour
{
    public GameObject prefab;
    public Transform parent;

    Dictionary<Team, ElementTeamBar> obj = new Dictionary<Team, ElementTeamBar>();

    List<Team> presentTeam = new List<Team>();

    // Start is called before the first frame update
    void Start()
    {
        GetPresentTeam();
        CreateMarblCount();
    }

    // Update is called once per frame
    void Update()
    {
        Dictionary<Team, int> MarblAndNum = QuickScoreboard.Instance.teamAndBall;

        if (MarblAndNum.Count == 0)
        {
            return;
        }

        foreach (KeyValuePair<Team, ElementTeamBar> marblTeam in obj)
        {
            float newValue = 0;
            try
            {
                newValue = MarblAndNum[marblTeam.Key] * (1000 / QuickScoreboard.Instance.Balls.Length);
            }
            catch { }

            marblTeam.Value.myText.text = MarblAndNum[marblTeam.Key].ToString();
            marblTeam.Value.myRec.sizeDelta = new Vector2(Mathf.Lerp(marblTeam.Value.myRec.sizeDelta.x, newValue, 3 * Time.deltaTime),  marblTeam.Value.myRec.sizeDelta.y);
        }
    }
    


    void GetPresentTeam()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Team _pTeam = p.GetTeam();

            if (!presentTeam.Contains(_pTeam))
            {
                presentTeam.Add(_pTeam);
            }
        }

        if (PhotonNetwork.CurrentRoom.GetHue())
        {
            presentTeam.Add(Team.neutral);
        }
    }

    void CreateMarblCount()
    {
        foreach(Team element in presentTeam)
        {
            GameObject newObj = Instantiate(prefab, parent);
            newObj.GetComponent<Image>().color = MarblGame.GetColor((int) element);

            ElementTeamBar newElement = new ElementTeamBar();
            newElement.myRec = newObj.GetComponent<RectTransform>();
            newElement.myText = newObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            newElement.myRec.sizeDelta = new Vector2(100 * 5, newElement.myRec.sizeDelta.y);

            obj.Add(element, newElement);
        }
    }
}

public class ElementTeamBar
{
    public RectTransform myRec;
    public TextMeshProUGUI myText;
}
