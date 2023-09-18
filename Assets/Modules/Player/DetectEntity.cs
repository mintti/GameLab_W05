using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEntity : MonoBehaviour
{
    private Shooter _shooter { get; set; }
    
    public float viewAngle = 90f; // 시야각도 (도)
    public float viewDistance = 20; // 시야 거리

    public LayerMask targetMask ; // 감지 대상 레이어 마스크
    public LayerMask obstacleMask;// 장애물 레이어 마스크

    public bool ing;
    
    // Start is called before the first frame update
    public void Init(Shooter shooter)
    {
        _shooter = shooter;
        
        StartCoroutine(Detect());
    }
    
    IEnumerator Detect()
    {
        while (ing)
        {
            DetectEnemies();
            //OnDrawGizmosSelected();
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DetectEnemies()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance, targetMask);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask);
 
                if (hit.collider == null && dstToTarget < closestDistance)
                {
                    closestTarget = target;
                    closestDistance = dstToTarget;
                }
            }
        }

        if (closestTarget != null)
        {
            Debug.Log("가장 가까운 적 발견: " + closestTarget.name);
            _shooter.StartAttack(closestTarget); // 가장 가까운 적을 타겟으로 설정
        }
        else
        {
            _shooter.StopAttack();
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    }

    Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}
