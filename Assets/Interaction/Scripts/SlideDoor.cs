using UnityEngine;
using Mirror;

public class SlideDoor : NetworkBehaviour, IInteractable
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
       // NetworkIdentity.AssignClientAuthority(GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient));
        //NetworkIdentity.AssignClientAuthority(NetworkServer.connections[0]);
        //NetworkIdentity.clientAuthorityCallback(this, GetComponent<NetworkIdentity>(), true);
        //NetworkIdentity.AssignClientAuthority(NetworkServer.connections[0].connectionId);
        // NetworkIdentity.AssignClientAuthority(GetPlayer.Instance.getPlayer().GetComponent<NetworkIdentity>());
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
        if (isServer)
        {
            Debug.Log("I'm the server (or host)");
            RpcPlayOpenAnimation();
        }
        else
        {
            if (hasAuthority)
            {

                Debug.Log("I'm the client");
                CmdCallServertoOpenDoor();
            }

        }
        Debug.Log("1");
    }
    

    [Command]
    private void CmdCallServertoOpenDoor()
    {
        RpcPlayOpenAnimation();
        Debug.Log("2");
    }

    [ClientRpc]
    private void RpcPlayOpenAnimation()
    {
        Debug.Log("3");
        if (DoorIsClosed())
        {
            if (anim1 != null)
                anim1.Play("Open");
            if (anim2 != null)
                anim2.Play("Open");
        }
    }

    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
