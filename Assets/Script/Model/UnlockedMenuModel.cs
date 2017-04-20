using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedMenuModel : MonoBehaviour {

    static Dictionary<string, bool> _UnlockMenuModel = new Dictionary<string, bool>()
    {
        {"BoiledEgg", true },
        {"KannyakuNori", true },
        {"Chikuwa", false },
        {"Carrot", false },
        {"BoiledRadish", false },
        {"Corn", false },
        {"CrabStick", false },
        {"RolledCabbage", false },
        {"Sausage", false },
        {"NarutoMaki", false },
        {"Dango", false }
    };

    static public void UpdateSpecificUnlockMenu(string odenKeyName, bool isUnlocked)
    {
        _UnlockMenuModel[odenKeyName] = isUnlocked;
    }

    static public Dictionary<string, bool> UnlockMenuList
    {
        get { return _UnlockMenuModel; }
    }
}
