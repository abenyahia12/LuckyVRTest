using System.Collections;
using System.Collections.Generic;
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
    public BetColor betColor = BetColor.White;
    public List<GameObject> ChipStacks;
    public int Id = 0;
    public string Name;
    //UI Information
    public int multiplesOfstackstoBet=1;
   
    void Awake()
    { 
    }

}
