using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIUPArrow : MonoBehaviour
{
    private PlayerController order;
    public bool IsActive { get; set; } = false;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (IsActive)
        {
            if (col.CompareTag("Player"))
            {
                // [TODO] 해당 객체를 생성한 주인인지 체크 필요 
                col.GetComponentInChildren<TowardToAngle>().Detect(transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
