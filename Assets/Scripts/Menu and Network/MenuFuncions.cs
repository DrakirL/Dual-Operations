using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class MenuFuncions : MonoBehaviour
{
    public GameObject[] menuLayers;

    public void HostGame()
    {
        // 
    }

    // Take address from input field and connect as client
    public void JoinGame()
    { 
        
    }

    // Start the game with the current players
    public void PlayGame()
    {
        // If connection has been established
        SceneManager.LoadScene(1);
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