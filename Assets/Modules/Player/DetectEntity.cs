using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectEntity : MonoBehaviour
{
    
    public float viewAngle = 90f; // 시야각도 (도)
    public float viewDistance = 20; // 시야 거리

    public LayerMask targetMask ; // 감지 대상 레이어 마스크
    public LayerMask obstacleMask;// 장애물 레이어 마스크

    public bool ing;
    public Action<bool, Transform> DetectAction { get; set; }
    
    // Start is called before the first frame update
    public void Init(Action<bool, Transform> detectAction)
    {
        DetectAction = detectAction;
        StartCoroutine(Detect());
    }
    
    IEnumerator Detect()
    {
        while (ing)
        {
            DetectEntities();
            //OnDrawGizmosSelected();
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DetectEntities()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewDistance, targetMask);

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right, dirToTarget) < viewAngle/2)
            {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                var hits = Physics2D.RaycastAll(transform.position, dirToTarget, dstToTarget, obstacleMask);
                var hit = hits.Where(x => !x.collider.CompareTag("Bullet")).FirstOrDefault();
                
                if (hit.collider == null && dstToTarget < closestDistance)
                {
                    closestTarget = target;
                    closestDistance = dstToTarget;
                }
            }
        }

        if (closestTarget != null && !closestTarget.CompareTag("ENV"))
        {
            DetectAction?.Invoke(true, closestTarget);
        }
        else
        {
            DetectAction?.Invoke(false, null);
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
