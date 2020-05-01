using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{
    [SerializeField] bool activated;
    [SerializeField] GameObject[] doors;

    private void Start()
    {
        activated = false;
        foreach (GameObject door in doors)
        {
            door.GetComponent<BoxCollider>().enabled = false;
            door.GetComponent<SlideDoor>().active = false;
        }
    }

    public void GetInteracted()
    {
        if (!IsActivated())
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<BoxCollider>().enabled = true;
                door.GetComponent<SlideDoor>().active = true;
            }
            activated = true;
        }

        Debug.Log("Generator Activated");


    }

    public bool IsActivated() => (activated) ? true : false;
}