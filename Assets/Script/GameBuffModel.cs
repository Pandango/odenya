using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuffModel : MonoBehaviour {

    public string[] buffList = new string[]
    {
        "EXTRA_MONEY",
        "ITEM_PRICE_INCREASE",
        "SLOT_INCREASE"
    };    

    public string getBuff(int index)
    {
        return buffList[index];
    }

    public string getRandomBuff()
    {
        int ranIndex = Random.RandomRange(0, 100);

        if(ranIndex == 1)
        {
            return buffList[2];
        }
        else
        {
            return buffList[0];
        }

    }
}
