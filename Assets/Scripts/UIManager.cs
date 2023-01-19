using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    #region Private Serializable Fields
    [SerializeField]
    private Button IncreaseMultipleOfStacksToBet;
    [SerializeField]
    private Button DecreaseMultipleOfStacksToBet;
    [SerializeField]
    private Button BetGreenButton;
    [SerializeField]
    private Button BetRedButton;
    [SerializeField]
    private Button Bet;
    [SerializeField]
    private Image BetImageColor;
    [SerializeField]
    private TMP_Text TotalOfChips;
    [SerializeField]
    private TMP_Text PlayerName;
    [SerializeField]
    private TMP_Text TotalOfChipsToBet;
    [SerializeField]
    private MyPlayer[] Players;
    [SerializeField]
    private int ControllingPlayerId;
    [SerializeField] 
    private GameObject[] Cameras;
    [SerializeField]
    private LocalGameManager myLocalGameManger;
    #endregion
    private int chipsPerStack = 10;
    const string playerNamePrefKey = "PlayerName";

    public void ConnectPlayerToUI(bool IsMaster)
    {
        if (IsMaster)
        {
            ControllingPlayerId = 0;
        }
        else
        {
            ControllingPlayerId = 1;
        }
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (i==ControllingPlayerId)
            { 
                Cameras[i].gameObject.SetActive(true);
            }
            else
            {
                Cameras[i].gameObject.SetActive(false);
            }
        }
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            Players[ControllingPlayerId].Name= PlayerPrefs.GetString(playerNamePrefKey);
            PlayerName.text = Players[ControllingPlayerId].Name;
        }
        RefreshUITexts();
        Players[ControllingPlayerId].betColor = UtilityClass.BetColor.Red;
        IncreaseMultipleOfStacksToBet.onClick.AddListener(IncreaseStacksToBet);
        DecreaseMultipleOfStacksToBet.onClick.AddListener(DecreaseStacksToBet);
        BetGreenButton.onClick.AddListener(BetGreen);
        BetRedButton.onClick.AddListener(BetRed);
        Bet.onClick.AddListener(ConfirmBet);
    }

    void RefreshUITexts()
    {
        TotalOfChips.text ="Total = "+ Players[ControllingPlayerId].numberOfChips.ToString();
        TotalOfChipsToBet.text = (Players[ControllingPlayerId].multiplesOfstackstoBet * chipsPerStack).ToString();
    }
    public void IncreaseStacksToBet()
    {
        if (Players[ControllingPlayerId].multiplesOfstackstoBet * chipsPerStack < Players[ControllingPlayerId].numberOfChips)
        {
            Players[ControllingPlayerId].multiplesOfstackstoBet++;
            RefreshUITexts();
        }
    }
    public void DecreaseStacksToBet()
    {
        if (Players[ControllingPlayerId].multiplesOfstackstoBet>1)
        {
            Players[ControllingPlayerId].multiplesOfstackstoBet--;
            RefreshUITexts();
        }
    }
    public void BetGreen()
    {
        Players[ControllingPlayerId].betColor = UtilityClass.BetColor.Green;
    }
    public void BetRed()
    {
        Players[ControllingPlayerId].betColor = UtilityClass.BetColor.Red;
    }
    public void ConfirmBet()
    {

        myLocalGameManger.ConfirmBet(Players[ControllingPlayerId].multiplesOfstackstoBet, Players[ControllingPlayerId].betColor);
        SetBetting(false);
    }
    //this blocks user from betting again and wait for his turn or it resets it
    void SetBetting(bool betting)
    {
        IncreaseMultipleOfStacksToBet.interactable= betting;
        DecreaseMultipleOfStacksToBet.interactable = betting;
        BetGreenButton.interactable = betting;
        BetRedButton.interactable = betting;
        Bet.interactable = betting;
        if (betting == false)
        {
            if (Players[ControllingPlayerId].betColor == UtilityClass.BetColor.Red)
                BetImageColor.color = Color.red;
            else
                BetImageColor.color = Color.green;
        }
        else
        {
            BetImageColor.color = Color.white;
        }
    }
    public void ProcessBetResult()
    {
        SetBetting(true);
       
        RefreshUITexts();
    }
}
