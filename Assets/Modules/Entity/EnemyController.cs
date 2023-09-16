using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Trace
    }
    
    public bool IsActive { get; set; }
    public State CurrentState { get; set; }

    private NavMeshAgent _myNavAgent;
    private void Start()
    {
        _myNavAgent = GetComponent<NavMeshAgent>();
        _myNavAgent.updateUpAxis = false;
        _myNavAgent.updateRotation = false;
        
        IsActive = true;
        CurrentState = State.Idle;
        StartCoroutine(ActiveState());
    }

    IEnumerator ActiveState()
    {
        while (IsActive)
        {
            switch (CurrentState)
            {
                case State.Idle : 
                case State.Move : 
                    _myNavAgent.SetDestination(RandomNavmeshLocation(4f));
                    break;
                case State.Trace : break;
            }
            yield return null;
        }       
    }

    private void Update()
    {
        CheckPlayerInSight();
    }

    Vector2 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector2 finalPosition = Vector2.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }

    private void CheckPlayerInSight()
    {
        Vector2 dir = GameManager.I.Player.transform.position - transform.position;
        Vector2 trans = transform.position;
        
        var hit = Physics2D.Raycast(trans, dir.normalized, dir.magnitude);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("SEE");
                // CurrentState = State.Trace;
            }
        }
    }
}
