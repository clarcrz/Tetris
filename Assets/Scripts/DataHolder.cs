using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour { 

    public static int globalScore = 0;
    public static int globalLevel = 0;
    public static int globalLines = 0;

    public void SetScoreData (int score, int level, int lines)
    {
        globalScore = score;
        globalLevel = level;
        globalLines = lines;
    }

    public Vector3 GetScoreData ()
    {
        return new Vector3(globalScore, globalLevel, globalLines);
    }
}
