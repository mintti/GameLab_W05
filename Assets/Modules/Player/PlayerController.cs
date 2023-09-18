using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private TowardToAngle _towardToAngle;
    [SerializeField] private NavMeshAgent _myNavAgent;

    [Header("Infomation")]
    private Queue<PlayerCommand> _playerCmdQ = new();
    public bool IsAction { get; set; }
    private bool _isAlive;
    public Vector3 Position => transform.position;

    private void Start()
    {
        _towardToAngle = GetComponentInChildren<TowardToAngle>();
        _towardToAngle.Detect(new Quaternion());
        
        _myNavAgent = GetComponent<NavMeshAgent>();
        _myNavAgent.updateUpAxis = false;
        _myNavAgent.updateRotation = false;

        _isAlive = true;
        IsAction = false;
        
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
    
    public void AddMoveAction(Vector3 position, int idx, Action<int> callback)
    {
        var cmd = new MoveCommand(this, position, idx, callback);
        _playerCmdQ.Enqueue(cmd);
    }

    public void OrderMove(Vector3 dir)
    {
        _myNavAgent.SetDestination(dir);
    }
    
    public void ClearAction()
    {
        _playerCmdQ.Clear();
        IsAction = false;
    }

    public bool IsPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        return NavMesh.SamplePosition(position, out hit, 0.1f, NavMesh.AllAreas);
    }
}
