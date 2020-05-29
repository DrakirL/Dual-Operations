using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsBonusSpeed : MonoBehaviour
{
    [SerializeField] float bonusSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * bonusSpeed * Time.deltaTime;
    }
}
