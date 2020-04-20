using UnityEngine;

public class SlideDoor : MonoBehaviour, IInteractable
{
    [Header("These fields are not required and can be empty")]
    public Animator anim1;
    public Animator anim2;
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

        if(light != null)
        {
            if (DoorIsClosed())
                light.GetComponent<SpriteRenderer>().sprite = lightSpriteOff;
            else
                light.GetComponent<SpriteRenderer>().sprite = lightSpriteOn;
        }      
    }

    public void GetInteracted()
    {
        if (DoorIsClosed())
        {
            if(anim1 != null)
            anim1.Play("Open");
            if (anim2 != null)
            anim2.Play("Open");
        }          
    }

    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
