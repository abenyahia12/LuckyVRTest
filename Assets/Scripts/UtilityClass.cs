using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityClass 
{

    public enum BetColor {Red,Green};
    [System.Serializable]
    public struct BetData { public int PlayerIndex;public  int amount; public BetColor betcolor; }
    public static bool IsRed(BetColor color)
    {
        return (color == BetColor.Red);
    }
    public static BetColor  ToColor(bool boolean)
    {
        if (boolean)
            return BetColor.Red;
        else
            return BetColor.Green;
    }

}
