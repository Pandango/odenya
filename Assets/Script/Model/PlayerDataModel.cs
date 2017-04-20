using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel {

    static public string playerName { get; set; }

    public const int MAXIMUN_SLOT_POT = 15;

    static public int TotalCoin { get; set; }
    static public int TotalPotSlot { get; set; }
    static public int UsedPotSlot { get; set; }

    static public int RemainPotSlot
    {
        get { return TotalPotSlot - UsedPotSlot; }
    }

    static public Vector3[] spawnAddedOdenPositions { get; set; }




}
