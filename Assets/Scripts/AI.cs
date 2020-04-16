﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateStuff;

public class AI : MonoBehaviour
{
    //   public bool switchState = false;
    public Transform[] goal;
    public StateMachine<AI> stateMachine { get; set; }
    public int destPoint = 0;
    public GameObject radio;
    public bool radioTurnOff;
    public bool fovtest123 = false;
    public float maxAngle = 50;
    public float maxRadius = 5;
    public Transform player;
    [HideInInspector]
    public GameObject detectCount;
    [HideInInspector]
    public ParticleSystem warning;
        


    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        detectCount = GameObject.FindGameObjectWithTag("DetectionCounter");
        warning = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        radioTurnOff = radio.GetComponent<radioInterract>().on;
        stateMachine.Update();
    }
}
