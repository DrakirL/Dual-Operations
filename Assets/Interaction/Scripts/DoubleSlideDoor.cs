using UnityEngine;

public class DoubleSlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim1;
    public Animator anim2;
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
        if (DoorIsClosed())
        {
            anim1.Play("Open");
            anim2.Play("Open");
        }
    }

    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
