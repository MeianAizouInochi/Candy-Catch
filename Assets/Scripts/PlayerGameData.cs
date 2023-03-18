using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGameData
{
    public int TotalScore;

    public float TotalAliveTime;

    public PlayerGameData(int TotalScore, float TotalAliveTime) 
    {
        this.TotalScore = TotalScore;

        this.TotalAliveTime = TotalAliveTime;
    }
}
