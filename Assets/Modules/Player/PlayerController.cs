using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Serialization;

public enum PlayerState
{
    Idle,
    Move
}
public class PlayerController : MonoBehaviour
{
    #region Player Info Property
    public NavMeshAgent MyNavAgent { get; private set; }
    public Vector3 Position => transform.position;
    public Transform ArrowPivotTr;

    #endregion

    private PlayerState CurrentPlayerState { get; set; }

    private Queue<PlayerCommand> _playerCmdQ = new();
    public bool IsAction { get; set; }
    
    public float viewAngle = 90f; // 시야각도 (도)
    public float viewDistance = 20; // 시야 거리

    public LayerMask targetMask ; // 감지 대상 레이어 마스크
    public LayerMask obstacleMask;// 장애물 레이어 마스크
    
    private void Awake()
    {
        GameManager.I.Player = gameObject;
    }

    private void Start()
    {
        MyNavAgent = GetComponent<NavMeshAgent>();
        MyNavAgent.updateUpAxis = false;
        MyNavAgent.updateRotation = false;

        IsAction = false;
        CurrentPlayerState = PlayerState.Idle;
        StartCoroutine(nameof(ActiveState));
    }

    IEnumerator ActiveState()
    {
        PlayerCommand action = null;
        while (true)
        {
            yield return new WaitUntil(() => !IsAction && _playerCmdQ.TryDequeue(out action));
            
            IsAction = true;
            yield return action.Execute();
            IsAction = false;
            
            if(_playerCmdQ .Count == 0) Debug.Log("Queue End!");
        }
    }

    void AddAction(PlayerCommand cmd)
    {
        _playerCmdQ.Enqueue(cmd);
    }

    public void AddMoveAction(Vector3 position, int idx, Action<int> callback)
    {
        var cmd = new MoveCommand(this, PlayerState.Move, position, idx, callback);
        AddAction(cmd);
    }
    
    public void ClearAction()
    {
        _playerCmdQ.Clear();
        IsAction = false;
    }

    public void TowardToAngle(Quaternion Angles)
    {
        var dir = Angles.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        ArrowPivotTr.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f,0f, angle * Mathf.Rad2Deg - 90f), 5);
    }
    
    void DetectEnemies()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    Debug.Log("적 발견: " + target.name);
                    // 여기에서 적을 처리하거나 원하는 작업을 수행하세요.
                }
            }
        }
    }
}
