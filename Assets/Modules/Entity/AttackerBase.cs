using UnityEngine;

public abstract class AttackerBase : MonoBehaviour
{
    public float attackInterval;
    public abstract void StartAttack(Transform target);
    public abstract void StopAttack();
}