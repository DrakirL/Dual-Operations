using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AgentTutorial : NetworkBehaviour
{
    [SerializeField] enum PlayerType
    {
        Agent,
        Hacker
    }
    [Tooltip("Hacker not yet implemented")]
    [SerializeField] PlayerType playerType;
    [TextArea(10,10)]
    [SerializeField] string tutorialText;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            GetPlayer.Instance.addCanvasText(tutorialText);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {        
        if (playerType == PlayerType.Agent)
        {
            if (collision.transform.tag == "Player")
            {
                GetPlayer.Instance.addCanvasText(tutorialText);
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (playerType == PlayerType.Agent)
        {
            if (collision.transform.tag == "Player")
            {
                GetPlayer.Instance.removeCanvasText();
            }
        }
    }
}
