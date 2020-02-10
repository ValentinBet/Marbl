using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostPanel : MonoBehaviourPunCallbacks
{
    [Header("Top")]
    public RectTransform currentChoice;
    public Text currentText;
    public GameObject mapTop;
    List<Text> oldTexts = new List<Text>();
    List<Text> removeTexts = new List<Text>();

    public RectTransform line;
    public List<GameObject> ChoiceObject = new List<GameObject>();
    [Header("panel")]
    public GameObject PlayersList;
    public GameObject GameMode;
    public GameObject Map;
    public GameObject Custom;
    public GameObject topPanel;
    public GameObject PlayerHostPanel;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;


    private void Start()
    {
        line.position = currentChoice.position;
    }

    // Update is called once per frame
    void Update()
    {
        line.position = Vector2.MoveTowards(line.position, currentChoice.position, 3000 * Time.deltaTime);

        removeTexts.Clear();
        foreach (Text oldText in oldTexts)
        {
            oldText.color = Color.Lerp(oldText.color, normalColor, Mathf.PingPong(Time.time, 3));

            if (oldText.color == normalColor)
            {
                removeTexts.Add(oldText);
            }
        }

        foreach (Text text in removeTexts)
        {
            oldTexts.Remove(text);
        }

        currentText.color = Color.Lerp(currentText.color, selectedColor, Mathf.PingPong(Time.time, 3));
    }


    public void ResetToDefaultRoomPanel(bool Init = false) //mean reset to default display
    {
        PlayersList.SetActive(true);
        GameMode.SetActive(false);
        Map.SetActive(false);
        Custom.SetActive(false);

        if (PhotonNetwork.IsMasterClient) PlayerHostPanel.SetActive(true);

        if (Init) // If just joined Room
        {
            for (int i = 1; i < ChoiceObject.Count; i++) // excluse player choice
            {
                ChoiceObject[i].GetComponent<Text>().color = normalColor;
            }

            // ChoiceObject[0] == player
            currentChoice = ChoiceObject[0].GetComponent<RectTransform>();
            currentText = ChoiceObject[0].GetComponent<Text>();
        }
    }

    public void SetChoice(RectTransform element)
    {
        Text newtext = element.GetComponent<Text>();

        if (currentChoice != newtext)
        {
            oldTexts.Add(currentText);
            currentChoice = element;
            currentText = newtext;
            PlayerHostPanel.SetActive(true);
            switch (newtext.text)
            {
                case "Players":
                    ResetToDefaultRoomPanel();
                    break;

                case "Gamemode":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(true);
                    Map.SetActive(false);
                    Custom.SetActive(false);
                    PlayerHostPanel.SetActive(false);
                    break;

                case "Map":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(true);
                    Custom.SetActive(false);
                    PlayerHostPanel.SetActive(false);

                    RoomScripts.Instance.Refresh();
                    RoomScripts.Instance.RefreshMaps();
                    break;

                case "Custom":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(false);
                    Custom.SetActive(true);
                    PlayerHostPanel.SetActive(false);
                    break;
            }
        }
    }
}
