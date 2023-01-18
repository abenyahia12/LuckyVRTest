using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    private int chipsPerStack = 10;
    public Button IncreaseMultipleOfStacksToBet;
    public Button DecreaseMultipleOfStacksToBet;
    public Button BetGreenButton;
    public Button BetRedButton;
    public Button Bet;
    public TMP_Text TotalOfChips;
    public TMP_Text PlayerName;
    public TMP_Text TotalOfChipsToBet;
    public MyPlayer[] Players;
    public int ControllingPlayerId;
    public int randomSeed;
    public UtilityClass.BetColor ResultBetColor;
    public ChipInstantiation ChipsManager;
    void Start()
    {
        PlayerName.text = Players[ControllingPlayerId].Name;
        RefreshUITexts();
        Players[ControllingPlayerId].betColor = UtilityClass.BetColor.White;
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
        ProcessBetResult(Players[ControllingPlayerId].betColor);
    }
    public void ProcessBetResult(UtilityClass.BetColor betColor)
    {
        if (Players[ControllingPlayerId].betColor != ResultBetColor)
        {
            ChipsManager.DeActivatePlayerStacks(ControllingPlayerId, Players[ControllingPlayerId].multiplesOfstackstoBet);
        }
        else
        {
            ChipsManager.AddPlayerStacks(ControllingPlayerId, Players[ControllingPlayerId].multiplesOfstackstoBet);
        }
        RefreshUITexts();
    }
}
