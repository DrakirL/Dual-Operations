using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinDocument : MonoBehaviour, IInteractable
{

    //private Image img;

    //void Start()
    //{
    //    img = GameObject.FindGameObjectWithTag("USB");
    //    img.enabled = false;
    //}

    void Interact()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        // int layerMask = 1 << 8;

        // Invert bitmask to collide against everything except layer 8.
        // layerMask = ~layerMask;

        LayerMask layerMask = LayerMask.GetMask("Interactable");

        // Raycast from mouse position
        RaycastHit hit;
        Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, interactRange, layerMask))
        {

            if (interactTextOn)
                interactText.SetActive(true);
            // Does the ray intersect any objects excluding the player layer
            if (Input.GetKeyDown(KeyCode.E))//.ECGetButtonDown("Interact"))
            {
                // Get the component that is being interacted with
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.GetInteracted(objectPlayerCanInterRactWith); //????

                    // Sound Pls
                    DualOperationsAudioPlayer.Instance.Interact();
                }
            }
        }
        else
         if (interactTextOn)
            interactText.SetActive(false);
        {
        GameManager._instance.winState = true;
       // img.enabled = true;
        Destroy(this.gameObject);
    }
}
