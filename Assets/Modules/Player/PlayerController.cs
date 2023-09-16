using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent _myNavAgent;
    private void Awake()
    {
        GameManager.I.Player = this.gameObject;
    }

    private void Start()
    {
        _myNavAgent = GetComponent<NavMeshAgent>();
        _myNavAgent.updateUpAxis = false;
        _myNavAgent.updateRotation = false;
    }
}
