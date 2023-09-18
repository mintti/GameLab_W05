using System.Collections;
using UnityEngine;

public class Attacker : AttackerBase
{
    public int damage;
    public override void StartAttack(Transform target)
    {
        StartCoroutine(Attack(target));
    }

    public override void StopAttack()
    {
        // nothing   
    }

    IEnumerator Attack(Transform target)
    {
        target.BroadcastMessage("GetDamage", damage);
        yield return new WaitForSeconds(attackInterval);
    }
}