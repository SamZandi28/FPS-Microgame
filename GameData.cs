using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public bool IsDefendingShieldUnlocked;
    public bool IsShockwaveUnlocked;
    public int KillCount;
   // public Vector3 PlayerPosition;

   
    public GameData()
    {
        IsDefendingShieldUnlocked = false;
        IsShockwaveUnlocked = false;
        KillCount = 0;
        //PlayerPosition = Vector3.zero;
    }
}

