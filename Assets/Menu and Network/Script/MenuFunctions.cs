using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MenuFunctions : MonoBehaviour
{
    public GameObject[] menuLayers;
    public DualOperationsNetworkRoomManager roomManager;

    void Start()
    {
        roomManager = (DualOperationsNetworkRoomManager)FindObjectOfType(typeof(DualOperationsNetworkRoomManager));

        if(roomManager != null)
        {
            roomManager.menu = this;
            SwitchLayer(roomManager.layer);
        }
    }

    public void HostGame()
    {
        roomManager.StartHost();
        roomManager.layer = 0;
    }

    // Take address from input field and connect as client
    public void JoinGame()
    {
        roomManager.StartClient();
        roomManager.layer = 1;
    }

    // Start the game with the current players
    public void PlayGame()
    {
        roomManager.StartGame();
    }

    public void StopAndGoBack(int n)
    {
        roomManager.BreakConnection();
        SwitchLayer(n);
    }

    // Display a different part of the menu
    public void SwitchLayer(int n)
    {
        // Cut network-connection

        bool check;
        for (int i = menuLayers.Length - 1; i >= 0; i--)
        {
            check = n == i;
            menuLayers[i].SetActive(check);
        }
    }

    // Shut down the game
    public void Quit()
    {
        SwitchLayer(0);
        Application.Quit();
        Debug.Log("Quit message");
    }
}