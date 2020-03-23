using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mb_SkinManager : MonoBehaviour
{
    private static Mb_SkinManager _instance;
    public Sc_Skininfos playerSkinScriptable;

    public static Mb_SkinManager Instance { get { return _instance; } }


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
        DontDestroyOnLoad(this.gameObject);
}


}
