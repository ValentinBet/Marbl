using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewModifierProfile",menuName ="MARBL/ModifierProfile",order =0)]
public class ModifierProfile:ScriptableObject
{
    [Header("General")]
    private Dictionary<string, ModifierProperty> properties = new Dictionary<string, ModifierProperty>();
    public ModifierProperty[] ModifierProperties;

    public void FormDictionary()
    {
        //ATTENTION PAS DE VERIFICATION AUX DOUBLONS /!\
        properties.Clear();
        for (int i = 0;i < ModifierProperties.Length;i++)
        {
            properties.Add(ModifierProperties[i].formatedName(), ModifierProperties[i]);
        }
    }

    public float GetProperty(string name)
    {
        return properties[name].value;
    }

    /*    public ModifierProperty marblesPerPlayer;
        public ModifierProperty rounds;
        public ModifierProperty turnDuration;
    //    public ModifierProperty LaunchPower;

        [Header("DeathMatch")]
        public ModifierProperty marbleKill;*/
}

[System.Serializable]
public struct ModifierProperty
{
    public string name;
    public float value;

    public string formatedName()
    {
        string formStr = name;
        for (int i = 0; i < formStr.Length;i++)
        {
            if (formStr[i] == ' ')
            {
                formStr.Remove(i,1);
                i--;
            }
        }
        return formStr;
    }
    /*
    public float[] values; //Toutes les valeurs possibles
    public string[] valuesTexts; //Textes affiliés représentant chacunes des options
    public int defaultIndex; //L'index choisi par défaut
    */
}
