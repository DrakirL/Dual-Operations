using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class DualOperationsNetworkRoomManager : NetworkRoomManager
{
    public MenuFunctions menu;
    public int layer = 0;

    private void Start()
    {

        /*Debug.Log("ok");
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            for (int i = 0; i < roomSlots.Count; i++)
            {
                Debug.Log("ok2");
                Destroy(roomSlots[i].gameObject);
            }
            roomSlots.Clear();
        }
        */
    }

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
        //PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
        //playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
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

    public void ConfirmReady()
    {
        for(int i = 0; i < roomSlots.Count; i++)
        {
            if (NetworkClient.active && roomSlots[i].isLocalPlayer)
            {
                if (roomSlots[i].readyToBegin)
                    roomSlots[i].CmdChangeReadyState(false);

                else
                    roomSlots[i].CmdChangeReadyState(true);
            }
        }
    }

    public override void SceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer)
    {
        if (LogFilter.Debug) Debug.LogFormat("NetworkRoom SceneLoadedForPlayer scene: {0} {1}", SceneManager.GetActiveScene().name, conn);

        if (SceneManager.GetActiveScene().name == RoomScene)
        {
            // cant be ready in room, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            return;
        }

        GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);
        if (gamePlayer == null)
        {
            // get start position from base class
            Transform startPos = GetStartPosition();

            //Only player 0 becomes a hacker
            if (roomPlayer.GetComponent<NetworkRoomPlayer>().index == 0)
            {
                gamePlayer = startPos != null
                ? Instantiate(hackerPrefab, startPos.position, startPos.rotation)
                : Instantiate(hackerPrefab, Vector3.zero, Quaternion.identity);
                gamePlayer.name = hackerPrefab.name;
            }

            else
            {
                gamePlayer = startPos != null
                ? Instantiate(agentPrefab, startPos.position, startPos.rotation)
                : Instantiate(agentPrefab, Vector3.zero, Quaternion.identity);
                gamePlayer.name = agentPrefab.name;
            }
            
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }

    /*
    public override void OnRoomServerConnect(NetworkConnection conn)
    {
        if (menu != null)
            menu.ManagePlayers(numPlayers+1);
    }

    public override void OnRoomServerDisconnect(NetworkConnection conn)
    {
        if (menu != null)
            menu.ManagePlayers(numPlayers-1);
    }
    */
}