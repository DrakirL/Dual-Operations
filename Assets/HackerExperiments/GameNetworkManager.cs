using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    /*
    public class GameNetworkManager : NetworkManager
    {
        int amountOfPlayer = 0;
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if(amountOfPlayer == 0)
            {
                GameObject newPlayer = Object.Instantiate(agentPrefab);
                NetworkServer.AddPlayerForConnection(conn, newPlayer);
                amountOfPlayer++;
                
                //CanvasManager.Instance.openCanvas(true, conn.connectionId);
            }
            else if (amountOfPlayer == 1)
            {
                GameObject newPlayer = Object.Instantiate(hackerPrefab);
                NetworkServer.AddPlayerForConnection(conn, newPlayer);
                amountOfPlayer++;
                //CanvasManager.Instance.openCanvas(false, conn.connectionId);
            }
            else
            {
                Debug.LogError("to many player");
            }
        }
    }
    */
}
