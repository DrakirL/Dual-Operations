using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class DualOperationsNetworkRoomManager : NetworkRoomManager
{
    public MenuFunctions menu;
    public int layer = 0;

    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
        playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        return true;
    }

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        if (isHeadless)
        base.OnRoomServerPlayersReady();
        
        else
        showStartButton = true;
    }

    public void StartGame()
    {
        if (allPlayersReady && showStartButton)
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
            menu = null;
        }
    }

    public void BreakConnection()
    {
        StopClient();
        StopHost();
        Debug.Log("Should return you one step back and leave room");
    }
}