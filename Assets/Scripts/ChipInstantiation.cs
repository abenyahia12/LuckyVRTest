using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Procedural;
using System.Reflection;

public class ChipInstantiation : MonoBehaviour
{
    public GameObject chipPrefab;
    public int initialChipCount = 100;
    public int chipStackCount = 10;
    public int chipIncrement = 10;
    public List<Color> colors = new List<Color>();
    public Vector3 offset= Vector3.zero;
    public float rowOffset;
    public float chipOffSet=0.24f;
    public MyPlayer[] myPlayers;
    public Material material;
    private MaterialPropertyBlock block;

    public bool Deactivate = false;
    public bool ADD = false;
    public int playerId = 0;
    public int numberOfStacks = 0;
    void Awake()
    {
        block = new MaterialPropertyBlock();
        for (int i = 0; i < myPlayers.Length; i++)
        {
            GenerateChips(myPlayers[i].Id);
        }
        
    }

    private void GenerateChips(int playerId)
    {
        myPlayers[playerId].ChipStacks = new List<GameObject>();
        myPlayers[playerId].currentRow = 0;
        myPlayers[playerId].currentStackCount = -1;
        for (int i = 0; i < initialChipCount / chipStackCount; i++)
        {
            CreateStack(playerId, i);
        }
        myPlayers[playerId].numberOfChips+= initialChipCount;
    }

    private void CreateStack(int playerId, int i)
    {
        GenerateColor();
        myPlayers[playerId].currentStackCount++;
        if (myPlayers[playerId].currentStackCount >= chipStackCount)
        {
            myPlayers[playerId].currentRow++;
            myPlayers[playerId].currentStackCount = 0;
        }
        myPlayers[playerId].ChipStacks.Add(new GameObject("ChipStack" + i));
        myPlayers[playerId].NumberOfActivatedStacks++;
        myPlayers[playerId].ChipStacks[i].transform.parent = myPlayers[playerId].spawnTransform;
        myPlayers[playerId].ChipStacks[i].transform.localPosition = new Vector3(myPlayers[playerId].currentStackCount * offset.x, myPlayers[playerId].currentStackCount * offset.y, myPlayers[playerId].currentRow * rowOffset);
        myPlayers[playerId].ChipStacks[i].transform.rotation = myPlayers[playerId].spawnTransform.rotation;
        for (int j = 0; j < chipStackCount; j++)
        {
            GameObject chip = CreateChip(chipPrefab, colors[i], myPlayers[playerId].ChipStacks[i].transform);
            chip.transform.localPosition = new Vector3(0, j * chipOffSet, 0);
        }
    }

    public void AddPlayerStacks(int playerId, int numberOfStacks)
    {
        int numberOfActivations = 0;
        int startingNumberOfChipStacksCount = myPlayers[playerId].ChipStacks.Count;
        if (myPlayers[playerId].ChipStacks.Count - myPlayers[playerId].NumberOfActivatedStacks >= numberOfStacks)
        { 
            numberOfActivations = numberOfStacks;
        }
        else
        {
            numberOfActivations = myPlayers[playerId].ChipStacks.Count - myPlayers[playerId].NumberOfActivatedStacks;
        }
        int remainingStacksToAdd = numberOfStacks- numberOfActivations;
   
        if (numberOfActivations > 0)
        {
            //Activate
            for (int i = myPlayers[playerId].NumberOfActivatedStacks; i < myPlayers[playerId].NumberOfActivatedStacks+ numberOfActivations; i++)
            {
                myPlayers[playerId].ChipStacks[i].SetActive(true);
            }
            myPlayers[playerId].NumberOfActivatedStacks = myPlayers[playerId].NumberOfActivatedStacks+numberOfActivations;
        }
        //Generate the rest
        for (int i = startingNumberOfChipStacksCount; i < startingNumberOfChipStacksCount + remainingStacksToAdd; i++)
        {
            CreateStack(playerId, i);
        }
        myPlayers[playerId].numberOfChips += numberOfStacks * chipStackCount;
    }
    public void DeActivatePlayerStacks(int playerId, int numberOfStacks)
    {
        for (int i = 0; i < numberOfStacks; i++)
        {
            myPlayers[playerId].ChipStacks[myPlayers[playerId].NumberOfActivatedStacks - 1 - i].SetActive(false);
            
        }
        myPlayers[playerId].NumberOfActivatedStacks-= numberOfStacks;
        if(myPlayers[playerId].NumberOfActivatedStacks< myPlayers[playerId].multiplesOfstackstoBet)
        { 
            myPlayers[playerId].multiplesOfstackstoBet = myPlayers[playerId].NumberOfActivatedStacks;
        }
        myPlayers[playerId].numberOfChips-= numberOfStacks* chipStackCount;
        if (myPlayers[playerId].numberOfChips==0) 
        {
            AddPlayerStacks(playerId, 100/ chipStackCount);
            myPlayers[playerId].multiplesOfstackstoBet =1;
        }
    }

    void GenerateColor()
    {
        Color newColor = Random.ColorHSV();

        while (colors.Contains(newColor))
        {
            newColor = Random.ColorHSV();
        }

        colors.Add(newColor);
    }
 
    public  GameObject CreateChip(GameObject prefab, Color color, Transform parent)
    {
        GameObject chip = GameObject.Instantiate(prefab, parent);
        block.SetColor("_Color", color);
        Renderer renderer= chip.GetComponent<Renderer>();
        renderer.material = material;
        renderer.SetPropertyBlock(block);
      
        return chip;
    }
    void Update()
    {
        if (Deactivate)
        {
            Deactivate = false;
            DeActivatePlayerStacks(playerId, numberOfStacks);
        }

        if (ADD)
        {
            ADD = false;
            AddPlayerStacks(playerId, numberOfStacks);
        }
    }
}


