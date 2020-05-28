using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using Unity;

public class MenuFunctions : MonoBehaviour
{
    public GameObject[] menuLayers;
    public DualOperationsNetworkRoomManager roomManager;
    public InputField inputField;
    [SerializeField] Color readyColor, unReadyColor;
    [SerializeField] Image Host, Client;
    bool ready = false;


    void Start()
    {
        //Undo manipulation of the cursor
        Cursor.lockState = CursorLockMode.None;

        //Find the NetworkRoomManager
        roomManager = (DualOperationsNetworkRoomManager)FindObjectOfType(typeof(DualOperationsNetworkRoomManager));
        if(roomManager != null)
        {
            roomManager.menu = this;
            SwitchLayer(roomManager.layer);
            
            if (inputField != null)
            {
                inputField.text = roomManager.networkAddress;
            }
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
        if (inputField != null)
        {
            roomManager.networkAddress = inputField.text;
        }

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

    // Roll the credits
    public void Credits()
    {
        SceneManager.LoadScene("Credtis", LoadSceneMode.Single);
        Debug.Log("Credits message. You should change scene first.");
    }

    // Shut down the game
    public void Quit()
    {
        SwitchLayer(0);
        Application.Quit();
        Debug.Log("Quit message");
    }

    public void Ready(Image image)
    {
        ready = !ready;
        Color color;

        if(ready)
        {
            color = readyColor;
        }
        else
        {
            color = unReadyColor;
        }
        image.color = color;

        roomManager.ConfirmReady();
    }

    public void ManagePlayers(int amount)
    {

    }
}