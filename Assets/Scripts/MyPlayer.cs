using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using System.Collections;

using UnityEngine;
using static UtilityClass;

public class MyPlayer : MonoBehaviour
{
    //Chips Information
    public int numberOfChips = 0;
    public int NumberOfActivatedStacks;
    public Transform spawnTransform;
    public int currentRow = 0;
    public int currentStackCount = -1;
    public BetColor betColor = BetColor.Red;
    public List<GameObject> ChipStacks;
    public int multiplesOfstackstoBet = 1;
    //UI Information
    public int Id = 0;
    public string Name;
   
}
