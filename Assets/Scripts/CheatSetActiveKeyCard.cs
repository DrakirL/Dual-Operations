using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSetActiveKeyCard : MonoBehaviour
{

    [SerializeField] public Collider activeCollider;
    [SerializeField] public GameObject relatedKeyCard;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I´m in!");
        if (Input.GetKey("e"))
        {
            relatedKeyCard.SetActive(true);
            Debug.Log("I´m too far in");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
