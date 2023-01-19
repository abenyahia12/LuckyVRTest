using UnityEngine;
using System.Collections.Generic;

public class ChipInstantiation : MonoBehaviour
{
    #region Public Fields
    public MyPlayer[] m_MyPlayers;
    #endregion

    #region Private Serializable Fields
    [SerializeField]
    private GameObject chipPrefab;
    [SerializeField]
    private int initialChipCount = 100;
    [SerializeField]
    private int chipStackCount = 10;
    [SerializeField]
    private List<Color> colors = new List<Color>();
    [SerializeField]
    private Vector3 offset= Vector3.zero;
    [SerializeField]
    private float rowOffset;
    [SerializeField]
    private float chipOffSet=0.24f;
    [SerializeField]
    private Material material;
    #endregion
    #region Private  Fields
    private MaterialPropertyBlock block;
    #endregion
    private GameObject[] sceneObjects;
    private int seed = 0;
    // this one is the function that starts the Generation
    public void GeneratePlayers()
    {
        //this is made so that both players have the same randomization of colors without syncronizing it through network
        Random.InitState(seed);
        // we will use the same block material property with different colors for the stacks
        block = new MaterialPropertyBlock();
        for (int i = 0; i < m_MyPlayers.Length; i++)
        {
            GenerateChips(m_MyPlayers[i].Id);
        }
    }
    public void ProcessBetResult(bool isRedColor, List<UtilityClass.BetData> betDatas)
    {
        for (int i = 0; i < m_MyPlayers.Length; i++)
        {
             if (betDatas[i].betcolor!= UtilityClass.ToColor(isRedColor))
            {
                DeActivatePlayerStacks(betDatas[i].PlayerIndex, betDatas[i].amount);
            }
            else
            {
                AddPlayerStacks(betDatas[i].PlayerIndex, betDatas[i].amount);
            }
        }
        
    }
    public void AddPlayerStacks(int playerId, int numberOfStacks)
    {
        //
        int numberOfActivations = 0;
        int startingNumberOfChipStacksCount = m_MyPlayers[playerId].ChipStacks.Count;
        //this is done to check if if the number of stacks to activate is less then then the total of deactivated stacks 
        if (m_MyPlayers[playerId].ChipStacks.Count - m_MyPlayers[playerId].NumberOfActivatedStacks >= numberOfStacks)
        {
            numberOfActivations = numberOfStacks;
        }
        else
        {
            numberOfActivations = m_MyPlayers[playerId].ChipStacks.Count - m_MyPlayers[playerId].NumberOfActivatedStacks;
        }
        int remainingStacksToAdd = numberOfStacks - numberOfActivations;
        //Activate the inactive needed stacks to avoid regenerating them which will save us CPU 
        if (numberOfActivations > 0)
        {
  
            for (int i = m_MyPlayers[playerId].NumberOfActivatedStacks; i < m_MyPlayers[playerId].NumberOfActivatedStacks + numberOfActivations; i++)
            {
                m_MyPlayers[playerId].ChipStacks[i].SetActive(true);
            }
            m_MyPlayers[playerId].NumberOfActivatedStacks = m_MyPlayers[playerId].NumberOfActivatedStacks + numberOfActivations;
        }
        //Generate the rest
        for (int i = startingNumberOfChipStacksCount; i < startingNumberOfChipStacksCount + remainingStacksToAdd; i++)
        {
            CreateStack(playerId, i);
        }
        m_MyPlayers[playerId].numberOfChips += numberOfStacks * chipStackCount;
    }
    public void DeActivatePlayerStacks(int playerId, int numberOfStacks)
    {
        //To save some CPU time , I deactivated Player stacks instead of destroying them.
        for (int i = 0; i < numberOfStacks; i++)
        {
            m_MyPlayers[playerId].ChipStacks[m_MyPlayers[playerId].NumberOfActivatedStacks - 1 - i].SetActive(false);
        }
        m_MyPlayers[playerId].NumberOfActivatedStacks -= numberOfStacks;
        //this is made to make sure the user can't bet more than he does have
        if (m_MyPlayers[playerId].NumberOfActivatedStacks < m_MyPlayers[playerId].multiplesOfstackstoBet)
        {
            m_MyPlayers[playerId].multiplesOfstackstoBet = m_MyPlayers[playerId].NumberOfActivatedStacks;
        }
        m_MyPlayers[playerId].numberOfChips -= numberOfStacks * chipStackCount;
        // THIS IS where the PLAYER RESETS , new 100 chips 
        if (m_MyPlayers[playerId].numberOfChips == 0)
        {
            AddPlayerStacks(playerId, 100 / chipStackCount);
            m_MyPlayers[playerId].multiplesOfstackstoBet = 1;
        }
    }
    private void GenerateChips(int playerId)
    {
        m_MyPlayers[playerId].ChipStacks = new List<GameObject>();
        m_MyPlayers[playerId].currentRow = 0;
        m_MyPlayers[playerId].currentStackCount = -1;
        for (int i = 0; i < initialChipCount / chipStackCount; i++)
        {
            CreateStack(playerId, i);
        }
        m_MyPlayers[playerId].numberOfChips+= initialChipCount;
    }
    private void CreateStack(int playerId, int i)
    {
        GenerateColor();
        m_MyPlayers[playerId].currentStackCount++;
        if (m_MyPlayers[playerId].currentStackCount >= chipStackCount)
        {
            m_MyPlayers[playerId].currentRow++;
            m_MyPlayers[playerId].currentStackCount = 0;
        }
        m_MyPlayers[playerId].ChipStacks.Add(new GameObject("ChipStack" + i));
        m_MyPlayers[playerId].NumberOfActivatedStacks++;
        m_MyPlayers[playerId].ChipStacks[i].transform.parent = m_MyPlayers[playerId].spawnTransform;
        m_MyPlayers[playerId].ChipStacks[i].transform.localPosition = new Vector3(m_MyPlayers[playerId].currentStackCount * offset.x, m_MyPlayers[playerId].currentStackCount * offset.y, m_MyPlayers[playerId].currentRow * rowOffset);
        m_MyPlayers[playerId].ChipStacks[i].transform.rotation = m_MyPlayers[playerId].spawnTransform.rotation;
        for (int j = 0; j < chipStackCount; j++)
        {
            GameObject chip = CreateChip(chipPrefab, colors[i], m_MyPlayers[playerId].ChipStacks[i].transform);
            chip.transform.localPosition = new Vector3(0, j * chipOffSet, 0);
        }
    }
    //We need a different color for every new stack
    private void GenerateColor()
    {
        Color newColor = Random.ColorHSV();

        while (colors.Contains(newColor))
        {
            newColor = Random.ColorHSV();
        }

        colors.Add(newColor);
    }
    private GameObject CreateChip(GameObject prefab, Color color, Transform parent)
    {
        GameObject chip = GameObject.Instantiate(prefab, parent);
        block.SetColor("_Color", color);
        Renderer renderer= chip.GetComponent<Renderer>();
        renderer.material = material;
        renderer.SetPropertyBlock(block);
        return chip;
    }

}


