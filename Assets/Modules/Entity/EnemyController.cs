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

    public State _curState;

    public State CurrentState
    {
        get => _curState;
        set
        {
            if (_curState != value)
            {
                traceMark.SetActive(value == State.Trace);
            }
            _curState = value;
        }
    }

    public LayerMask targetMask;  // 감지 대상 레이어 마스크
    public LayerMask obstacleMask;// 장애물 레이어 마스크
    
    public GameObject traceMark;

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
            CheckPlayerInSight();
            switch (CurrentState)
            {
                case State.Idle : 
                case State.Move : 
                    _myNavAgent.SetDestination(RandomNavmeshLocation(10f));
                    break;
                case State.Trace :
                    _myNavAgent.SetDestination(PlayerTr.position);
                    break;
            }
            yield return null;
        }       
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
        Vector2 dir = PlayerTr.position - transform.position;
        Vector2 trans = transform.position;
        
        var hit = Physics2D.Raycast(trans, dir.normalized, dir.magnitude, targetMask);


        //var firstHit = hit.FirstOrDefault(x => !x.collider.CompareTag("Bullet"));
        if (hit.collider != null)
        {
            bool isCatchPlayer = hit.collider.gameObject.CompareTag("Player");
            CurrentState = isCatchPlayer ? State.Trace : State.Move;
        }
    }

    private Transform PlayerTr => GameManager.I.Player.transform;
}
