using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UtilityClass;

public class LocalGameManager : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    [SerializeField]
    private ChipInstantiation m_ChipInstantiation;
    [SerializeField]
    private UIManager m_UIManager;
    [SerializeField]
    private MyGameManager m_myGameManager;
    [SerializeField]
    private Renderer m_BettingObjectRenderer;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }
    void StartGame()
    {
        m_ChipInstantiation = GetComponent<ChipInstantiation>();
        m_ChipInstantiation.GeneratePlayers();
        m_UIManager = GetComponent<UIManager>();
        m_UIManager.ConnectPlayerToUI(PhotonNetwork.IsMasterClient);
        m_myGameManager.StartGame();
    }
    public void ConfirmBet(int amount, UtilityClass.BetColor betColor)
    {
        m_myGameManager.BetRPCCall(PhotonNetwork.IsMasterClient, amount, UtilityClass.IsRed(betColor));
    }
    public void ProcessResult(bool betResultIsRed, List<UtilityClass.BetData> betDatas)
    {
        List<UtilityClass.BetData> tempBetData = betDatas;
        //this is taking care of displaying which color is chosen on the bettingObect
        if (betResultIsRed)
        {
            m_BettingObjectRenderer.material.SetColor("_Color", Color.red);
        }
        else
        {
            m_BettingObjectRenderer.material.SetColor("_Color", Color.green);
        }
        //this takes care of processing the result on chips
        m_ChipInstantiation.ProcessBetResult(betResultIsRed, tempBetData);
        //this takes care of processing the result on UI
        m_UIManager.ProcessBetResult();
    }

}
