using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using static UtilityClass;


public class MyGameManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private LocalGameManager m_LocalGameManager;
    private PhotonView myPhotonView;
    [SerializeField]
    private bool betResultIsRed;
    [SerializeField]
    private List<UtilityClass.BetData> betDataList;
    [SerializeField]
    private int playersrequiredToProcessBet = 2;
    #region Photon Callbacks

    /// &lt;summary&gt;
    /// Called when the local player left the room. We need to load the launcher scene.
    /// &lt;/summary&gt;
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        myPhotonView= GetComponent<PhotonView>();
        betDataList = new List<UtilityClass.BetData>();
    }
    #endregion
    #region Private Methods
    //The Master is the one that decides which color is revealed to both players
    void RevealColor()
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        if (randomNumber == 0)
        {
            betResultIsRed = true;
        }
        else
        {
            betResultIsRed = false;
        }
        myPhotonView.RPC("ProcessResult", RpcTarget.All, betResultIsRed);
    }
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion
    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    //this is the RPC call to inform everyone that that player bet how many stacks on which coller
    public void BetRPCCall(bool IsMaster, int amount, bool isRed)
    {
        myPhotonView.RPC("BetRPC", RpcTarget.All, IsMaster, amount, isRed);
    }
    #endregion
    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }


    }
    #endregion
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //The Master is the one that decides which color is revealed to both players
            if (betDataList.Count == playersrequiredToProcessBet)
            {
                RevealColor();
            }
        }
    }
   
    #region PUNRPC

    [PunRPC]
    public void BetRPC(bool IsMaster,int amount, bool isRed, PhotonMessageInfo photonMessageInfo)
    {
        BetData myBetData= new BetData();
        myBetData.PlayerIndex = IsMaster ? 0 : 1;
        myBetData.amount = amount;
        myBetData.betcolor = UtilityClass.ToColor(isRed);
        betDataList.Add(myBetData);
    }

    [PunRPC]
    public void ProcessResult(bool betResultIsRed,PhotonMessageInfo photonMessageInfo)
    {
        m_LocalGameManager.ProcessResult(betResultIsRed, betDataList);
        //this is to reset the game
        betDataList.Clear();
    }
    #endregion
}


