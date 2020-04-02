using UnityEngine;

public class SlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim;
    public GameObject door;

    BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        col.enabled = DoorIsClosed();
    }

    public void GetInteracted()
    {       
        if(DoorIsClosed())
            anim.Play("Open");
    }

    bool DoorIsClosed() => (anim.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
