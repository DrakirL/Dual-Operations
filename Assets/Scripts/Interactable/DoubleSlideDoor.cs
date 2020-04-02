using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim1;
    public Animator anim2;
    public BoxCollider col;

    private void Update()
    {
        EnableDoor(DoorIsClosed());        
    }
    
    // Checks current animation status
    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;

    public void GetInteracted()
    {
        OpenDoor();
    }

    // Play animation for door open, closes itself in animation
    void OpenDoor()
    {
        anim1.Play("Open");
        anim2.Play("Open");
    }

    // Sets the trigger collider status
    void EnableDoor(bool boolToSet)
    {
        col.enabled = boolToSet;
    }
    
}
