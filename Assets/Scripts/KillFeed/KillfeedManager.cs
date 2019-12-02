using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillfeedManager : MonoBehaviour
{
    public Transform killFeed;
    List<string> currentKillFeedMessage = new List<string>();
    public GameObject prefabMessage;


    private static KillfeedManager _instance;
    public static KillfeedManager Instance { get { return _instance; } }

    void Awake()
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


    public void AddMessage(string _message)
    {
        GameObject newMessage = Instantiate(prefabMessage, killFeed);
        newMessage.GetComponent<Text>().text = _message;
        Destroy(newMessage, 6f);
    }
}
