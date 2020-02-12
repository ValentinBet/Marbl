using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModesPanelControl : MonoBehaviour
{
    [Header("Hue")]
    [SerializeField] private GameObject hueCircle;
    [Header("DeathMatch")]
    [SerializeField] private GameObject dmCircle;
    [Header("Koth")]
    [SerializeField] private GameObject kothCircle;

    [Header("Lock")]
    [SerializeField] private List<GameObject> ListPadlock = new List<GameObject>();

    private void Start()
    {
        print(PlayerPrefs.GetInt("HavePlayedHue"));
        if (PlayerPrefs.GetInt("HavePlayedHue") == 1)
        {
            UnlockGamemodes();
        }
    }

    private void UnlockGamemodes()
    {
        foreach (GameObject item in ListPadlock)
        {
            item.SetActive(false);
        }
    }
}
