using UnityEngine;

public class SlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim;
    public GameObject door;
    public new GameObject light;
    public Sprite lightSpriteOn;
    public Sprite lightSpriteOff;

    BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        col.enabled = DoorIsClosed();

        if (DoorIsClosed())
            light.GetComponent<SpriteRenderer>().sprite = lightSpriteOff;
        else
            light.GetComponent<SpriteRenderer>().sprite = lightSpriteOn;
        

    }

    public void GetInteracted()
    {       
        if(DoorIsClosed())
            anim.Play("Open");
    }

    bool DoorIsClosed() => (anim.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
