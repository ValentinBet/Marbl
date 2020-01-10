using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public Text myText;

    public enum popUpType
    {
        Confirmation,
        Exclamation,
        Question,
        Forbident
    }

    public Animator myAnimator;
    public AudioSource myAudioSource;
    public AudioClip TingSound;

    public List<GameObject> iconObj = new List<GameObject>();

    private static PopupManager _instance;
    public static PopupManager Instance { get { return _instance; } }

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

    public void DisplayPopup(popUpType myType, string myValue)
    {
        myAnimator.SetTrigger("Display");
        myAudioSource.PlayOneShot(TingSound);

        DisplayIcon((int)myType);
        myText.text = myValue;
    }

    void DisplayIcon(int value)
    {
        int i = 0;
        foreach(GameObject icon in iconObj)
        {
            if(i == value)
            {
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);
            }
            i++;
        }
    }
}
