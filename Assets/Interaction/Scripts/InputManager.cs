﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 5f;
    public bool interactTextOn;
    public GameObject interactText;

    void Update()
    {
        Interact();
    }

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

            if(interactTextOn) 
				interactText.SetActive(true);
            // Does the ray intersect any objects excluding the player layer
            if (Input.GetKeyDown(KeyCode.E))//.ECGetButtonDown("Interact"))
            {                
                // Get the component that is being interacted with
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.GetInteracted();
                }                  
            }
        }
        else
         if(interactTextOn) 
			 interactText.SetActive(false);
    }
}