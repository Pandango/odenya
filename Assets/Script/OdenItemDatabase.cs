using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdenItemDatabase : MonoBehaviour {
    public List<GameObject> Odens = new List<GameObject>();

    public GameObject getSelectedOdenPrefab(string odenName)
    {
        GameObject selectedOdenObj = null;
        foreach(GameObject oden in Odens)
        {
            if(oden.name.ToLower() == odenName.ToLower())
            {
                selectedOdenObj = oden;
                break;
            }
        }
        return selectedOdenObj;
    }
}
