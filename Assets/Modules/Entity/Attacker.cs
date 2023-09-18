using UnityEngine;

public abstract class Attacker : MonoBehaviour
{
    public abstract void StartAttack(Transform target);
    public abstract void StopAttack();
}