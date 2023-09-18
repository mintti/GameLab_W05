using System;
using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public abstract class PlayerCommand : MonoBehaviour
{
    protected PlayerController Controller { get; }
    protected PlayerState PlayerState { get; }
    protected PlayerCommand(PlayerController controller, PlayerState state )
    {
        Controller = controller;
        PlayerState = state;
    }

    public abstract IEnumerator Execute();
}

public class MoveCommand : PlayerCommand
{
    public Vector3 MovePosition { get; }
    public int Index { get; }
    public Action<int> EndAction { get; }
    
    public MoveCommand(PlayerController controller, PlayerState state, Vector3 movePosition, int idx, Action<int> endCb) : base(controller, state)
    {
        MovePosition = movePosition;
        Index = idx;
        EndAction = endCb;
    }
    
    public override IEnumerator Execute()
    {       
        Controller.MyNavAgent.SetDestination(MovePosition);
        while (!(Vector3.SqrMagnitude(new Vector2(Controller.Position.x, Controller.Position.y) - (Vector2)MovePosition) < 0.1))
        {   
            yield return new WaitForSeconds(0.1f);
        }
        EndAction?.Invoke(Index);
    }
}

public class AngleCommand : MoveCommand
{
    public Vector3 Angles { get; }

    public AngleCommand(PlayerController controller, PlayerState state, Vector3 movePosition, int idx, Action<int> endCb, Vector3 angles) : base(controller, state, movePosition, idx, endCb)
    {
        Angles = angles;
    }
    
    public override IEnumerator Execute()
    {
        var dir = Angles.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        Controller.ArrowPivotTr.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f,0f, angle * Mathf.Rad2Deg - 90f), 5);
        yield return base.Execute();
    }
}